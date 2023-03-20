using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ass3.Models;
using SignalRAssignment.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ass3.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Ass3.Pages_Post
{
    [Authentication]
    public class DeleteModel : PageModel
    {
        private readonly PostDbContext _context;
        private readonly IHubContext<SignalRServer> _signalRHub;

        public DeleteModel(PostDbContext context, IHubContext<SignalRServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.PostID == id);

            if (post == null)
            {
                return NotFound();
            }
            else
            {
                Post = post;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FindAsync(id);

            if (post != null)
            {
                Post = post;
                _context.Posts.Remove(Post);
                await _context.SaveChangesAsync();
                await _signalRHub.Clients.All.SendAsync("DeletePost", Post);
            }

            return RedirectToPage("./Index");
        }
    }
}
