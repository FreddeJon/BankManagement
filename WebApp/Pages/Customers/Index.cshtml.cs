namespace WebApp.Pages.Customers;

public class IndexModel : PageModel
{

    [TempData]
    public string Message { get; set; }

    public void OnGet()
    {

    }

}