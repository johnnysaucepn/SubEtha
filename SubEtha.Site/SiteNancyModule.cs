using System;
using Howatworks.Configuration;
using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Security;

namespace SubEtha.Site
{
    public class SiteNancyModule : NancyModule
    {
        public SiteNancyModule(IConfigLoader configLoader)
        {
            var configReader = configLoader.GetConfigurationSection("SubEtha.Site");
            var serviceUri = new Uri(configReader.Get<string>("ServiceUri"));
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