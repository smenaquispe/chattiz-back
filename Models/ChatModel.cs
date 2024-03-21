namespace chattiz_back.Models;

public enum ChatStatus
{
    Sended,
    Received,
}

public class ChatModel {

    /*
    * This class is used to represent a chat in the system.
    Owner: The username of the user that created the chat.
    ChatName: The name of the chat (the name of the other user in a private chat, or the name of the group chat in a group chat)
    

    */
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? OwnerId { get; set; }

    public string[]? Participants { get; set; }

    public ChatStatus? Status { get; set; }

    public string? LastMessager { get; set; }

    public int NumberOfMessages { get; set; }

};