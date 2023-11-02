using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public InputModel Input { get; set; } = new();
        public string ReturnUrl { get; set; } = string.Empty;

        public RegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");

            if(ModelState.IsValid)
            {
                var identity = new IdentityUser 
                {
                    UserName = Input.Email,
                    Email = Input.Email
                };

                var result = await _userManager.CreateAsync(identity, Input.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(identity, isPersistent: false);

                    return LocalRedirect(ReturnUrl);
                }
            }

            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }
    }
}