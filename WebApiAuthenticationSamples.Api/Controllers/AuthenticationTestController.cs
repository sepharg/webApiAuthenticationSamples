using System.Threading;
using System.Web.Http;
using WebApiAuthenticationSamples.Api.Filters;

namespace WebApiAuthenticationSamples.Api.Controllers
{
    [RoutePrefix("api/authenticationTest")]
    public class AuthenticationTestController : ApiController
    {
        /// <summary>
        /// Tests basic authentication. For demo purposes, enter any value for username and password, but make sure they´re the same. i.e. : "user" / "user"
        /// </summary>
        /// <returns>A message indicating that authentication was successfull or Unauthorized HTTP code otherwise.</returns>
        [BasicAuthenticationFilter]
        [HttpGet]
        [Route("api/authenticationTest/testbasicauth")]
        public string TestBasicAuth()
        {
            // if authentication fails, we won´t reach the controller and an Unauthorized response will be shortcircuited by the authentication filter.
            return $"User has been authenticated sucessfully using Basic Authentication, for username '{Thread.CurrentPrincipal.Identity.Name}'";
        }

        /// <summary>
        /// Tests digest authentication. For demo purposes, enter any value for username and password, but make sure they´re the same. i.e. : "user" / "user"
        /// </summary>
        /// <returns>A message indicating that authentication was successfull or Unauthorized HTTP code otherwise.</returns>
        [DigestAuthenticationFilter]
        [HttpGet]
        [Route("api/authenticationTest/testdigestauth")]
        public string TestDigestAuth()
        {
            // if authentication fails, we won´t reach the controller and an Unauthorized response will be shortcircuited by the authentication filter.
            return $"User has been authenticated sucessfully using Digest Authentication, for username '{Thread.CurrentPrincipal.Identity.Name}'";
        }
    }
}
