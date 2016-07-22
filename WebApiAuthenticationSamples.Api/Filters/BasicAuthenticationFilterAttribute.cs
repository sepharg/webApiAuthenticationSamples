using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApiAuthenticationSamples.Api.Models;

namespace WebApiAuthenticationSamples.Api.Filters
{
    public class BasicAuthenticationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    var dnsHost = actionContext.Request.RequestUri.DnsSafeHost;
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    actionContext.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{dnsHost}\"");
                }
                else
                {
                    string authHeader = null;
                    var auth = actionContext.Request.Headers.Authorization;
                    if (auth != null && auth.Scheme == "Basic")
                    {
                        authHeader = auth.Parameter;
                    }
                    authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));
                    string[] httpRequestHeaderValues = authHeader.Split(':');
                    var username = httpRequestHeaderValues[0];
                    var password = httpRequestHeaderValues[1];

                    var identity = new BasicAuthenticationIdentity(username, password);
                    
                    if (!AreValidCredentials(identity))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    };

                    var principal = new GenericPrincipal(identity, null);

                    Thread.CurrentPrincipal = principal;
                    actionContext.RequestContext.Principal = principal;
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private bool AreValidCredentials(BasicAuthenticationIdentity identity)
        {
            // here would go your code to check username / password against your user data store. 
            // I´m just gonna say that the credentials are valid if the username and password are the same for the purpose of the exercise.
            return identity.Name.Equals(identity.Password);
        }
    }
}