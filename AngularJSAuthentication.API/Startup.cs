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
using Microsoft.Owin.Security;
using System.Net.Http;

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
                ClientId = "857698632407-2oh1dlmgvkcr3s1t7ogk2r1kvvr2t34v.apps.googleusercontent.com",
                ClientSecret = "EwDEtu5aecpoPO74wQTQNDDy",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1050543328368934",
                AppSecret = "78145c119c1d6c64dd64bf129cc3a0c2",
                Provider = new FacebookAuthProvider()
            };

            facebookAuthOptions.Scope.Add("email");

            app.UseFacebookAuthentication(facebookAuthOptions);

            linkedInAuthOptions = new LinkedInAuthenticationOptions()
            {
                ClientId = "78h6qf8zyp15lg",
                ClientSecret = "I86DaIuxrfqsv3tW",
                Provider = new LinkedInAuthProvider()
            };
            app.UseLinkedInAuthentication(linkedInAuthOptions);

            twitterAuthOptions = new TwitterAuthenticationOptions()
            {
                ConsumerKey = "6hiW6eeKKWNx00xBiICcbtVlP",
                ConsumerSecret = "kI5gRkew9tf0l1itwReiFj5L7dnFwWjTRSldh5IsmnAQrNc0Yn",
                Provider = new TwitterAuthProvider(),
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "‎add53f6680fe66e383cbac3e60922e3b4c412bed", // Symantec Class 3 EV SSL CA - G3
                    "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
                    "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
                })
            };
            app.UseTwitterAuthentication(twitterAuthOptions);

        }
    }
}