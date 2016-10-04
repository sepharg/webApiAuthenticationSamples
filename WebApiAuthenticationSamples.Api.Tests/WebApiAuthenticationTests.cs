using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using WebApiAuthenticationSamples.Api.Tests.Helpers;

namespace WebApiAuthenticationSamples.Api.Tests
{
    [TestClass]
    public class WebApiAuthenticationTests
    {
        private static IDisposable _webApp;

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            _webApp = WebApp.Start<Startup>("http://*:9443/");
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            _webApp.Dispose();
        }

        [TestMethod]
        public async Task ValidUserPassword_BasicAuth_Passes()
        {
            using (var httpClient = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes("validUser:validUser");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var requestUri = new Uri("http://localhost:9443/api/authenticationTest/testbasicauth");
                HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                // ... Check Status Code                                
                Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [TestMethod]
        public void ValidUserPassword_DigestAuth_Passes()
        {
            var client = new RestClient(@"http://localhost:9443/")
            {
                Authenticator = new DigestAuthenticator("validUser", "validUser")
            };

            var request = new RestRequest
            {
                Resource = "api/authenticationTest/testdigestauth",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            };

            var response = client.Execute(request);

            // ... Check Status Code                                
            Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
