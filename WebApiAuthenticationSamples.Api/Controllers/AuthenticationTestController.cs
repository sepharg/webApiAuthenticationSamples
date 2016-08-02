using System.Threading;
using System.Web.Http;
using WebApiAuthenticationSamples.Api.Filters;

namespace WebApiAuthenticationSamples.Api.Controllers
{
    [RoutePrefix("api/authenticationTest")]
    public class AuthenticationTestController : ApiController
    {
        /// <summary>
        /// Tests the basic authentication. For demo purposes, enter any value for username and password, but make sure they´re the same. i.e. : "user" / "user"
        /// </summary>
        /// <returns>A message indicating that authentication was successfull or Unauthorized HTTP code otherwise.</returns>
        [BasicAuthenticationFilter]
        [HttpGet]
        public string TestBasicAuth()
        {
            // if authentication fails, we won´t reach the controller and an Unauthorized response will be shortcircuited by the authentication filter.
            return $"User has been authenticated sucessfully, for username '{Thread.CurrentPrincipal.Identity.Name}'";
        }
    }
}
