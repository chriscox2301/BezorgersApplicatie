using System.Collections.ObjectModel;
using System.Data;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace BezorgApplicatie.Views;
public partial class PakkettenInscannen : ContentPage
{
    private ObservableCollection<string> scannedBarcodes;

    public PakkettenInscannen()
    {
        InitializeComponent();
        scannedBarcodes = new ObservableCollection<string>();
        BindingContext = this;

        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.Code39,
            AutoRotate = true
        };

        barcodeReader.BarcodesDetected += OnBarcodesDetected;
    }

    public ObservableCollection<string> ScannedBarcodes => scannedBarcodes;

    public void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var barcode in e.Results)
            {
                if (!scannedBarcodes.Contains(barcode.Value))
                {
                    scannedBarcodes.Add(barcode.Value);
                }
            }
        });
    }

    public static class Constants
    {
        public const string DatabaseFilename = "MatrixIncBezorger.db";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
