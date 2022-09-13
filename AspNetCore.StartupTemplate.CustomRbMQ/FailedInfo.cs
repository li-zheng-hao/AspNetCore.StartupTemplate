namespace AspNetCore.StartupTemplate.CustomRbMQ;

public class FailedInfo
{
    public IServiceProvider ServiceProvider { get; set; } = default!;

    public MessageType MessageType { get; set; }

    public Message Message { get; set; } = default!;
}