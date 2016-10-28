﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Twitter;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AngularJSAuthentication.API.Providers
{
    public class TwitterAuthProvider : ITwitterAuthenticationProvider
    {
        public Task Authenticated(TwitterAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            context.Identity.AddClaim(new Claim("AccessTokenSecret", context.AccessTokenSecret));
            return Task.FromResult<object>(null);
        }
        public Task ReturnEndpoint(TwitterReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }

        public void ApplyRedirect(TwitterApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }
    }
}