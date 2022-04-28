using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Transactions.Commands.Transfer;

namespace UnitTest.Features.Transaction.Commands;
public class TransferCommandHandlerTest
{
    private readonly TransferCommandHandler _sut;
    private readonly ApplicationDbContext _context;

    public TransferCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _sut = new TransferCommandHandler(_context);


        _context.AddRange(DummyData.GetDummyCustomers());
    }



    [Fact]
    public async Task Valid_amount_should_transfer()
    {
        const int fromAccount = 1;
        const int toAccount = 2;
        const decimal amount = 100;




        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);

        accountFrom!.Transactions.Count.ShouldBe(1);
        accountFrom.Balance.ShouldBe(0);


        accountTo!.Transactions.Count.ShouldBe(1);
        accountTo.Balance.ShouldBe(1100);

        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<TransferResponse>();
    }



    [Fact]
    public async Task Valid_amount50_should_transfer()
    {
        const int fromAccount = 1;
        const int toAccount = 2;
        const decimal amount = 50;


        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);

        accountFrom!.Transactions.Count.ShouldBe(1);
        accountFrom.Balance.ShouldBe(50);


        accountTo!.Transactions.Count.ShouldBe(1);
        accountTo.Balance.ShouldBe(1050);

        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<TransferResponse>();
    }


    [Fact]
    public async Task Valid_amount_other_account_should_transfer()
    {
        const int fromAccount = 2;
        const int toAccount = 1;
        const decimal amount = 50;


        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);

        accountFrom!.Transactions.Count.ShouldBe(1);
        accountFrom.Balance.ShouldBe(950);


        accountTo!.Transactions.Count.ShouldBe(1);
        accountTo.Balance.ShouldBe(150);

        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<TransferResponse>();
    }



    [Fact]
    public async Task Invalid_negative_amount_should_not_transfer()
    {
        const int fromAccount = 1;
        const int toAccount = 2;
        const decimal amount = -50;


        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);

        accountFrom!.Transactions.Count.ShouldBe(0);
        accountFrom.Balance.ShouldBe(100);


        accountTo!.Transactions.Count.ShouldBe(0);
        accountTo.Balance.ShouldBe(1000);

        response.Status.ShouldBe(StatusCode.Error);
        response.Errors.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<TransferResponse>();
    }


    [Fact]
    public async Task Invalid_account_should_not_transfer()
    {
        const int fromAccount = 1;
        const int toAccount = 200;
        const decimal amount = -50;


        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);

        accountFrom!.Transactions.Count.ShouldBe(0);
        accountFrom.Balance.ShouldBe(100);

        accountTo.ShouldBeNull();

        //accountTo!.Transactions.Count.ShouldBe(0);
        //accountTo.Balance.ShouldBe(1000);

        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<TransferResponse>();
    }





    [Fact]
    public async Task Invalid_accountFrom_and_accountTo_should_not_transfer()
    {
        const int fromAccount = 100;
        const int toAccount = 200;
        const decimal amount = -50;


        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);

        accountFrom.ShouldBeNull();

        accountTo.ShouldBeNull();

        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<TransferResponse>();
    }


    [Fact]
    public async Task Invalid_valid_accounts_amount_to_high_should_not_transfer()
    {
        const int fromAccount = 5;
        const int toAccount = 2;
        const decimal amount = 100000000;


        var response = await _sut.Handle(new TransferCommand() { FromAccountId = fromAccount, ToAccountId = toAccount, Amount = amount }, new CancellationToken());


        var accountFrom = await _context.Accounts.FindAsync(fromAccount);
        var accountTo = await _context.Accounts.FindAsync(toAccount);


        accountFrom!.Transactions.Count.ShouldBe(0);
        accountFrom.Balance.ShouldBe(10000000000000);

        accountTo!.Transactions.Count.ShouldBe(0);
        accountTo.Balance.ShouldBe(1000);

        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<TransferResponse>();
    }
}
