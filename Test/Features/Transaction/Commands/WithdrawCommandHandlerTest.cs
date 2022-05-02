using Application.Features.Transactions.Commands.Withdraw;

namespace UnitTest.Features.Transaction.Commands;
public class WithdrawCommandHandlerTest
{
    private readonly ApplicationDbContext _context;
    private readonly WithdrawCommandHandler _sut;


    public WithdrawCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options, new FakeUserService());
        _sut = new WithdrawCommandHandler(_context);


        _context.AddRange(DummyData.GetDummyCustomers());
    }



    [Fact]
    public async Task Valid_amount_should_be_withdrawn()
    {
        const int accountId = 1;
        const decimal amount = 100;
        const string operation = "ATM withdrawal";

        var response = await _sut.Handle(new WithdrawCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);


        account!.Balance.ShouldBe(0);
        account.Transactions.Count.ShouldBe(1);
        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<WithdrawResponse>();
    }


    [Fact]
    public async Task Valid_amount50_should_be_withdrawn()
    {
        const int accountId = 1;
        const decimal amount = 50;
        const string operation = "Payment";



        var response = await _sut.Handle(new WithdrawCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);


        account!.Balance.ShouldBe(50);
        account.Transactions.Count.ShouldBe(1);
        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<WithdrawResponse>();
    }




    [Fact]
    public async Task Invalid_negative_amount_should_not_be_withdrawn()
    {
        const int accountId = 1;
        const decimal amount = -50;
        const string operation = "Payment";



        var response = await _sut.Handle(new WithdrawCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);


        account!.Balance.ShouldBe(100);
        account.Transactions.Count.ShouldBe(0);
        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<WithdrawResponse>();
    }

    [Fact]
    public async Task Invalid_balance_to_low_should_not_be_withdrawn()
    {
        const int accountId = 1;
        const decimal amount = 500;
        const string operation = "Payment";



        var response = await _sut.Handle(new WithdrawCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);


        account!.Balance.ShouldBe(100);
        account.Transactions.Count.ShouldBe(0);
        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<WithdrawResponse>();
    }


    [Fact]
    public async Task Invalid_accountId_should_not_be_withdrawn()
    {
        const int accountId = 100;
        const decimal amount = 50;
        const string operation = "Payment";



        var response = await _sut.Handle(new WithdrawCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);


        account.ShouldBeNull();
        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<WithdrawResponse>();
    }

}
