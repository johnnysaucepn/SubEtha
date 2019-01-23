using System;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Security;

namespace SubEtha.Site
{
    public class SiteNancyModule : NancyModule
    {
        public SiteNancyModule(IConfiguration config)
        {
            var configReader = config.GetSection("SubEtha.Site");
            var serviceUri = new Uri(configReader["ServiceUri"]);
            IUserValidator userValidator = new StubValidator();

            this.RequiresAuthentication();
            this.EnableBasicAuthentication(new BasicAuthenticationConfiguration(userValidator, "SubEtha", UserPromptBehaviour.Always));
            this.RequiresClaims("User");

            Get["/"] = parameters => View["Index"];

            Get["/Location"] = parameters =>
            {
                dynamic model = new {
                    ServiceBaseUri = serviceUri,
                    User = Context.CurrentUser as SubEthaUser
                };

                return View["Location", model];
            };

            Get["/LiveTracking"] = parameters =>
            {
                dynamic model = new
                {
                    ServiceBaseUri = serviceUri,
                    User = Context.CurrentUser as SubEthaUser
                };

                return View["LiveTracking", model];
            };

        }

    }
}