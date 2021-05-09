namespace Tragate.Common.Library.Dto
{
    public class QuoteUserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
    }
}