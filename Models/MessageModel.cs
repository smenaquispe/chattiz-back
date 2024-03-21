namespace chattiz_back.Models;

public class MessageModel {

    public string? Id { get; set;}
    public string? ChatId { get; set; }
    public string? SenderId { get; set; }
    public string? Content { get; set; }
    public DateTime SentAt { get; set; }

};