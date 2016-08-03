using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApiAuthenticationSamples.Api.Models;
using WebApiAuthenticationSamples.Api.Models.DigestAuthentication;

namespace WebApiAuthenticationSamples.Api.Filters
{
    public class DigestAuthenticationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                var request = actionContext.Request;
                if (actionContext.Request.Headers.Authorization == null)
                {
                    // first request from client will not have any auth headers
                    UnauthorizedResponse(actionContext);
                }
                else
                {
                    var header = new Header(request.Headers.Authorization.Parameter, request.Method.Method);

                    if (Nonce.IsValid(header.Nonce, header.NounceCounter))
                    {
                        // here would go your code to check username / password against your user data store. 
                        // I´m just gonna say that the credentials are valid if the username and password are the same for the purpose of the exercise.
                        string password = header.UserName;

                        string ha1 = $"{header.UserName}:{header.Realm}:{password}".ToMD5Hash();
                        string ha2 = $"{header.Method}:{header.Uri}".ToMD5Hash();
                        string computedResponse = $"{ha1}:{header.Nonce}:{header.NounceCounter}:{header.Cnonce}:{"auth"}:{ha2}".ToMD5Hash();

                        if (string.CompareOrdinal(header.Response, computedResponse) == 0)
                        {
                            // Computed digest matches the value sent by the client, we can create a principal here and pass authentication.
                            AuthenticationHelper.SetCurrentPrincipal(actionContext, header.UserName, password);
                        }
                        else // auth failure
                        {
                            UnauthorizedResponse(actionContext);
                        }
                    }
                    else // nonce failure
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private void UnauthorizedResponse(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Digest", Header.UnauthorizedResponseHeader.ToString()));
        }
    }
}