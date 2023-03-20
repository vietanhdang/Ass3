using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ass3.Models;
using SignalRAssignment.Common;

namespace Ass3.Pages_Post
{
    [Authentication]
    public class IndexModel : PageModel
    {
        private readonly PostDbContext _context;

        public IndexModel(PostDbContext context)
        {
            _context = context;
        }

        public IList<Post> Post { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Posts != null)
            {
                Post = await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Category)
                    .Where(x => x.PublishStatus == true || x.AuthorID == VaSession.Get<AppUser>(HttpContext.Session, "Account").UserID)
                    .OrderByDescending(x => x.UpdatedDate)
                    .ToListAsync();
            }
        }
    }
}
