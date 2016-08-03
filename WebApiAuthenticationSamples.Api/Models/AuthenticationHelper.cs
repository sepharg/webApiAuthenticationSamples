using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using WebApiAuthenticationSamples.Api.Models.BasicAuthentication;

namespace WebApiAuthenticationSamples.Api.Models
{
    public class AuthenticationHelper
    {
        public static void SetCurrentPrincipal(HttpActionContext actionContext, string userName, string password)
        {
            var identity = new BasicAuthenticationIdentity(userName, password);
            var principal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = principal;
            actionContext.RequestContext.Principal = principal;
        }
    }
}