namespace Tragate.Domain.EventHandlers
{
    public interface IEMailHandler
    {
        void Execute(object data);
    }
}