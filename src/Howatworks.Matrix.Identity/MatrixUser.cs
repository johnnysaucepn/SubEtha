using System;
using Microsoft.AspNetCore.Identity;

namespace Howatworks.Matrix.Identity
{
    public class MatrixUser : IdentityUser
    {
        [PersonalData]
        public string CmdrName { get; set; }

        public MatrixUser(string userName, string cmdrName) : base(userName)
        {
            CmdrName = cmdrName;
        }

        public MatrixUser()
        {
        }

    }
}
