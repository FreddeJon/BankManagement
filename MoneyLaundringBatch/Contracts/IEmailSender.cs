namespace MoneyLaunderingBatch.Contracts;
public interface IEmailSender
{
    void SendEmail(string receiver, string subject, string body);
}
