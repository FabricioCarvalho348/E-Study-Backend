namespace EStudy.Communication.Requests.Events;

public class RequestPatchEventJson
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public bool? IsAllDay { get; set; }
}

