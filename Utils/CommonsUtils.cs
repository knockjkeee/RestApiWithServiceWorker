using System;
using System.IO;
using Microsoft.Extensions.Logging;
using RestApiWithServiceWorker.Domain;

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
            FileStream fs = new FileStream(pathTempDir, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(pathTempDir).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
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
    }
}