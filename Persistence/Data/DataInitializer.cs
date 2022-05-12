using System.Diagnostics;
using Bogus;
using Persistence.Options;

namespace Persistence.Data;

public static class DataInitializer
{
    private static readonly Random Random = new();

    public static async Task InitializeDataAsync(this IServiceProvider service)
    {
        await using var scope = service.CreateAsyncScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        var databaseOptions = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
        var accountOptions = scope.ServiceProvider.GetRequiredService<IOptions<AccountOptions>>().Value;

        if (context is null) return;
        if (userManager is null) return;
        if (roleManager is null) return;


        await context.Database.MigrateAsync();




        if (accountOptions.SeedAccounts)
        {
            await SeedRoleAsync(roleManager, nameof(ApplicationRoles.Admin));
            await SeedRoleAsync(roleManager, nameof(ApplicationRoles.Cashier));
            await SeedRoleAsync(roleManager, nameof(ApplicationRoles.Customer));

            var adminOptions = accountOptions.AdminOptions;
            if (adminOptions?.Email is null || adminOptions.Password is null)
                throw new Exception("Somethings wrong in admin options");

            await userManager.SeedUserAsync(adminOptions.Email, adminOptions.Password, new[] { nameof(ApplicationRoles.Admin), nameof(ApplicationRoles.Cashier) });


            var userOptions = accountOptions.UserOptions;
            if (userOptions?.Email is null || userOptions.Password is null)
                throw new Exception("Somethings wrong in user options");

            await userManager.SeedUserAsync(userOptions.Email, userOptions.Password, new[] { nameof(ApplicationRoles.Cashier) });


            if (databaseOptions.SeedDatabase)
            {
                await context.SeedDataAsync(databaseOptions, userManager);
            }
        }
    }

    public static async Task<IdentityUser?> SeedUserAsync(this UserManager<IdentityUser> userManager, string? email, string password, IEnumerable<string> roles)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new IdentityUser()
            {
                Email = email,
                UserName = email,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRolesAsync(user, roles);

            return user;
        }

        return null;
    }

    private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }


    private static async Task SeedDataAsync(this ApplicationDbContext context, DatabaseOptions databaseOptions,
        UserManager<IdentityUser> userManager)
    {
        var customers = new List<Customer>();
        while (await context.Customers.CountAsync() < databaseOptions.CustomersToSeed)
        {
            var num = databaseOptions.CustomersToSeed - await context.Customers.CountAsync();
            if (num > 0)
            {
                for (var i = 0; i < num; i++)
                {
                    var customer = GenerateCustomer();
                    customers.Add(customer);
                    Debug.WriteLine($"Created {customer.Givenname} {customer.Surname} NO:{i}");
                }
            }

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        // Create login for customers
        if (databaseOptions.SeedCustomerUserAccount)
        {
            await context.GenerateAccountsForCustomers(userManager);
        }
    }



    private static async Task GenerateAccountsForCustomers(this ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {

        var getCustomers = await context.Customers.AsNoTracking().ToListAsync();

        foreach (var customer in getCustomers)
        {
            var user = await userManager.FindByEmailAsync(customer.EmailAddress);

            if (user is null)
            {
                var password = $"{customer.Givenname}{customer.Surname}1".ToLower();
                user = await userManager.SeedUserAsync(email: customer.EmailAddress.ToLower(), password, new[] { nameof(ApplicationRoles.Customer) });

                string id = user!.Id;
                var customerUser = new CustomerUser
                {
                    CustomerId = customer.Id,
                    UserId = id
                };
                context.CustomerUser.Add(customerUser);
                // ReSharper disable once MethodHasAsyncOverload
                context.SaveChanges();
                Debug.WriteLine($"Created login for: {user.Email} with password: {password}");
            }
        }

    }
    private static Customer GenerateCustomer()
    {
        // f.Date.Between(new DateTime(1999,1,1), new DateTime(1940,1,1))
        var n = Random.Next(0, 100);
        Customer person;
        switch (n)
        {
        case < 20:
        {
            var testUser = new Faker<Customer>("nb_NO")
                .StrictMode(false)
                .RuleFor(e => e.Id, f => 0)
                .RuleFor(e => e.City, (f, u) => f.Address.City())
                .RuleFor(e => e.Country, (f, u) => "Norge")
                .RuleFor(e => e.CountryCode, (f, u) => "NO")
                .RuleFor(e => e.Birthday, (f, u) => f.Person.DateOfBirth)
                .RuleFor(e => e.EmailAddress, (f, u) => f.Internet.Email())
                .RuleFor(e => e.Givenname, (f, u) => f.Person.FirstName)
                .RuleFor(e => e.Surname, (f, u) => f.Person.LastName)
                .RuleFor(e => e.NationalId, (f, u) => f.Person.DateOfBirth.ToString("yyyyMMdd") + "-3333")
                .RuleFor(e => e.Streetaddress, (f, _) => f.Address.StreetAddress())
                .RuleFor(e => e.Telephone, (f, u) => f.Person.Phone)
                .RuleFor(e => e.Zipcode, (f, u) => f.Address.ZipCode())
                .RuleFor(e => e.TelephoneCountryCode, (f, u) => 47);
            person = testUser.Generate(1).First();
            break;
        }
        case < 80:
        {
            var testUser = new Faker<Customer>("sv")
                .StrictMode(false)
                .RuleFor(e => e.Id, f => 0)
                .RuleFor(e => e.City, (f, u) => f.Address.City())
                .RuleFor(e => e.Country, (f, u) => "Sverige")
                .RuleFor(e => e.CountryCode, (f, u) => "SE")
                .RuleFor(e => e.Birthday, (f, u) => f.Person.DateOfBirth)
                .RuleFor(e => e.EmailAddress, (f, u) => f.Internet.Email())
                .RuleFor(e => e.Givenname, (f, u) => f.Person.FirstName)
                .RuleFor(e => e.Surname, (f, u) => f.Person.LastName)
                .RuleFor(e => e.NationalId, (f, u) => f.Person.DateOfBirth.ToString("yyyyMMdd") + "-1111")
                .RuleFor(e => e.Streetaddress, (f, u) => f.Address.StreetAddress())
                .RuleFor(e => e.Telephone, (f, u) => f.Person.Phone)
                .RuleFor(e => e.Zipcode, (f, u) => f.Address.ZipCode())
                .RuleFor(e => e.TelephoneCountryCode, (f, u) => 46);
            person = testUser.Generate(1).First();
            break;
        }
        default:
        {
            var testUser = new Faker<Customer>("fi")
                .StrictMode(false)
                .RuleFor(e => e.Id, f => 0)
                .RuleFor(e => e.City, (f, u) => f.Address.City())
                .RuleFor(e => e.Country, (f, u) => "Finland")
                .RuleFor(e => e.CountryCode, (f, u) => "FI")
                .RuleFor(e => e.Birthday, (f, u) => f.Person.DateOfBirth)
                .RuleFor(e => e.EmailAddress, (f, u) => f.Internet.Email())
                .RuleFor(e => e.Givenname, (f, u) => f.Person.FirstName)
                .RuleFor(e => e.Surname, (f, u) => f.Person.LastName)
                .RuleFor(e => e.NationalId, (f, u) => f.Person.DateOfBirth.ToString("yyyyMMdd") + "-2222")
                .RuleFor(e => e.Streetaddress, (f, u) => f.Address.StreetAddress())
                .RuleFor(e => e.Telephone, (f, u) => f.Person.Phone)
                .RuleFor(e => e.Zipcode, (f, u) => f.Address.ZipCode())
                .RuleFor(e => e.TelephoneCountryCode, (f, u) => 48);
            person = testUser.Generate(1).First();
            break;
        }
        }

        for (var i = 0; i < Random.Next(1, 5); i++)
        {
            person.Accounts.Add(GenerateAccount());
        }
        return person;
    }
    private static Account GenerateAccount()
    {
        string[] accountType = { "Personal", "Checking", "Savings" };
        var testUser = new Faker<Account>()
            .StrictMode(false)
            .RuleFor(e => e.Id, f => 0)
            .RuleFor(e => e.AccountType, (f, u) => f.PickRandom(accountType))
            .RuleFor(e => e.Balance, (f, u) => 0);

        var account = testUser.Generate(1).First();
        var start = DateTime.Now.AddDays(-Random.Next(5000, 10000));
        account.Created = start;
        account.Balance = 0;
        var transactions = Random.Next(25, 65);
        for (var i = 0; i < transactions; i++)
        {
            var tran = new Transaction
            {
                Amount = Random.NextInt64(1, 50) * 100
            };
            start = start.AddDays(Random.NextInt64(50, 600));
            if (start > DateTime.Now)
                break;
            tran.Date = start;
            account.Transactions.Add(tran);
            if (account.Balance - tran.Amount < 0)
                tran.Type = "Debit";
            else
            {
                tran.Type = Random.NextInt64(0, 100) > 70 ? "Debit" : "Credit";
            }

            var r = Random.Next(0, 100);
            if (tran.Type == "Debit")
            {
                account.Balance += tran.Amount;
                tran.Operation = r switch
                {
                    < 20 => "Deposit cash",
                    < 66 => "Salary",
                    _ => "Transfer"
                };
            }
            else
            {
                account.Balance -= tran.Amount;
                tran.Operation = r switch
                {
                    < 40 => "ATM withdrawal",
                    < 66 => "Payment",
                    _ => "Transfer"
                };
            }

            tran.NewBalance = account.Balance;
        }
        return account;
    }
}