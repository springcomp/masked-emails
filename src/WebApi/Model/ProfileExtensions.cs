using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace WebApi.Model
{
    public static class ProfileExtensions
    {
        public static User ToModel(this Data.Model.Profile profile)
        {
            return new User
            {
                DisplayName = profile.DisplayName,
                ForwardingAddress = profile.ForwardingAddress,
                CreatedUtc = profile.CreatedUtc,
            };
        }
        public static User ToModel(this CosmosDb.Model.Profile profile)
        { 
            return new User
            {
                DisplayName = profile.DisplayName,
                ForwardingAddress = profile.ForwardingAddress,
                CreatedUtc = profile.CreatedUtc,
            };
        }
    }
}
