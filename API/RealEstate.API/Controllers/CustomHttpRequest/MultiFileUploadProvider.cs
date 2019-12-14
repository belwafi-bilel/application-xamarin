using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RealEstate.API.Controllers.CustomHttpRequest
{
    public class MultiFileUploadProvider : MultipartFormDataStreamProvider
    {
        public MultiFileUploadProvider(string rootPath) : base(rootPath) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            if (headers != null && headers.ContentDisposition != null)
            {
                return headers
                    .ContentDisposition
                    .FileName.TrimEnd('"').TrimStart('"');
            }

            return base.GetLocalFileName(headers);
        }

        public class FileUploadDetails
        {
            public string FilePath { get; set; }
            public FileInfo FileInfo { get; set; }

        }
    }
}