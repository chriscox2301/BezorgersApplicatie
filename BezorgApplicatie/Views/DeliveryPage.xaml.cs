using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using ZXing.Net.Maui;

namespace BezorgApplicatie.Views;

//Shift is commented because I added it but do not remember why(I am keeping it in case I remember).
public partial class DeliveryPage : ContentPage
{
	private readonly DataContext _dataContext;
	//public Shift Shift { get; set; }
	public Order Order {  get; set; }
	public ObservableCollection<Package> Packages { get; set; }


    public DeliveryPage(DataContext dataContext)
	{
		InitializeComponent();
		barcodeReader.Options = new BarcodeReaderOptions
		{
			AutoRotate = true,
			Multiple = false,
			Formats = BarcodeFormat.Code128 | BarcodeFormat.Code39 | BarcodeFormat.Ean13,
            TryHarder = true
		};
		_dataContext = dataContext;
		//Shift = new Shift();
		Order = new Order();
		Packages = new ObservableCollection<Package>();
		BindingContext = this;
		InitPage();
	}

	private async void InitPage()
	{
        //await LoadShift();
        await LoadOrder();
        await LoadPackages();
    }

	//private async Task LoadShift()
	//{
	//	Placeholder->Takes the first shift of the driver named Piet.
	//	try
	//	{
	//		await _dataContext.Database.EnsureCreatedAsync();
	//		var shift = await _dataContext.Shifts.Where(s => s.Driver.Name == "Piet").FirstAsync();
	//		Shift = shift;
	//	}
	//	catch (Exception ex)
	//	{
	//		await DisplayAlert("Error", $"Er ging iets fout bij het laden van de huidige dienst: {ex.Message}", "OK");
	//	}
	//}

	private async Task LoadOrder()
    {
		//Placeholder -> Takes the first Order in the db.
        try
        {
            await _dataContext.Database.EnsureCreatedAsync();
            Order = await _dataContext.Orders.FirstAsync();
			OnPropertyChanged(nameof(Order));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Er ging iets fout bij het laden van de order: {ex.Message}", "OK");
        }
    }

    private async Task LoadPackages()
	{ 
		//Takes every package that belongs to the current Order.
        try
        {
            await _dataContext.Database.EnsureCreatedAsync();
			var packages = await _dataContext.Packages.Where(p => p.OrderId == Order.Id).ToListAsync();
			foreach (var package in packages)
			{
				Packages.Add(package);
			}
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Er ging iets fout bij het laden van de pakketten: {ex.Message}", "OK");
        }
    }

    private void barcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
		//When a barcode is scanned it searches the current order for a package with that barcode
		//and updates the status accordingly
		var first = e.Results.FirstOrDefault();
		if (first == null)
		{
			return;
		}

        Dispatcher.DispatchAsync(async () =>
        {
            barcodeReader.IsDetecting = false;
            var matchedPackage = Packages.FirstOrDefault(p => p.Barcode == first.Value);

            if (matchedPackage != null)
            {
                matchedPackage.Status = "Present";
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(50));
                await DisplayAlert("Goed", $"Pakketnummer {matchedPackage.Number}", "Ok");
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                await DisplayAlert("Fout", $"Dit pakket is niet in deze Order", "Ok");
            }

            await Task.Delay(3000);
            barcodeReader.IsDetecting = true;
        });
    }
}