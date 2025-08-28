namespace BrainCancerClassification.Helpers
{
    using OpenCvSharp;
    using Parquet;
    using Parquet.Data;
    using Parquet.Schema;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents data in Parquet file
    /// </summary>
    public  record MriData(int Label, Mat Image, string OriginalFileName) : IDisposable
    {
        public void Dispose()
        {
            Image?.Dispose();
        }
    }

    /// <summary>
    /// A class for uploading, processing and saving Alzheimer MRI Dataset
    /// </summary>
    public class PreprocessorAlzheimer
    {
        /// <summary>
        /// Dictionary that matches labels with categories
        /// </summary>
        public static readonly IReadOnlyDictionary<int, string> DiseaseLabelFromCategory = new Dictionary<int, string>
        {
            { 0, "Mild_Demented" },
            { 1, "Moderate_Demented" },
            { 2, "Non_Demented" },
            { 3, "Very_Mild_Demented" }
        };

        /// <summary>
        /// Loads MRI data from given file path
        /// </summary>
        public static async Task<List<MriData>> LoadDataFromParquetAsync(string parquetFilePath)
        {
            if (!File.Exists(parquetFilePath))
                throw new FileNotFoundException($"Parquet dosyası bulunamadı: {parquetFilePath}");

            using Stream fileStream = File.OpenRead(parquetFilePath);
            using ParquetReader parquetReader = await ParquetReader.CreateAsync(fileStream);

            var labelField = parquetReader.Schema.DataFields.FirstOrDefault(f => f.Path.ToString() == "label");
            var imageBytesField = parquetReader.Schema.DataFields.FirstOrDefault(f => f.Path.ToString() == "image/bytes");

            if (labelField == null || imageBytesField == null)
            {
                throw new InvalidDataException("Parquet dosyasında 'label' veya 'image/bytes' sütunları bulunamadı.");
            }

            var mriDataList = new List<MriData>();
            for (int i = 0; i < parquetReader.RowGroupCount; i++)
            {
                using ParquetRowGroupReader rowGroupReader = parquetReader.OpenRowGroupReader(i);

                var labelsColumn = await rowGroupReader.ReadColumnAsync(labelField);
                var imageBytesColumn = await rowGroupReader.ReadColumnAsync(imageBytesField);

                // Veriyi doğrudan nullable long dizisi olarak oku.
                var labels = (long?[])labelsColumn.Data;
                var imageByteArrays = (byte[][])imageBytesColumn.Data;

                // LINQ Zip ile etiketleri ve resimleri birleştir, sonra null olanları temizle.
                var processedData = labels.Zip(imageByteArrays, (label, bytes) => new { Label = label, Bytes = bytes })
                    .Where(pair =>
                        pair.Label.HasValue &&          // Sadece null olmayan etiketleri al
                        pair.Bytes != null &&           // Null olmayan resim verilerini al
                        pair.Bytes.Length > 0)          // Boş olmayan resim verilerini al
                    .Select(pair =>
                    {
                        // Cv2.ImDecode null döndürebilir, bu yüzden bir ara değişkene atıyoruz.
                        var imageMat = Cv2.ImDecode(pair.Bytes, ImreadModes.Grayscale);
                        if (imageMat != null && !imageMat.Empty())
                        {
                            // Etiketin .Value özelliğini kullanarak null olmayan değerini al ve int'e çevir.
                            return new MriData((int)pair.Label.Value, imageMat, $"{Guid.NewGuid()}.jpg");
                        }
                        return null; // Eğer resim decode edilemezse null döndür.
                    })
                    .Where(mriData => mriData != null); // Sadece başarılı bir şekilde oluşturulan MriData nesnelerini al.

                mriDataList.AddRange(processedData!);
            }

            // Artık ConvertLabels metoduna ihtiyacımız yok.
            return mriDataList;
        }

        /// <summary>
        /// Process and save images to directories by category
        /// </summary>
        public static void ProcessAndSaveImages(IEnumerable<MriData> data, string outputDir, Size newSize)
        {
            if (newSize.Width <= 0 || newSize.Height <= 0)
                throw new ArgumentException("Size must be positive");

            Directory.CreateDirectory(outputDir);

            int saved = 0;
            int errors = 0;

            foreach (var item in data)
            {
                try
                {
                    // Get category folder
                    string categoryName = DiseaseLabelFromCategory[item.Label];
                    string categoryDir = Path.Combine(outputDir, categoryName);
                    Directory.CreateDirectory(categoryDir);

                    // Resize image
                    using var resized = new Mat();
                    Cv2.Resize(item.Image, resized, newSize);

                    // Save image
                    string filePath = Path.Combine(categoryDir, item.OriginalFileName);
                    Cv2.ImWrite(filePath, resized);
                    saved++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving {item.OriginalFileName}: {ex.Message}");
                    errors++;
                }
            }

            Console.WriteLine($"Saved: {saved}, Errors: {errors}");
        }
    }
}