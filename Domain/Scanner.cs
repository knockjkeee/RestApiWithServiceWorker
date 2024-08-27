namespace RestApiWithServiceWorker.Domain;

public class Scanner
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsFeeder { get; set; }
    public bool isDuplex { get; set; }

    public bool IsAuto { get; set; } = true;

    public string Format { get; set; }

    public bool isDebug { get; set; }

    public string File { get; set; }

    public MessageResponse messageResponse { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(IsFeeder)}: {IsFeeder}, {nameof(isDuplex)}: {isDuplex}, {nameof(IsAuto)}: {IsAuto}, {nameof(Format)}: {Format}, {nameof(isDebug)}: {isDebug}, {nameof(File)}: {File}, {nameof(messageResponse)}: {messageResponse}";
    }
}