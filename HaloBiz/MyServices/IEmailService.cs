namespace HaloBiz.MyServices
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }
}