namespace RestApiWithServiceWorker.Domain;

public class IndexDTO
{
    public string Scanner { get; set; }
    public string Format { get; set; } 
    public bool IsDuplex  { get; set; }
    public bool IsFeeder  { get; set; }
}