using Ass3.Models;
using Microsoft.AspNetCore.SignalR;

namespace Ass3.Hubs
{
    public class SignalRServer : Hub
    {
        public async Task UpdatePost(Post post, string email)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("UpdatePost", post, email);
        }
    }
}
