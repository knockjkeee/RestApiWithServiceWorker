namespace RestApiWithServiceWorker.Domain
{
    public class TwainMessageResponse
    {
        public string Rest { get; set; } = "add-file";
        public string AccessKey { get; set; }
        
        public string Subject { get; set; } //UUid

        public string File { get; set; }
        public string Fname { get; set; }
        public string Hostname { get; set; } //Url

        public string Attr { get; set; } = "";
        

        public MessageResponse reFormat()
        {
            return new MessageResponse()
            {
                Url = Hostname,
                Attr = Attr,
                File = File,
                Fname = Fname,
                Rest = Rest,
                AccessKey = AccessKey,
                Uuid = Subject
            };
        }

        public bool IsValid =>
            !string.IsNullOrEmpty(Hostname) && !string.IsNullOrEmpty(Rest) && !string.IsNullOrEmpty(Subject) &&
            !string.IsNullOrEmpty(AccessKey);


        public override string ToString()
        {
            return
                $"{nameof(Rest)}: {Rest}, {nameof(AccessKey)}: {AccessKey}, {nameof(Subject)}: {Subject}, {nameof(File)}: {File}, {nameof(Fname)}: {Fname}, {nameof(Hostname)}: {Hostname}, {nameof(Attr)}: {Attr}, {nameof(IsValid)}: {IsValid}";
        }
    }
}