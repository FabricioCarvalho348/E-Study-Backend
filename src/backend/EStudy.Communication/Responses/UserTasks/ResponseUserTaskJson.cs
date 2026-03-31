namespace EStudy.Communication.Responses.UserTasks;

public class ResponseUserTaskJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedOn { get; set; }
}

