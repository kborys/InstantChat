using Microsoft.AspNetCore.Identity;

namespace InstantChat.SignalR.Models;

public class AppUser : IdentityUser<int>
{
    public AppUser()
    {
        Messages = new HashSet<Message>();
    }
    public virtual ICollection<Message> Messages { get; set; }
}