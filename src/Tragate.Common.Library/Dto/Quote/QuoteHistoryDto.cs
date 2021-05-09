using System;

namespace Tragate.Common.Library
{
    public class QuoteHistoryDto
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedUser { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}