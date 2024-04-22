namespace API.Interfaces
{
    public interface IFacebookMessageService
    {
        public string ServiceApiKey { get;}
        public string SenderPageId { get;}
        public string RecipientId { get;}
    }
}