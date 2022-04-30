using Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;
using Azure.Search.Documents.Models;
using AzureSearch.Entities;

namespace Application.Infrastructure.AutoMapper;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => $"{x.Givenname} {x.Surname}"))
            .ForMember(x => x.Balance, opt => opt.MapFrom(x => x.Accounts.Sum(s => s.Balance)))
            .ReverseMap();
        CreateMap<Account, AccountDto>()
            .ForMember(x => x.LatestTransaction, opt => opt.MapFrom((src, _) =>
            {
                if (src.Transactions.Count > 0)
                {
                    return src.Transactions.Max(transaction => transaction.Date);
                }
                else
                {
                    return (DateTime?)null;
                }
            }))
            .ReverseMap();
        CreateMap<Transaction, TransactionDto>()
            .ReverseMap();




        CreateMap<SearchResult<SearchCustomer>, SearchCustomerDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => MapIdFromString(x.Document.Id)))
            .ForMember(x => x.Firstname, opt => opt.MapFrom(x => x.Document.Givenname))
            .ForMember(x => x.Lastname, opt => opt.MapFrom(x => x.Document.Surname))
            .ForMember(x => x.Address, opt => opt.MapFrom(x => x.Document.Streetaddress))
            .ForMember(x => x.City, opt => opt.MapFrom(x => x.Document.City))
            .ForMember(x => x.Country, opt => opt.MapFrom(x => x.Document.Country))
            .ForMember(x => x.NationalId, opt => opt.MapFrom(x => x.Document.NationalId))
            .ReverseMap();

    }



    private static int MapIdFromString(string id)
    {
        _ = int.TryParse(id, out var result);


        return result;
    }
}
