namespace RestApiWithServiceWorker.Domain;

public class Scanner
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsFeeder { get; set; }
    public bool isDuplex { get; set; } = true;
    public bool IsAuto { get; set; }

    public string Format { get; set; }

    public bool isDebug { get; set; }

}


