using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRAssignment.Common;

namespace Ass3.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            // remove the account from the session
            VaSession.Remove(HttpContext.Session, "Account");

            System.Threading.Thread.Sleep(1000);

            Response.Redirect("/Index");
        }
    }
}
