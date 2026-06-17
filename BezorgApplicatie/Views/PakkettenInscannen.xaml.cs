namespace BezorgApplicatie.Views;

public partial class PakkettenInscannen : ContentPage
{
	public PakkettenInscannen()
	{
		InitializeComponent();
	}
    public async void TakePhotoCommand(object sender, EventArgs args)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }
}