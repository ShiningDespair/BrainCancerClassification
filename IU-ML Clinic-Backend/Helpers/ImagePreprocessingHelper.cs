using OpenCvSharp;
using System;
using System.IO;

namespace BrainCancerClassification.Helpers
{
    public static class ImagePreprocessingHelper
    {
        public static Mat PreprocessImage(Stream imageStream, int targetWidth, int targetHeight)
        {
            using (var memoryStream = new MemoryStream())
            {
                imageStream.CopyTo(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                if (imageData == null || imageData.Length == 0)
                {
                    throw new ArgumentException("Image data is empty. The uploaded file might be empty or corrupt.");
                }

                Mat image = Cv2.ImDecode(imageData, ImreadModes.Color);
                if (image == null || image.Empty())
                {
                    throw new ArgumentException("The uploaded file could not be decoded as a valid image. It might be corrupt or in an unsupported format.");
                }

                var resizedImage = new Mat();
                Cv2.Resize(image, resizedImage, new Size(targetWidth, targetHeight));

                image.Dispose();

                return resizedImage;
            }
        }
    }
}