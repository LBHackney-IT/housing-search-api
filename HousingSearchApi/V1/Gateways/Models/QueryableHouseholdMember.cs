namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryableHouseholdMember
    {
        public string FullName { get; set; }
        public bool IsResponsible { get; set; }
        public string DateOfBirth { get; set; }
        public string PersonTenureType { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
    }
}
