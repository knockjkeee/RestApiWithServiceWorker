using System;
using System.IO;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;
using MimeTypes;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Utils
{
    class CommonsUtilst
    {
        private readonly ILogger _logger;

        public CommonsUtilst(ILogger logger)
        {
            _logger = logger;
        }


        public void printConsoleAndLogFile(string namelog, object obj)
        {
            _logger.LogInformation(namelog + ": " + obj);
            // System.Console.WriteLine(namelog + ": " + obj);
        }


        public byte[] getByteLocalFile(string pathTempDir)
        {
            byte[] buff = null;
            if (File.Exists(pathTempDir))
            {
                FileStream fs = new FileStream(pathTempDir, FileMode.Open, FileAccess.Read);
                // FileStream fs = File.OpenRead(pathTempDir);
                // fs.Name.get
                // string v = Path.GetExtension(pathTempDir);
                // Console.WriteLine(v);

                // string contentType = MimeTypeMap.GetMimeType(Path.GetExtension(pathTempDir));
                // Console.WriteLine(contentType);
                // // File.Create();


                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(pathTempDir).Length;
                buff = br.ReadBytes((int)numBytes);
                fs.Close();
                File.Delete(pathTempDir);
                return buff;
            }
            else
            {
                printConsoleAndLogFile("File not exist to path", pathTempDir);
                return new byte[0];
            }
        }

        public string createUrl(MessageResponse messageResponse)
        {
            string urlToRequestNaumen;
            if (messageResponse.Attr.Length == 0)
            {
                urlToRequestNaumen = messageResponse.Url + "/services/rest/" + messageResponse.Rest + "/" + messageResponse.Uuid + "?accessKey=" + messageResponse.AccessKey;
            }
            else
            {
                urlToRequestNaumen = messageResponse.Url + "/services/rest/" + messageResponse.Rest + "/" + messageResponse.Uuid + "?accessKey=" + messageResponse.AccessKey + "&attrCode=" + messageResponse.Attr;

            }
            return urlToRequestNaumen;
        }

        public async Task<string> Upload(byte[] data, string url, MessageResponse messageResponse, string path)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Headers.ContentType.MediaType = "multipart/form-data";
                    StreamContent streamContent = new StreamContent(new MemoryStream(data));
                    string contentType = MimeTypeMap.GetMimeType(Path.GetExtension(path));
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    content.Add(streamContent, messageResponse.File, messageResponse.Fname);

                    using (var message = await client.PostAsync(url, content))
                    {
                        var input = await message.Content.ReadAsStringAsync();
                        // return response Naumen
                        return input;
                    }
                }
            }
        }
    }
}