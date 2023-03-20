using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ass3.Models;
using SignalRAssignment.Common;
using Ass3.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Ass3.Pages_Post
{
    [Authentication]
    public class CreateModel : PageModel
    {
        private readonly PostDbContext _context;
        private readonly IHubContext<SignalRServer> _signalRHub;

        public CreateModel(PostDbContext context, IHubContext<SignalRServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryID"] = new SelectList(
                _context.PostCategories,
                "CategoryID",
                "CategoryName"
            );
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (
                _context.Posts == null
                || Post == null
                || string.IsNullOrEmpty(Post.Title)
                || string.IsNullOrEmpty(Post.Content)
            )
            {
                return Page();
            }
            var accountSession = VaSession.Get<AppUser>(HttpContext.Session, "Account");
            Post.AuthorID = accountSession.UserID;
            Post.CreatedDate = DateTime.Now;
            Post.UpdatedDate = DateTime.Now;
            _context.Posts.Add(Post);
            _signalRHub.Clients.All.SendAsync("CreatePost", Post, accountSession.Email);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
