using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRMVCApp.Data;
using SignalRMVCApp.Models;

namespace SignalRMVCApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                Timestamp = DateTime.Now
            };

            // Save message to the database
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Broadcast message to all clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
