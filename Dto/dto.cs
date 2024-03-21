namespace chattiz_back.Dto;

public class ChatCreateModel
{
    public string? Name { get; set;}
    public string[]? UserIds { get; set;}

};

public class ChatAddUsers
{
    public string? ChatId { get; set;}
    public string[]? UserIds { get; set;}
};

public class ChatRemoveUser
{
    public string? ChatId { get; set;}
    public string? UserId { get; set;}
};