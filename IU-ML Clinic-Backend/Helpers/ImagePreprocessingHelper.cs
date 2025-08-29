using OpenCvSharp;
using System.IO;

namespace BrainCancerClassification.Helpers
{
    public static class ImagePreprocessingHelper
    {
        public static Mat PreprocessImage(Stream imageStream, int width, int height)
        {
            using (var memoryStream = new MemoryStream())
            {
                imageStream.CopyTo(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                Mat image = Cv2.ImDecode(imageBytes, ImreadModes.Color);

                // Resize the image
                Mat resizedImage = new Mat();
                Cv2.Resize(image, resizedImage, new Size(width, height));

                // Normalize the image (example: scale pixel values to [0, 1])
                Mat normalizedImage = new Mat();
                resizedImage.ConvertTo(normalizedImage, MatType.CV_32F, 1.0 / 255.0);

                return normalizedImage;
            }
        }
    }
}
