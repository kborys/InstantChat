using InstantChat.SignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace InstantChat.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message) => await Clients.All.SendAsync("ReceiveMessage", message);
    }
}