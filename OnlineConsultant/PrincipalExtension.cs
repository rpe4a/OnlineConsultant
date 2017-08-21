using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace OnlineConsultant
{
    public static class PrincipalExtension
    {
        private static Claim FindClain(IPrincipal user, string clainType)
        {
            var userIdentity = ((ClaimsIdentity)user.Identity).Claims;

            return userIdentity.FirstOrDefault(x => x.Type == clainType);
        }

        public static int GetUserId(this IPrincipal user)
        {
            var identifier = FindClain(user,ClaimTypes.NameIdentifier);
            return Convert.ToInt32(identifier.Value);
        }

        public static bool IsSpec(this IPrincipal user)
        {
            Claim role = FindClain(user,ClaimTypes.Role);
            return Convert.ToBoolean(role.Value);
        }

    }
}