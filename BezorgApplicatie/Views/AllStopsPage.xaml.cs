using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace BezorgApplicatie.Views;

public partial class AllStopsPage : ContentPage
{
    private readonly DataContext _dataContext;
    public ObservableCollection<Order> AlleStops { get; set; } = new ObservableCollection<Order>();

    public string OrderStatus = "Onderweg";
    public string ZoekTerm = "";
	public AllStopsPage(DataContext dataContext)
	{
		InitializeComponent();
        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            AutoRotate = true,
            Multiple = false,
            Formats = ZXing.Net.Maui.BarcodeFormat.Code128 | ZXing.Net.Maui.BarcodeFormat.Code39,
            TryHarder = true
        };
        _dataContext = dataContext;
        BindingContext = this;
        LaadStops();
        OrderStatus = "Onderweg";
        Filter();

        Dispatcher.StartTimer(TimeSpan.FromMinutes(5), () =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                
                await LaadStops();
                Filter();
            });

            return true;
        });
    }


    public async Task LaadStops()
    {
        var stops = await _dataContext.Orders
                        .AsNoTracking()
                        .Include(s => s.Packages)
                        .ToArrayAsync();

        AlleStops.Clear();

        foreach (var stop in stops)
        {
            AlleStops.Add(stop);
        }
    }

    private void Filter()
    {
        var FilteredStops = AlleStops.Where(s => s.Status == OrderStatus);

        if (!string.IsNullOrWhiteSpace(ZoekTerm))
        {
            FilteredStops = FilteredStops.Where(s => s.Address.ToLower().Contains(ZoekTerm)
            || s.Id.ToString().ToLower().Contains(ZoekTerm)
            || s.Packages.Any(p => p.Barcode.ToLower().Contains(ZoekTerm))
            );
        }

        StopsList.ItemsSource = FilteredStops.ToList();
    }

    private void ToDoClicked(object sender, EventArgs e)
    {
        OrderStatus = "Onderweg";
        Filter();
    }

    private void CompletedClicked(object sender, EventArgs e)
    {
        OrderStatus = "Bezorgd";
        Filter();
    }

    private void DifferentClicked(object sender, EventArgs e)
    {
        OrderStatus = "Anders";
        Filter();
    }

    private void ZoektermIngevuld(object sender, TextChangedEventArgs e)
    {
        ZoekTerm = e.NewTextValue.ToLower();
        Filter();
    }

    private async void StopSelected(object sender, SelectionChangedEventArgs e)
    {
        var order = e.CurrentSelection.FirstOrDefault() as Order;

        if (order == null)
            return;

        StopsList.SelectedItem = null;

        await Navigation.PushAsync(new StopDetailPage(order));
    }

    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first is null)
            return;

        Dispatcher.DispatchAsync(async () =>
        {
            CameraVenster.IsVisible = false;
            barcodeReader.IsDetecting = false;
            Zoekbalk.Text = first.Value;
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
            await DisplayAlert("Barcode Gedecteerd", first.Value, "OK");

        });
    }

    private void ScanButton_Clicked(object sender, EventArgs e)
    {
        CameraVenster.IsVisible = !CameraVenster.IsVisible;

        if (CameraVenster.IsVisible)
        {
            barcodeReader.IsDetecting = true;
        }
        else
        {
            barcodeReader.IsDetecting = false;
        }
    }
}