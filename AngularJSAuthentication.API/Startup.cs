using AngularJSAuthentication.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin.Security.Providers.LinkedIn;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(AngularJSAuthentication.API.Startup))]

namespace AngularJSAuthentication.API
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }
        public static LinkedInAuthenticationOptions linkedInAuthOptions { get; private set; }
        public static TwitterAuthenticationOptions twitterAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, AngularJSAuthentication.API.Migrations.Configuration>());
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions() {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "xxxxxx",
                ClientSecret = "xxxxxx",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1050543328368934",
                AppSecret = "78145c119c1d6c64dd64bf129cc3a0c2",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

            linkedInAuthOptions = new LinkedInAuthenticationOptions()
            {
                ClientId = "78h6qf8zyp15lg",
                ClientSecret = "I86DaIuxrfqsv3tW",
                Provider = new LinkedInAuthenticationProvider()
            };
            app.UseLinkedInAuthentication(linkedInAuthOptions);

            twitterAuthOptions = new TwitterAuthenticationOptions()
            {
                ConsumerKey = "oX140tWdlh4W0vEeD48xlfoaE",
                ConsumerSecret = "WnY8OooFOEEiyvB9nJLPEgtDP12HXAfcTOmEYfsEfNbFslQDHs",
                Provider = new TwitterAuthenticationProvider()
            };
            app.UseTwitterAuthentication(twitterAuthOptions);

        }
    }
}