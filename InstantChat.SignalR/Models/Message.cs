using System.ComponentModel.DataAnnotations;

namespace InstantChat.SignalR.Models;

public class Message
{
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Text { get; set; }
    public DateTime Sent { get; set; }
    public int UserId { get; set; }
    public virtual AppUser User { get; set; }
}