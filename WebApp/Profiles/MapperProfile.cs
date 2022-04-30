using Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;
using WebApp.Pages.Customers;
using WebApp.Pages.Customers.Accounts.Transaction;
using IndexModel = WebApp.Pages.Customers.Accounts.IndexModel;

namespace WebApp.Profiles;

// ReSharper disable once UnusedType.Global
public class MapperProfile : Profile
{
    public MapperProfile()
    {


        // Pages/Index
        CreateMap<CustomerDto, Pages.IndexModel.CustomerViewModel>()
            .ForMember(x => x.Balance, opt => opt.MapFrom(x => x.Balance.ToString("C")));



        // Pages/Shared/Components/CustomerTable
        CreateMap<SearchCustomerDto, CustomerTable.CustomerViewModel>()
            .ReverseMap();


        //  Pages/Customers/Details
        CreateMap<AccountDto, DetailsModel.AccountViewModel>();
        CreateMap<CustomerDto, DetailsModel.CustomerViewModel>();


        // Pages/Customers/Create
        CreateMap<CreateModel.CreateCustomerModel, CustomerDto>()
            .ForMember(x => x.TelephoneCountryCode, opt => opt.MapFrom(y => GetTelephoneCode(y.CountryCode)))
            .ForMember(x => x.Country, opt => opt.MapFrom(x => GetCountry(x.CountryCode)));

        // Pages/Customers/Edit
        CreateMap<EditModel.EditCustomerModel, CustomerDto>()
            .ForMember(x => x.TelephoneCountryCode, opt => opt.MapFrom(y => GetTelephoneCode(y.CountryCode)))
            .ForMember(x => x.Country, opt => opt.MapFrom(x => GetCountry(x.CountryCode))).ReverseMap();

        // Pages/Customers/Accounts/Index
        CreateMap<CustomerDto, IndexModel.CustomerViewModel>();
        CreateMap<AccountDto, IndexModel.AccountViewModel>();
        CreateMap<TransactionDto, IndexModel.TransactionViewModel>()
            .ForMember(x => x.Amount, opt => opt.MapFrom(x => x.Amount.ToString("C")))
            .ForMember(x => x.NewBalance, opt => opt.MapFrom(x => x.NewBalance.ToString("C")))
            .ForMember(x => x.Date, opt => opt.MapFrom(x => x.Date.ToString("yyyy-MM-dd HH:mm:ss")));

        // Pages/Customers/Accounts/Transactions/Deposit
        CreateMap<AccountDto, DepositModel.AccountViewModel>();

        // Pages/Customers/Accounts/Transactions/Withdraw
        CreateMap<AccountDto, WithdrawModel.AccountViewModel>().ReverseMap();

        // Pages/Customers/Accounts/Transactions/Transfer
        CreateMap<AccountDto, TransferModel.AccountViewModel>().ReverseMap();



        // Pages/Users/Index
        CreateMap<IdentityUser, Pages.Users.IndexModel.UserViewModel>().ReverseMap();





    }

    private static string GetCountry(string countryCode)
    {
        return countryCode switch
        {
            "SE" => "Sverige",
            "NO" => "Norge",
            "FI" => "Finland",
            _ => throw new Exception()
        };
    }
    private static int GetTelephoneCode(string countryCode)
    {
        return countryCode switch
        {
            "SE" => 46,
            "NO" => 47,
            "FI" => 48,
            _ => throw new Exception()
        };
    }
}