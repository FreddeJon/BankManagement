namespace MoneyLaunderingBatch.Configurations;

public class MoneyLaunderingEmailOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Sender { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}