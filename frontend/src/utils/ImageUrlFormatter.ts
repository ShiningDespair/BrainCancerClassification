export class ImageUrlFormatter {
  public static formatGoogleDriveUrl(url: string | null | undefined): string {
    if (!url) {
      return '';
    }

    const fileIdRegex = /\/file\/d\/([a-zA-Z0-9_-]+)/;
    const fileIdMatch = url.match(fileIdRegex);

    if (fileIdMatch && fileIdMatch[1]) {
      const fileId = fileIdMatch[1];
      return `https://drive.google.com/uc?export=view&id=${fileId}`;
    }

    if (url.includes('/uc?id=')) {
      try {
        const urlObject = new URL(url);
        const fileId = urlObject.searchParams.get('id');
        if (fileId) {
          return `https://drive.google.com/uc?export=view&id=${fileId}`;
        }
      } catch (error) {
        console.error("Geçersiz URL formatı:", url, error);
        return url; // Hata durumunda orijinal URL'i döndür
      }
    }

    // Eğer bilinen bir Google Drive formatı değilse, orijinal URL'i geri döndür.
    return url;
  }
}