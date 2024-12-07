namespace Example06.Contracts;

public sealed record Message
{
    public Guid Id { get; init; }
    public string Text { get; init; }

    public Message()
    {
        Id = Guid.NewGuid();
        Text = $"Text for Id {Id}";
    }
}