using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServerAspNetIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified

                }
            }
        };

    public static IEnumerable<ApiScope> ApiScope =>
        new ApiScope[]
        {
            new ApiScope(name: "api1", displayName: "identityApi"),
            new ApiScope(name: "api2", displayName: "weatherApi")
        };
    public static IEnumerable<ApiResource> ApiResource =>

       new ApiResource[]
       {
           new ApiResource("identity")
           {
               Scopes = {"api1"}
           },
           new ApiResource("weather")
           {
               Scopes = {"api2"}
           }
       };

    public static IEnumerable<Client> Clients =>
        new List<Client>
            {
                new Client
                {
                    ClientId  = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = {"api1"}
                },

                new Client
                {
                    ClientId = "web",
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:5002/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:5002/signout-callback-oidc"},

                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "verification",
                        "api1"
                    }
                },

                 new Client
                {
                    ClientId = "AngularClient",
                    //ClientSecrets = { new Secret("secret".Sha256())},
                    RequirePkce = true,
                    RequireClientSecret= false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"http://localhost:4200"},
                    PostLogoutRedirectUris = {"http://localhost:4200"},

                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser= true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "verification",
                        "api1"
                    }
                }

            };

}
