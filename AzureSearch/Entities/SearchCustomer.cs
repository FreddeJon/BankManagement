#pragma warning disable CS8618
namespace AzureSearch.Entities;
public class SearchCustomer
{
    [SimpleField(IsKey = true, IsSortable = false)]
    public string Id { get; init; }

    [SearchableField(IsSortable = true)]
    public string Givenname { get; init; }

    [SearchableField(IsSortable = true)]
    public string Surname { get; init; }

    [SearchableField(IsSortable = true)]
    public string Country { get; init; }

    [SearchableField(IsSortable = true)]
    public string City { get; init; }

    [SearchableField(IsSortable = true)]
    public string Streetaddress { get; init; }

    [SearchableField(IsSortable = false)]
    public string NationalId { get; init; }

}