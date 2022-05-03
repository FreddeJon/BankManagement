using Domain.Entities;
using MoneyLaunderingBatch.Contracts;
using Persistence;

namespace MoneyLaunderingBatch.Services;
public class LaunderingChecker : ILaunderingChecker
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public LaunderingChecker(ApplicationDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task RunLaunderingCheck()
    {
        var checkCountries = new[] { "Sverige", "Norge", "Finland" };



        foreach (var country in checkCountries)
        {
            var receiverEmail = $"{country.ToLower()}@bankmanagement.com";

            var subject = $"Money laundering report for {country}";


            var body = await CheckCountry(country);
            _emailSender.SendEmail(receiver: receiverEmail, subject: subject, body: body);
        }
    }

    public class LaunderingCheck
    {
        public string Name { get; set; }
        public List<AccountDetails> Accounts { get; set; }
        public class AccountDetails
        {
            public int AccountId { get; set; }
            public List<Transaction> Transactions { get; set; }
        }

    }
    private async Task<string> CheckCountry(string country)
    {

        var query = _context.Customers.Where(x => x.Country == country).Include(x => x.Accounts)
            .ThenInclude(x => x.Transactions)
            .AsQueryable();

        var customersToCheck = await query.Select(x => new LaunderingCheck
        {
            Name = x.Givenname + " " + x.Surname,
            Accounts = x.Accounts.Select(a => new LaunderingCheck.AccountDetails
            {
                AccountId = a.Id,
                Transactions = a.Transactions.Where(t => t.Date >= DateTime.Now.AddDays(-4)).ToList()
            }).ToList()
        }).ToListAsync();

        var foundSuspiciousTransactions = false;

        var report = $"Report for {country}";

        foreach (var customer in customersToCheck)
        {
            foreach (var account in customer.Accounts)
            {
                var transactionsLatest72Hours = account.Transactions.Where(x => x.Date >= DateTime.Now.AddHours(-72)).ToList();

                var transactionsLatest24Hours = account.Transactions.Where(x => x.Date >= DateTime.Now.AddHours(-24))
                    .Where(x => x.Amount >= 15000).ToList();

                if (transactionsLatest72Hours.Sum(x => x.Amount) >= 23000)
                {
                    foundSuspiciousTransactions = true;
                    transactionsLatest72Hours.ForEach(x =>
                        report = report.AddTransactionDetails(name: customer.Name, accountId: account.AccountId, transactionId: x.Id, amount: x.Amount.ToString("C"), date: x.Date.ToString("s")));
                }
                else if (transactionsLatest24Hours.Count > 0)
                {
                    foundSuspiciousTransactions = true;
                    transactionsLatest24Hours.ForEach(x =>
                        report = report.AddTransactionDetails(name: customer.Name, accountId: account.AccountId, transactionId: x.Id, amount: x.Amount.ToString("C"), date: x.Date.ToString("s")));
                }
            }
        }

        return foundSuspiciousTransactions ? report : $"Checked for {country} found nothing";
    }
}
public static class ReportFormatHelper
{
    public static string AddLineBreak(this string report)
    {

        return report += "\n";
    }


    public static string AddTransactionDetails(this string report, string name, int accountId, int transactionId, string amount, string date)
    {
        report = report.AddLineBreak();
        report += $"Name: {name}, AccountID: {accountId}, TransactionID: {transactionId}, Amount: {amount}, Date: {date}";

        return report;
    }
}

