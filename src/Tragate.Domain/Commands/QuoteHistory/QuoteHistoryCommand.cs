namespace Tragate.Domain.Commands
{
    public abstract class QuoteHistoryCommand : Command
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
    }
}