using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiAuthenticationSamples.Api.Startup))]

namespace WebApiAuthenticationSamples.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // Execute any other ASP.NET Web API-related initialization, i.e. IoC, authentication, logging, mapping, DB, etc.
            ConfigureAuth(app);
            app.UseWebApi(configuration);
            /*
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);*/
        }
    }
}
