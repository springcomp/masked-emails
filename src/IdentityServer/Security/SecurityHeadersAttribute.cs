// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Quickstart.UI;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpringComp.IdentityServer.TableStorage.Stores;
using System.Linq;
using System.Reflection;

namespace IdentityServer.Security
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            string cors = GetCorsOrigins(context);

            var result = context.Result;
            if (result is ViewResult)
            {
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
                var csp = "default-src 'self';" +
                              "object-src 'none';" +
                              "frame-ancestors 'self' " + cors + ";" +
                              "sandbox allow-forms allow-same-origin allow-scripts;" +
                              "base-uri 'self';" +
                              "style-src 'self' https://fonts.googleapis.com;" +
                              "font-src 'self' https://fonts.gstatic.com;";
                // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
                //csp += "upgrade-insecure-requests;";
                // also an example if you need client images to be displayed from twitter
                // csp += "img-src 'self' https://pbs.twimg.com;";

                // once for standards compliant browsers
                if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
                }
                // and once again for IE
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
                var referrer_policy = "no-referrer";
                if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
                }
            }
        }

        private static string GetCorsOrigins(ResultExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<SecurityHeadersAttribute>>();

            logger.LogDebug("Retrieving CorsOrigins from in-memory cache...");

            var cache = CorsOriginsCache.Instance;
            var cors = cache.Origins;
            if (cors == null)
            {
                logger.LogDebug("Cache entry missing. Retrieving clients from the data store…");

                var store = serviceProvider.GetRequiredService<IClientStore>();
                var clientStore = store as CachingClientStore<ClientStore>;
                var innerFieldInfo = clientStore.GetType().GetField("_inner", BindingFlags.NonPublic | BindingFlags.Instance);
                var inner = innerFieldInfo.GetValue(clientStore) as ClientStore;
                System.Diagnostics.Debug.Assert(inner != null);

                if (inner == null)
                    logger.LogCritical("Please, make sure to use a TableStorage.ClientStore.");

                var clients = inner.GetAllClientsAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult()
                    ;

                logger.LogDebug("Clients successfully retrieved from data store.");

                var origins = clients
                    .SelectMany(c => c.AllowedCorsOrigins)
                    .ToArray()
                    ;

                cors = string.Join(" ", origins);

                logger.LogDebug($"Adding CorsOrigins with value '{cors}' to in-memory cache.");

                cache.AddToCache(cors);
            }

            return cors;
        }
    }
}
