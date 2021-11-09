namespace RestApiWithServiceWorker.Domain
{
    public class MessageResponse
    {
        public string Rest { get; set; }
        public string AccessKey { get; set; }
        public string Uuid { get; set; }

        public string File { get; set; }
        public string Fname { get; set; }
        public string Url { get; set; }

        public string Attr { get; set; } = "";

        public override string ToString()
        {
            return "Rest: " + Rest + " AccesKey: " + AccessKey + " uuid: " + Uuid + " File: " + File + " Fname: " + Fname + " url: " + Url + " Attr: " + Attr;
        }
    }



}