namespace WebApp.Pages;

public class PageNotFoundModel : PageModel
{
    [TempData]
    public string? ErrorMessage { get; set; }
    public void OnGet()
    {
    }
}