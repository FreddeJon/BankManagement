using Application.Features.Transactions.Commands.Deposit;

namespace UnitTest.Features.Transaction.Commands;
public class DepositCommandHandlerTest
{
    private readonly ApplicationDbContext _context;
    private readonly DepositCommandHandler _sut;

    public DepositCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options, new FakeUserService());
        _sut = new DepositCommandHandler(_context);


        _context.AddRange(DummyData.GetDummyCustomers());
    }

    [Fact]
    public async Task Valid_amount_deposit_should_be_deposited()
    {

        const int accountId = 1;
        const decimal amount = 100;
        const string operation = "Deposit cash";



        var response = await _sut.Handle(new DepositCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());

        var account = await _context.Accounts.FindAsync(accountId);


        account!.Balance.ShouldBe(200);
        account.Transactions.Count.ShouldBe(1);
        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<DepositResponse>();

    }

    [Fact]
    public async Task Valid_amount_valid_operation_should_be_deposited()
    {

        const int accountId = 1;
        const decimal amount = 100;
        const string operation = "Salary";



        var response = await _sut.Handle(new DepositCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);


        account!.Balance.ShouldBe(200);
        account.Transactions.Count.ShouldBe(1);
        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<DepositResponse>();

    }

    [Fact]
    public async Task Valid_amount_10m_should_be_deposited()
    {

        const int accountId = 1;
        const decimal amount = 10000000;
        const string operation = "Deposit cash";


        var response = await _sut.Handle(new DepositCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());

        var account = await _context.Accounts.FindAsync(accountId);



        account!.Balance.ShouldBe(10000100);
        account.Transactions.Count.ShouldBe(1);
        response.Status.ShouldBe(StatusCode.Success);
        response.Errors.ShouldBeNull();
        response.ShouldBeOfType<DepositResponse>();
    }

    [Fact]
    public async Task Invalid_accountId_should_not_be_deposited()
    {

        const int accountId = 10;
        const decimal amount = 100;
        const string operation = "Deposit cash";



        var response = await _sut.Handle(new DepositCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);



        account.ShouldBeNull();
        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<DepositResponse>();
    }

    [Fact]
    public async Task Invalid_negative_amount_should_not_be_deposited()
    {

        const int accountId = 1;
        const decimal amount = -100;
        const string operation = "Deposit cash";



        var response = await _sut.Handle(new DepositCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);



        account!.Balance.ShouldBe(100);
        account.Transactions.Count.ShouldBe(0);
        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<DepositResponse>();
    }

    [Fact]
    public async Task Invalid_amount_to_high_should_not_be_deposited()
    {

        const int accountId = 1;
        const decimal amount = 100000000;
        const string operation = "Deposit cash";



        var response = await _sut.Handle(new DepositCommand() { AccountId = accountId, Amount = amount, Operation = operation }, new CancellationToken());


        var account = await _context.Accounts.FindAsync(accountId);



        account!.Balance.ShouldBe(100);
        account.Transactions.Count.ShouldBe(0);
        response.Status.ShouldBe(StatusCode.Error);
        response.Errors!.Count.ShouldBeGreaterThan(0);
        response.ShouldBeOfType<DepositResponse>();
    }
}
