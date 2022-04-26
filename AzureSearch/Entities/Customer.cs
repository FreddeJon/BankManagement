using Azure.Search.Documents.Indexes;

namespace AzureSearch.Entities;
public class Customer
{
    [SimpleField(IsKey = true, IsSortable = true)]
    public string Id { get; set; }

    [SearchableField(IsSortable = true)]
    public string Givenname { get; set; }

    [SearchableField(IsSortable = true)]
    public string Surname { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string Country { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string City { get; set; }

    [SearchableField(IsSortable = true)]
    public string Streetaddress { get; set; }

}