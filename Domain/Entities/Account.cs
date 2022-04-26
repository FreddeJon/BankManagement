using System.ComponentModel.DataAnnotations;

// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618

namespace Domain.Entities;

public class Account : EntityBase
{
    public int Id { get; set; }


    [MaxLength(10)]
    public string AccountType { get; set; }

    public DateTime Created { get; set; }
    public decimal Balance { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}