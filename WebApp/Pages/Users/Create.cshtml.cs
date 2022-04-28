using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [PageRemote(
            ErrorMessage = "Email already in use",
            AdditionalFields = "__RequestVerificationToken",
            HttpMethod = "post",
            PageHandler = "CheckEmail"
        )]
        [Required(ErrorMessage = "{0} is required")]
        [BindProperty]
        public string Email { get; set; }




        [PageRemote(
            ErrorMessage = "Username already in use",
            AdditionalFields = "__RequestVerificationToken",
            HttpMethod = "post",
            PageHandler = "CheckUsername"
        )]
        [Required(ErrorMessage = "{0} is required")]
        [BindProperty]
        [MaxLength(20, ErrorMessage = "{0} max length {1}")]
        public string Username { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [BindProperty]
        [MaxLength(20, ErrorMessage = "{0} max length {1}")]
        public string Password { get; set; }

        [BindProperty]
        public bool IsAdmin { get; set; }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPost()
        {
            var userEmail = await _userManager.FindByEmailAsync(Email);
            var username = await _userManager.FindByNameAsync(Username);

            if (userEmail != null)
            {
                ModelState.AddModelError(Email, "Email already in use");
            }

            if (username != null)
            {
                ModelState.AddModelError(Username, "Username already in use");
            }

            if (!ModelState.IsValid) return Page();


            var user = new IdentityUser() { Email = Email, UserName = Username, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(user, Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("User", "User could not be created");
                return Page();
            }

            _ = IsAdmin
                  ? await _userManager.AddToRolesAsync(user, new[] { nameof(ApplicationRoles.Admin), nameof(ApplicationRoles.Cashier) })
                  : await _userManager.AddToRolesAsync(user, new[] { nameof(ApplicationRoles.Cashier) });

            TempData["Message"] = "User created";
            return RedirectToPage("/Users/Index");
        }




        public async Task<JsonResult> OnPostCheckEmail()
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var valid = user is null;
            return new JsonResult(valid);
        }

        public async Task<JsonResult> OnPostCheckUsername()
        {
            var user = await _userManager.FindByNameAsync(Username);
            var valid = user is null;
            return new JsonResult(valid);
        }
    }
}
