using System;
using System.Text;

namespace WebApiAuthenticationSamples.Api.Models.DigestAuthentication
{
    public class Header
    {
        private Header() { }

        public Header(string header, string method)
        {
            string keyValuePairs = header.Replace("\"", String.Empty);

            foreach (string keyValuePair in keyValuePairs.Split(','))
            {
                var index = keyValuePair.IndexOf("=", StringComparison.Ordinal);
                var key = keyValuePair.Substring(0, index).Trim();
                var value = keyValuePair.Substring(index + 1).Trim();

                switch (key)
                {
                    case "username": { UserName = value; break;}
                    case "realm": { Realm = value; break;}
                    case "nonce": { Nonce = value; break;}
                    case "uri": { Uri = value; break;}
                    case "nc": { NounceCounter = value; break;}
                    case "cnonce": { Cnonce = value; break;}
                    case "response": { Response = value; break;}
                    case "method": { Method = value; break;}
                }
            }

            if (string.IsNullOrEmpty(Method))
            {
                Method = method;
            }
        }

        public string Cnonce { get; private set; }
        public string Nonce { get; private set; }
        public string Realm { get; private set; }
        public string UserName { get; private set; }
        public string Uri { get; private set; }
        public string Response { get; private set; }
        public string Method { get; private set; }
        public string NounceCounter { get; private set; }

        // This property is used by the handler to generate a
        // nonce and get it ready to be packaged in the
        // WWW-Authenticate header, as part of 401 response
        public static Header UnauthorizedResponseHeader
        {
            get
            {
                return new Header()
                {
                    Realm = "TestRealm",
                    Nonce = DigestAuthentication.Nonce.Generate()
                };
            }
        }

        public override string ToString()
        {
            StringBuilder header = new StringBuilder();
            header.AppendFormat("realm=\"{0}\"", Realm);
            header.AppendFormat(", nonce=\"{0}\"", Nonce);
            header.AppendFormat(", qop=\"{0}\"", "auth");
            return header.ToString();
        }
    }
}