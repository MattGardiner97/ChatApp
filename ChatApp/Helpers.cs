using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ChatApp
{
    public static class Helpers
    {
        public static int GetCurrentUserID(ClaimsPrincipal Claim)
        {
            return int.Parse(Claim.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
            
    }
}
