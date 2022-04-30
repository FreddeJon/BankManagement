namespace WebApp.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(UserManager<IdentityUser> userManager)
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
        [MaxLength(30, ErrorMessage = "{0} max length {1}")]
        public string Username { get; set; }


        [BindProperty]
        public bool IsAdmin { get; set; }


        public async Task<IActionResult> OnGet(string id)
        {
            await LoadUser(id);
            return Page();
        }

        private async Task<IActionResult> LoadUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToPage("/PageNotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            _ = roles.Contains(nameof(ApplicationRoles.Admin)) ? IsAdmin = true : IsAdmin = false;

            Email = user.Email;
            Username = user.UserName;

            return Page();
        }


        public async Task<IActionResult> OnPost(string id)
        {

            var currentUser = await _userManager.FindByIdAsync(id);
            var userEmail = await _userManager.FindByEmailAsync(Email);
            var username = await _userManager.FindByNameAsync(Username);

            if (userEmail != null && Email != currentUser.Email)
            {
                ModelState.AddModelError(Email, "Email already in use");
            }

            if (username != null && Username != currentUser.UserName)
            {
                ModelState.AddModelError(Username, "Username already in use");
            }

            if (!ModelState.IsValid) return Page();


            currentUser.Email = Email;
            currentUser.UserName = Username;

            var result = await _userManager.UpdateAsync(currentUser);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("User", "User could not be created");
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);

            if (roles.Contains(nameof(ApplicationRoles.Admin)))
            {
                if (!IsAdmin)
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, nameof(ApplicationRoles.Admin));
                }
            }
            else
            {
                if (IsAdmin)
                {
                    await _userManager.AddToRoleAsync(currentUser, nameof(ApplicationRoles.Admin));
                }
            }




            TempData["Message"] = "User edited";
            return RedirectToPage("/Users/Index");
        }




        public async Task<JsonResult> OnPostCheckEmail(string id)
        {

            var currentUser = await _userManager.FindByIdAsync(id);

            var user = await _userManager.FindByEmailAsync(Email);

            var valid = user is null || Email == currentUser.Email;

            return new JsonResult(valid);
        }

        public async Task<JsonResult> OnPostCheckUsername(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);

            var user = await _userManager.FindByNameAsync(Username);
            var valid = user is null || Username == currentUser.UserName;
            return new JsonResult(valid);
        }
    }
}
