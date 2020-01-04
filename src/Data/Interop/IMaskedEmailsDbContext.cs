using System;
using System.Threading.Tasks;
using Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Interop
{
    public interface IBaseDbContext : IDisposable
    {
        Task SaveChangesAsync();
        void SaveChanges();
    }

    public interface IMaskedEmailsDbContext : IBaseDbContext
    {
        DbSet<Profile> Users { get; }
        DbSet<Address> Addresses { get; }
    }
}