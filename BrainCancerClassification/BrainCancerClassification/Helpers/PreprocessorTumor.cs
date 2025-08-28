namespace BrainCancerClassification.Helpers
{
    using System;
    using System.IO;
    using OpenCvSharp;
    using System.Linq;

    class PreprocessorTumor
    {
        public static void CropAndResizeImages(string inputDir, string outputDir,int IMG_SIZE)
        {
            // For every folder in the directory to crop and resize images
            foreach (var dir in Directory.GetDirectories(inputDir))
            {
                string folderName = Path.GetFileName(dir);
                string savePath = Path.Combine(outputDir, folderName);
                Directory.CreateDirectory(savePath);

               // For every file in the folder to get cropped and resized
                foreach (var imgPath in Directory.GetFiles(dir))
                {
                    try
                    {
                        using var image = Cv2.ImRead(imgPath);

                        if (image.Empty())
                            continue;

                        using var newImg = CropImage(image);
                        using var resized = new Mat();
                        Cv2.Resize(newImg, resized, new Size(IMG_SIZE, IMG_SIZE));

                        string saveFile = Path.Combine(savePath, Path.GetFileName(imgPath));
                        Cv2.ImWrite(saveFile, resized);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {imgPath}: {ex.Message}");
                    }
                }
            }
        }

        private static Mat CropImage(Mat img)
        {
            //Finds extreme points on images and crops a rectengular size out of them
            using var gray = new Mat();
            Cv2.CvtColor(img, gray, ColorConversionCodes.RGB2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(3, 3), 0);

            // Applies threshold, eroisions and dilation to remove small region of noises
            using var thresh = new Mat();
            Cv2.Threshold(gray, thresh, 45, 255, ThresholdTypes.Binary);
            Cv2.Erode(thresh, thresh, null, iterations: 2);
            Cv2.Dilate(thresh, thresh, null, iterations: 2);

            // Find countours in the threshold image
            Cv2.FindContours(thresh.Clone(), out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            if (contours.Length == 0)   
                return img;

           // Take the largest one
           var largestContour = contours
            .OrderByDescending(c => Cv2.ContourArea(c))
            .First();

            // Define the extreme points
            var extLeft = largestContour.OrderBy(p => p.X).First();
            var extRight = largestContour.OrderByDescending(p => p.X).First();
            var extTop = largestContour.OrderBy(p => p.Y).First();
            var extBottom = largestContour.OrderByDescending(p => p.Y).First();

            int addPixels = 0;
            int x1 = Math.Max(extLeft.X - addPixels, 0);
            int x2 = Math.Min(extRight.X + addPixels, img.Width - 1);
            int y1 = Math.Max(extTop.Y - addPixels, 0);
            int y2 = Math.Min(extBottom.Y + addPixels, img.Height - 1);

            return new Mat(img, new Rect(x1, y1, x2 - x1, y2 - y1));
        }
    }

}
