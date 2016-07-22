using System.Threading;
using System.Web.Http;
using WebApiAuthenticationSamples.Api.Filters;

namespace WebApiAuthenticationSamples.Api.Controllers
{
    [RoutePrefix("api/authenticationTest")]
    public class AuthenticationTestController : ApiController
    {
       [BasicAuthenticationFilter]
       [HttpGet]
        public string TestBasicAuth()
        {
            // if authentication fails, we won´t reach the controller and an Unauthorized response will be shortcircuited by the authentication filter.
            return $"User has been authenticated sucessfully, for username '{Thread.CurrentPrincipal.Identity.Name}'";
        }
    }
}
