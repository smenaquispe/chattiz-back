namespace chattiz_back.Models;

public enum ChatStatus
{
    None,
    Sended,
    Received,
}

public class ChatModel {

    public string? Id { get; set; }
    public string? Name { get; set; }

    public ChatStatus? Status { get; set; }

    public string? LastMessager { get; set; }

    public int NumberOfMessages { get; set; }

};