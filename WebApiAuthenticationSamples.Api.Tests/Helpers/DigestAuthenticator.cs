using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace WebApiAuthenticationSamples.Api.Tests.Helpers
{
    public class DigestAuthenticator : IAuthenticator
    {
        private readonly string _user;
        private readonly string _pass;

        public DigestAuthenticator(string user, string pass)
        {
            _user = user;
            _pass = pass;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.Credentials = new NetworkCredential(_user, _pass);
            
            var url = client.BuildUri(request).ToString();
            var uri = new Uri(url);

            var digestAuthFixer = new DigestAuthFixer(client.BaseUrl.ToString(), _user, _pass);
            digestAuthFixer.GrabResponse(uri.PathAndQuery);
            var digestHeader = digestAuthFixer.GetDigestHeader(uri.PathAndQuery);
            request.AddParameter("Authorization", digestHeader, ParameterType.HttpHeader);
        }
    }

    public class DigestAuthFixer
    {
        private static string _host;
        private static string _user;
        private static string _password;
        private static string _realm;
        private static string _nonce;
        private static string _qop;
        private static string _cnonce;
        private static string _opaque;
        private static DateTime _cnonceDate;
        private static int _nc;

        public DigestAuthFixer(string host, string user, string password)
        {
            _host = host;
            _user = user;
            _password = password;
        }

        private string CalculateMd5Hash(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private string GrabHeaderVar(string varName, string header)
        {
            var regHeader = new Regex($@"{varName}=""([^""]*)""");
            var matchHeader = regHeader.Match(header);
            if (matchHeader.Success)
                return matchHeader.Groups[1].Value;
            throw new ApplicationException($"Header {varName} not found");
        }

        public string GetDigestHeader(string dir)
        {
            _nc = _nc + 1;

            var ha1 = CalculateMd5Hash($"{_user}:{_realm}:{_password}");
            var ha2 = CalculateMd5Hash($"{"GET"}:{dir}");
            var digestResponse =
            CalculateMd5Hash($"{ha1}:{_nonce}:{_nc:00000000}:{_cnonce}:{_qop}:{ha2}");

            return string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", " + "algorithm=MD5, response=\"{4}\", opaque=\"{8}\", qop={5}, nc={6:00000000}, cnonce=\"{7}\"", _user, _realm, _nonce, dir, digestResponse, _qop, _nc, _cnonce, _opaque);
        }

        public void GrabResponse(string dir)
        {
            var url = _host + dir;
            var uri = new Uri(url);

            var request = (HttpWebRequest)WebRequest.Create(uri);

            // If we've got a recent Auth header, re-use it!
            if (!string.IsNullOrEmpty(_cnonce) && DateTime.Now.Subtract(_cnonceDate).TotalHours < 1.0)
            {
                request.Headers.Add("Authorization", GetDigestHeader(dir));
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    throw;

                var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);

                _nc = 0;
                _opaque = GrabHeaderVar("opaque", wwwAuthenticateHeader);
                _cnonce = new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);
                _cnonceDate = DateTime.Now;
            }

        }
    }
}
