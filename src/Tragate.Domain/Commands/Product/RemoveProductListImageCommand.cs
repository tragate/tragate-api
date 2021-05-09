namespace Tragate.Domain.Commands
{
    public class RemoveProductListImageCommand : ProductCommand
    {
        public string SmallImagePath { get; set; }

        public override bool IsValid(){
            return true;
        }
    }
}