using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace CarStore.Authentication.Externals
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource("roles", new[] { JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("apis","My API"),                
                new ApiResource("web_mvc", "Web Mvc", new [] { JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                //Admin Mvc
                new Client
                {
                    ClientId = "web_mvc",
                    ClientName = "Web Mvc",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret(Commons.CarStoreConstants.AuthenSecretKey.Sha256()) },                                        
                    RedirectUris = { AdminMvcUrl  + "signin-oidc" },
                    FrontChannelLogoutUri = AdminMvcUrl  + "signout-oidc",
                    PostLogoutRedirectUris = { AdminMvcUrl  + "signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "apis", "roles" }

                },
                new Client
                {
                    ClientId = "nextjs_web_app",
                    ClientName = "NextJs Web App",
                    ClientSecrets = { new Secret(Commons.CarStoreConstants.AuthenSecretKey.Sha256()) },
                    AllowedGrantTypes =  GrantTypes.Code,
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:3000/api/auth/callback/identity-server4","http://localhost:3000" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:3000" },
                    AllowedCorsOrigins= { "http://localhost:3000" },
                    RequireClientSecret = false,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                    },

                },
                new Client
                {
                    ClientId = "react-clients",
                    ClientName = "react-clients",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,
                    RedirectUris = new List<string> {
                        "http://localhost:3000/authentication/login-callback",
                        "http://localhost:3000/"
                    },
                    PostLogoutRedirectUris = new List<string> {
                        "http://localhost:3000/authentication/logout-callback",
                        "http://localhost:3000/"
                    },
                    AllowedScopes = {
                                      IdentityServerConstants.StandardScopes.OpenId,
                                      IdentityServerConstants.StandardScopes.Profile,
                                      IdentityServerConstants.StandardScopes.Email,
                                      IdentityServerConstants.StandardScopes.Phone
                                     },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:3000",
                    },
                    AccessTokenLifetime = 86400
                }

            };
        }

        #region Config for Authentication
        public static string AdminMvcUrl { get; set; }
        #endregion
    }
}
