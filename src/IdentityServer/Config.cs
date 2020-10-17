// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[] {
                new ApiResource("api", "protected api"){
                    UserClaims = {
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Role,
                    },
                },
                new ApiResource("inbox", "inbox api"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client.ro",
                    ClientSecrets =
                    {
                        new Secret("please-type-the-secret-here".Sha256()),
                    },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "api",
                    }
                },
                new Client
                {
                    ClientId = "client.svc",
                    ClientSecrets =
                    { 
                        new Secret("please-type-the-secret-here".Sha256()),
                    },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    { 
                        "inbox",
                    }
                },
                new Client{
                    ClientId = "client.JS",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    PostLogoutRedirectUris = { "http://localhost:5001", },
                    RedirectUris = {
                        "http://localhost:5001/",
                        "http://localhost:5001/assets/static/silent-renew.html",
                    },
                    AllowedCorsOrigins = { "http://localhost:5001", },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api",
                    },
                },
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>(new[]
            {
                new TestUser
                {
                    Username = "alice",
                    Password = "password",

                    Claims = new[]
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "alice@example.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address,
                            @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                            IdentityServerConstants.ClaimValueTypes.Json),
                    },
                },
                new TestUser
                {
                    Username = "bob",
                    Password = "password",

                    Claims = new[]
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address,
                            @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                            IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere"),
                    },
                },
            });
        }
    }
}