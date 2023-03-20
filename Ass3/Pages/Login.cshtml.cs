using Ass3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRAssignment.Common;

namespace Ass3.Pages
{
    public class LoginModel : PageModel
    {
        private readonly PostDbContext _context;

        [BindProperty]
        public AppUser AppUser { get; set; } = default!;

        public LoginModel(PostDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (
                _context.AppUsers == null
                || AppUser == null
                || string.IsNullOrEmpty(AppUser.Email)
                || string.IsNullOrEmpty(AppUser.Password)
            )
            {
                return Page();
            }

            var user = _context.AppUsers.FirstOrDefault(u => u.Email == AppUser.Email);
            if (user == null)
            {
                ModelState.AddModelError("AppUser.Email", "Email not found");
                return Page();
            }

            if (user.Password != AppUser.Password)
            {
                ModelState.AddModelError("AppUser.Password", "Password is incorrect");
                return Page();
            }

            // login success
            VaSession.Set(HttpContext.Session, "Account", user);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToPage("./Post/Index"); 
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }
    }
}
