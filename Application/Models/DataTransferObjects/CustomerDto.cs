namespace Application.Models.DataTransferObjects;
public class CustomerDto
{
    public int Id { get; set; }
    public string NationalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Givenname { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Streetaddress { get; set; } = null!;
    public string Telephone { get; set; } = null!;
    public int TelephoneCountryCode { get; set; }
    public string Zipcode { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public DateTime Birthday { get; set; }
    public decimal Balance { get; set; }
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<AccountDto> Accounts { get; set; } = new();
}
