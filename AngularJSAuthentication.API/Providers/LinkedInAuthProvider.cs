using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin.Security.Providers.LinkedIn;
using Owin;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AngularJSAuthentication.API.Providers
{
    public class LinkedInAuthProvider : ILinkedInAuthenticationProvider
    {
        public Task Authenticated(LinkedInAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(LinkedInReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}