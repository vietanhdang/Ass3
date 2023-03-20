using Ass3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ass3.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly PostDbContext _context;

        public RegisterModel(PostDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AppUser AppUser { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.AppUsers == null || AppUser == null)
            {
                return Page();
            }

            _context.AppUsers.Add(AppUser);
            await _context.SaveChangesAsync();

            // redirect to home page not user page
            return RedirectToPage("/Index");
        }
    }
}
