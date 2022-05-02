using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Users;

public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public IndexModel(UserManager<IdentityUser> userManager, IMapper mapper, ApplicationDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public List<UserViewModel> Users { get; private set; } = null!;

    [TempData]
    public string? Message { get; set; }

    public async Task OnGet()
    {
        var users = _mapper.Map<List<UserViewModel>>(await _userManager.GetUsersInRoleAsync(nameof(ApplicationRoles.Cashier)));



        foreach (var user in users)
        {
            var dbUser = await _userManager.FindByEmailAsync(user.Email);
            var roles = await _userManager.GetRolesAsync(dbUser);
            _ = roles.Contains(nameof(ApplicationRoles.Admin)) ? user.IsAdmin = true : user.IsAdmin = false;
            _ = roles.Contains(nameof(ApplicationRoles.Cashier)) ? user.IsCashier = true : user.IsCashier = false;
        }

        Users = users;
    }


    public async Task<RedirectToPageResult> OnGetDelete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.DeleteAsync(user);


        return RedirectToPage();
    }


    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public bool IsCashier { get; set; }
    }
}