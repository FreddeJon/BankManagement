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

    }
}
