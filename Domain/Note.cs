namespace Domain;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string[] Tags { get; set; }
}