using System.Collections.Generic;
using Domain.Entities;

namespace UnitTest.Data;
public static class DummyData
{
    public static List<Transaction> GetDummyTransactions()
    {
        var transactions = new List<Transaction>
        {
            new()
            {
                Id = 1,
                Amount = 100,
                Date = DateTime.Now,
                NewBalance = 200,
                Operation = "Debit",
                Type = "Transfer"
            },
            new()
            {
                Id = 2,
                Amount = 1000,
                Date = DateTime.Now,
                NewBalance = 1100,
                Operation = "Debit",
                Type = "Transfer"
            }
        };

        return transactions;
    }

    public static IEnumerable<Customer> GetDummyCustomers()
    {
        var customers = new List<Customer>
        {
            new()
            {
                Id = 1,
                Birthday = DateTime.Parse("07/05/1993"),
                City = "Stockholm",
                Country = "Sweden",
                CountryCode = "SE",
                Streetaddress = "Tennisvägen 2F",
                Zipcode = "17553",
                EmailAddress = "fredrikjonson@hotmail.se",
                Givenname = "Fredrik",
                Surname = "Jonson",
                Telephone = "0765508838",
                TelephoneCountryCode = 46,
                NationalId = "1",
                Accounts = new List<Account>
                {
                    new()
                    {
                        Id = 1,
                        Balance = 100,
                        AccountType = "Saving",
                        Created = DateTime.Now
                    },
                    new()
                    {
                        Id = 2,
                        Balance = 1000,
                        AccountType = "Debit",
                        Created = DateTime.Now
                    },
                    new()
                    {
                        Id = 3,
                        Balance = 10000,
                        AccountType = "Salary",
                        Created = DateTime.Now
                    }
                }
            },
            new()
            {
                Id = 2,
                Birthday = DateTime.Parse("07/05/1993"),
                City = "Stockholm",
                Country = "Sweden",
                CountryCode = "SE",
                Streetaddress = "Tennisvägen 2F",
                Zipcode = "17553",
                EmailAddress = "fredrikjonson@hotmail.se",
                Givenname = "Fredrik",
                Surname = "Jonson",
                Telephone = "0765508838",
                TelephoneCountryCode = 46,
                NationalId = "1",
                Accounts = new List<Account>
                {
                    new()
                    {
                        Id = 4,
                        Balance = 100,
                        AccountType = "Saving",
                        Created = DateTime.Now
                    },
                    new()
                    {
                        Id = 5,
                        Balance = 10000000000000,
                        AccountType = "Debit",
                        Created = DateTime.Now
                    }
                }
            },
            new()
            {
                Id = 3,
                Birthday = DateTime.Parse("07/05/1993"),
                City = "Stockholm",
                Country = "Sweden",
                CountryCode = "SE",
                Streetaddress = "Tennisvägen 2F",
                Zipcode = "17553",
                EmailAddress = "fredrikjonson@hotmail.se",
                Givenname = "Herman",
                Surname = "Hmm",
                Telephone = "0765508838",
                TelephoneCountryCode = 46,
                NationalId = "1",
                Accounts = new List<Account>()
            }
        };

        return customers;
    }


    public static List<Account> GetDummyAccounts()
    {
        var accounts = new List<Account>
        {
            new()
            {
                Id = 1,
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>
                {
                    new()
                    {
                        Id = 1,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 2,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    }
                }
            },
            new()
            {
                Id = 2,
                AccountType = "Debit",
                Created = DateTime.Now,
                Balance = 1000,
                Transactions = new List<Transaction>
                {
                    new()
                    {
                        Id = 3,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 4,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 5,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 6,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    }
                }
            },
            new()
            {
                Id = 3,
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>()
            },
            new()
            {
                Id = 4,
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>
                {
                    new()
                    {
                        Id = 7,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 8,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 9,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 10,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 11,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 12,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 13,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 14,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 15,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 16,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 17,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 18,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 19,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 20,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 21,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 22,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 23,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 24,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 25,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 26,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 27,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 28,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 29,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 30,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },

                    new()
                    {
                        Id = 31,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 32,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 33,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 34,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 35,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 36,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 37,
                        Amount = 100,
                        Date = DateTime.Now,
                        NewBalance = 200,
                        Operation = "Debit",
                        Type = "Transfer"
                    },
                    new()
                    {
                        Id = 38,
                        Amount = 1000,
                        Date = DateTime.Now,
                        NewBalance = 1100,
                        Operation = "Debit",
                        Type = "Transfer"
                    }
                }
            }
        };
        return accounts;
    }
}
