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

}


