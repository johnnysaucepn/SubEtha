using Nancy.Authentication.Basic;
using Nancy.Security;

namespace SubEtha.Site
{
    public class StubValidator : IUserValidator
    {
        public IUserIdentity Validate(string user, string password)
        {
            if (password == "nopassword")
            {
                return new SubEthaUser(user);
            }
            return null;
        }
    }
}