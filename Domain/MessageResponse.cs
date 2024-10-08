namespace RestApiWithServiceWorker.Domain
{
    public class MessageResponse
    {
        public string Rest { get; set; } = "add-file";
        public string AccessKey { get; set; }
        public string Uuid { get; set; }
        
        public string Subject { get; set; } //UUid
        public string Hostname { get; set; } //Url

        public string File { get; set; }
        public string Fname { get; set; }
        public string Url { get; set; }

        public string Attr { get; set; } = "";

        public string QueryString { get; set; }

        public bool IsValid =>
            !string.IsNullOrEmpty(Url) && !string.IsNullOrEmpty(Rest) && !string.IsNullOrEmpty(Uuid) &&
            !string.IsNullOrEmpty(AccessKey);


        public override string ToString()
        {
            return
                $"{nameof(Rest)}: {Rest}, {nameof(AccessKey)}: {AccessKey}, {nameof(Uuid)}: {Uuid}, {nameof(File)}: {File}, {nameof(Fname)}: {Fname}, {nameof(Url)}: {Url}, {nameof(Attr)}: {Attr}, {nameof(IsValid)}: {IsValid}";
        }
    }
}