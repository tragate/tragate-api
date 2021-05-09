namespace Tragate.Common.Library.Dto
{
    public class CompanySearchDto
    {
        public int Id { get; set; }
        public string EstablishmentYear { get; set; }
        public string ResponseRate { get; set; }
        public string ResponseTime { get; set; }
        public int? TransactionAmount { get; set; }
        public int? TransactionCount { get; set; }
        public int? MembershipTypeId { get; set; }
        public int? VerificationTypeId { get; set; }
        public byte StatusId { get; set; }
        public UserSearchDto User { get; set; }
        public string MembershipType { get; set; }
        public string VerificationType { get; set; }
        public string Slug { get; set; }
        public string[] CategoryTags { get; set; }
    }
}