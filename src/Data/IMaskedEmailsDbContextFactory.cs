namespace Data
{
    public interface IMaskedEmailsDbContextFactory
    {
        MaskedEmailsDbContext CreateDbContext();
    }
}