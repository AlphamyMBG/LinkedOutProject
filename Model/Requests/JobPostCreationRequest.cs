

namespace BackendApp.Model.Requests;

public class JobCreationRequest
(string title, string description, string requirements)
{
    public string Title{get; set;} = title;
    public string Description{get; set;} = description;
    public string Requirements{get; set;} = requirements;
}
