namespace WebApp.Pages.Customers;

public class IndexModel : PageModel
{

    public IndexModel()
    {

    }


    [TempData]
    public string Message { get; set; }


    public int CurrentPage { get; set; }
    public string CurrentCol { get; set; }
    public string CurrentOrder { get; set; }


    //public Azure.Pageable<SearchResult<SearchCustomer>> Customers { get; private set; }

    public void OnGet(int pageno, string search, string? order, string col)
    {
        //CurrentPage = pageno;
        //CurrentCol = GetCol(col);
        //CurrentOrder = GetOrder(order);


        //var options = new SearchOptions
        //{
        //    IncludeTotalCount = true,
        //    OrderBy = { $"{CurrentCol} {CurrentOrder}" },
        //    Size = 50,
        //    Skip = CurrentPage * 50
        //};

        //var response = _azureSearchService.SearchClient.Search<SearchCustomer>(search, options);


        //Customers = response.Value.GetResults();

    }



    private static string GetOrder(string? order)
    {
        if (order is null) return "asc";


        return order.ToLower() switch
        {
            "desc" => "desc",
            _ => "asc"
        };
    }
    private static string GetCol(string currentCol)
    {
        return currentCol switch
        {
            "firstname" => "Givenname",
            "lastname" => "Surname",
            "address" => "Streetaddress",
            "city" => "City",
            "country" => "Country",
            null => "Id",
            _ => "Id"
        };
    }
}