namespace Tragate.Common.Library.Dto
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SmallImagePath { get; set; }
        public string BigImagePath { get; set; }
        public int StatusId { get; set; }
    }
}