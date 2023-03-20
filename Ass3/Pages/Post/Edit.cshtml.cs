using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ass3.Models;
using SignalRAssignment.Common;
using Ass3.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Ass3.Pages_Post
{
    [Authentication]
    public class EditModel : PageModel
    {
        private readonly PostDbContext _context;
        private readonly IHubContext<SignalRServer> _signalRHub;
        private readonly SignalRServer _signalRServer;

        public EditModel(
            PostDbContext context,
            IHubContext<SignalRServer> signalRHub,
            SignalRServer signalRServer
        )
        {
            _context = context;
            _signalRHub = signalRHub;
            _signalRServer = signalRServer;
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.PostID == id);
            if (post == null)
            {
                return NotFound();
            }
            Post = post;
            ViewData["CategoryID"] = new SelectList(
                _context.PostCategories,
                "CategoryID",
                "CategoryName"
            );
            return Page();
        }

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
            Post.UpdatedDate = DateTime.Now;
            _context.Attach(Post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _signalRHub.Clients.All.SendAsync("UpdatePost", Post, accountSession.Email);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.PostID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostID == id)).GetValueOrDefault();
        }
    }
}
