using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using WebApiAuthenticationSamples.Api;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9443/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/authenticationTest/testbasicauth
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/authenticationTest/testbasicauth").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }
    }
}
