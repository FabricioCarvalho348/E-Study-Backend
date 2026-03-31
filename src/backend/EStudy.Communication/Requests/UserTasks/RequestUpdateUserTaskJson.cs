namespace EStudy.Communication.Requests.UserTasks;

public class RequestUpdateUserTaskJson
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}

