using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeTypes;
using RestApiWithServiceWorker.Domain;

namespace RestApiWithServiceWorker.Utils
{
    class CommonsUtilst
    {
        private readonly ILogger _logger;

        public CommonsUtilst(ILogger logger)
        {
            _logger = logger;
        }


        public void PrintConsoleAndLogFile(string namelog, object obj)
        {
            _logger.LogInformation(namelog + ": " + obj);
        }


        public byte[] GetByteLocalFile(string pathTempDir)
        {
            if (File.Exists(pathTempDir))
            {
                using var fs = new FileStream(pathTempDir, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs);
                var numBytes = new FileInfo(pathTempDir).Length;
                var buff = br.ReadBytes((int)numBytes);
                File.Delete(pathTempDir);
                return buff;
            }
            else
            {
                PrintConsoleAndLogFile("File not exist to path", pathTempDir);
                return new byte[0];
            }
        }

        public string CreateUrl(MessageResponse messageResponse)
        {
            var urlToRequestNaumen = messageResponse.Url + "/services/rest/" + messageResponse.Rest + "/" + messageResponse.Uuid + "?accessKey=" + messageResponse.AccessKey;
            
            if (messageResponse.Attr.Length != 0)
                urlToRequestNaumen += "?accessKey=" + messageResponse.AccessKey;
            
            return urlToRequestNaumen;
        }

        public async Task<string> Upload(byte[] data, string url, MessageResponse messageResponse, string path)
        {
            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();
            content.Headers.ContentType.MediaType = "multipart/form-data";
            var streamContent = new StreamContent(new MemoryStream(data));
            var contentType = MimeTypeMap.GetMimeType(Path.GetExtension(path));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Add(streamContent, messageResponse.File, messageResponse.Fname);

            using var message = await client.PostAsync(url, content);
            var input = await message.Content.ReadAsStringAsync();
            return input;
        }

        public void PrintLog(MessageResponse messageResponse, byte[] data)
        {
            PrintConsoleAndLogFile("File pushed tpo Naumen SD property", "");
            PrintConsoleAndLogFile("File original name", messageResponse.File);
            PrintConsoleAndLogFile("File custom name", messageResponse.Fname);
            PrintConsoleAndLogFile("File size", data.Length);
            PrintConsoleAndLogFile("UUID", messageResponse.Uuid);
            PrintConsoleAndLogFile("Method", messageResponse.Rest);
        }
    }
}