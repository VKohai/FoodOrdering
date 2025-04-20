namespace FoodOrdering.Helpers;
public static class ImageHelper {
    public static async Task<ImageSource?> DownloadImageAsync(this string imagePath) {
        // Step 1: Download the image as a byte array
        string bucketId = imagePath.Substring(0, imagePath.IndexOf('/'));
        imagePath = imagePath.Substring(imagePath.IndexOf('/') + 1);
        var response = await SB.Storage.From(bucketId).Download(imagePath, onProgress: null);

        if (response == null || response.Length == 0)
            return null; // Return null if the image couldn't be downloaded

        // Step 2: Convert the byte array to a stream and create an ImageSource
        var stream = new MemoryStream(response);
        return ImageSource.FromStream(() => stream);
    }
}
