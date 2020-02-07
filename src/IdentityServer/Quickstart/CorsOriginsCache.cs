// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.Caching.Memory;
using System;

namespace IdentityServer4.Quickstart.UI
{
    public sealed class CorsOriginsCache
    {
        private readonly MemoryCache cache_;
        private static readonly string CorsOrigins = "CorsOrigins";
        private CorsOriginsCache()
        {
            cache_ = new MemoryCache(new MemoryCacheOptions());
        }
        public static CorsOriginsCache Instance { get; } = new CorsOriginsCache();
        public string Origins
        {
            get
            {
                if (cache_.TryGetValue(CorsOrigins, out string origins))
                    return origins;
                return null;
            }
        }
        public void AddToCache(string origins)
        {
            cache_.Set(CorsOrigins, origins, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(60.0),
            });
        }
    }
}
