using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealEstate.Mobile.Helpers
{
    public static class ImageHelpers
    {
        private static object locker = new object();

        public static async Task<Stream> GetStreamFromImageSource(this ImageSource imageSource)
        {
            if (imageSource is StreamImageSource)
                return await GetStreamFromStreamImageSource(imageSource);
            else
                return GetStreamFromFileImageSource(imageSource);

        }
        private static async Task<Stream> GetStreamFromStreamImageSource(ImageSource imageSource)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            var result =  await streamImageSource.Stream(cancellationToken);
            return result;
        }

        private static Stream GetStreamFromFileImageSource(ImageSource imageSource)
        {
            lock (locker)
            {
                FileImageSource fileImageSource = (FileImageSource)imageSource;
                using (var file = File.Open(fileImageSource.File, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(file.Length);
                    file.Read(memStream.GetBuffer(), 0, (int)file.Length);
                    return memStream;
                }
            }
        }
    }
}
