namespace CosmosDb.Utils.Interop
{
    public interface IRequestChargeAccumulator
    {
        void AccumulateRequestCharges(string name, double requestCharge);
    }
}
