using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ZXing.Net.Maui;

namespace BezorgApplicatie.Views;

//Shift is commented because I added it but do not remember why(I am keeping it in case I remember).
public partial class DeliveryPage : ContentPage
{
	private readonly DataContext _dataContext;
	//public Shift Shift { get; set; }
	public Order Order {  get; set; }
	public ObservableCollection<Package> Packages { get; set; }


    public DeliveryPage(DataContext dataContext, Order order)
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
        Order = order;

		Packages = new ObservableCollection<Package>();
        Packages.CollectionChanged += Packages_CollectionChanged;

        BindingContext = this;
		InitPage();
	}

	private async void InitPage()
	{
        //await LoadShift();
        //await LoadOrder();
        await LoadPackages();
        UpdateFeedbackLabel();
    }

    protected virtual void OnAppearing()
    {
        base.OnAppearing();
        InitPage();
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
                //Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(50));
                //await DisplayAlert("Goed", $"Pakketnummer {matchedPackage.Number}", "Ok");
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
                await DisplayAlert("Fout", $"Dit pakket is niet in deze order", "Ok");
            }

            await Task.Delay(3000);
            barcodeReader.IsDetecting = true;
        });
    }

    private void Packages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // Zorgt ervoor dat als een package wordt toegevoegde aan de colleciton, 
        // dat hij de Package_PropertyChanged aanroept als er iets in de package verandert
        if (e.NewItems != null)
        {
            foreach (Package package in e.NewItems)
            {
                package.PropertyChanged += Package_PropertyChanged;
            }
        }

        // Zorgt ervoor dat als een item in de collection wordt verwijdert, 
        // dat hij niet meer de Package_PropertyChanged aanroept als hij verandert wordt.
        if (e.OldItems != null)
        {
            foreach (Package package in e.OldItems)
            {
                package.PropertyChanged -= Package_PropertyChanged;
            }
        }
    }

    private void Package_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //Controleert of het veranderde property de Status was.
        if (e.PropertyName == nameof(Package.Status))
        {
            UpdateFeedbackLabel();
        }
    }

    private void UpdateFeedbackLabel()
    {
        int current = Packages.Count(p => !string.IsNullOrEmpty(p.Status));
        int total = Packages.Count;
        if (current == total)
        {
            FeedbackLabel.TextColor = Colors.Green;
            BezorgdBtn.IsEnabled = true;
            BezorgdBtn.BackgroundColor = Colors.Blue; 
            AndersPicker.IsEnabled = true;
            AndersBorder.BackgroundColor = Colors.Orange;
        }
        FeedbackLabel.Text = $"{current}/{total}";
    }
    private async void OnManualClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        Package package = (Package)button.BindingContext;

        string result = await DisplayPromptAsync(
        "Handmatige invoer",
        $"Voer barcode in voor pakket {package.Number}:",
        initialValue: "",
        maxLength: 10,
        keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(result))
            return;

        if (result == package.Barcode)
        {
            package.Status = "Present";
            await _dataContext.SaveChangesAsync();
        }
        else
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
            await DisplayAlert("Fout", "Barcode komt niet overeen", "OK");
        }
    }
    private async void OnProblemClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not Package package)
            return;
            await Shell.Current.GoToAsync($"{nameof(ProblemPage)}?pakketId={package.Id}");
    }
    private async void BezorgdBtn_Clicked(object sender, EventArgs e)
    {
        Order.Status = "Bezorgd";
        await _dataContext.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    private void OnAndersPickerChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var selectedItem = picker.SelectedItem?.ToString();

        if (selectedItem == "Buren")
        {
            AndersPickerOption("Buren");
        }
        else if (selectedItem == "Pakketpunt")
        {
            AndersPickerOption("Pakketpunt");
        }
        else if (selectedItem == "Geweigerd")
        {
            AndersPickerOption("Geweigerd");
        }
        else if (selectedItem == "Anders")
        {
            AndersPickerOption("Anders");
        }
    }

    private async void AndersPickerOption(string option)
    {
        Order.Status = option;
        await _dataContext.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }
}

//TODO:
//Handmatig invoeren
//Bezorgd
//Anders