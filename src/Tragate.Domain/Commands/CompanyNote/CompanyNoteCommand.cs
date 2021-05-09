namespace Tragate.Domain.Commands
{
    public abstract class CompanyNoteCommand : Command
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
    }
}