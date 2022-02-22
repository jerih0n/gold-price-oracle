namespace GoldPriceOracle.Services.Models.Information
{
    public record StakeholderInformationModel(string TotalAmount, string OwnedAmount, string NominatedAmount, string NominatorsCount, bool CanValidate);  
}
