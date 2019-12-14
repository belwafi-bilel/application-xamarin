using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using RealEstate.API.Controllers.CustomHttpRequest;

namespace RealEstate.API.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected HttpResponseMessage ReturnFileResult(Stream stream, string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return result;
        }

        protected async Task<MultiFileUploadProvider.FileUploadDetails> GetFileMultiDataFromRequest()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            // file path
            var fileuploadPath = HttpContext.Current.Server.MapPath("~/App_Data/File_Upload");

            var multiFormDataStreamProvider = new MultiFileUploadProvider(fileuploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);

            string uploadingFileName = multiFormDataStreamProvider
                .FileData.Select(x => x.LocalFileName).FirstOrDefault();

            // Create response, assigning appropriate values to properties 
            return new MultiFileUploadProvider.FileUploadDetails
            {
                FilePath = uploadingFileName,

                FileInfo = new FileInfo(uploadingFileName),
            };
        }
    }
}