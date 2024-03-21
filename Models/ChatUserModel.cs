namespace chattiz_back.Models;

public enum ChatStatus
{
    None,
    Sended,
    Received,
}
public class ChatUserModel
{
    public string? Id { get; set;}

    public string? UserId { get; set;}

    public string? ChatId { get; set;}

    public ChatStatus? Status { get; set; }

    public string? LastMessager { get; set; }

    public int NumberOfMessages { get; set; }

};