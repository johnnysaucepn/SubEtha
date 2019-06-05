using Nancy.Authentication.Basic;
using Nancy.Security;

namespace Howatworks.Matrix.Site
{
    public class StubValidator : IUserValidator
    {
        public IUserIdentity Validate(string user, string password)
        {
            if (password == "nopassword")
            {
                return new MatrixUser(user);
            }
            return null;
        }
    }
}