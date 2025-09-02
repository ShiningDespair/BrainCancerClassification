export const getCloudinaryPublicId = (url: string): string | null => {
  if (!url || !url.includes('cloudinary.com')) {
    return null;
  }
  const parts = url.split('/upload/');
  if (parts.length < 2) {
    return null;
  }
  const publicIdPart = parts[1].split('/').pop(); // Get the last segment
  if (publicIdPart) {
    // Remove the file extension
    return publicIdPart.split('.')[0];
  }
  return null;
};