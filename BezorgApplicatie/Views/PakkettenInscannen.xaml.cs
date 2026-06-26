using System.Collections.ObjectModel;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views;

public class ScannedPackageViewModel
{
    public Package Package { get; set; }
    public string BarcodeDisplay => Package?.Barcode ?? "N/A";
    public string WeightDisplay => Package?.Weight.ToString("F2") + " kg" ?? "N/A";
    public int Id => Package?.Id ?? 0;
}

public partial class PakkettenInscannen : ContentPage, INotifyPropertyChanged
{
    private ObservableCollection<ScannedPackageViewModel> scannedBarcodes;
    private string selectedBarcode;
    private DataContext _dataContext;
    private int _scannedCount = 0;
    private int _maxPackagesPerZone = 1;
    private List<string> _zones = new() { "D", "C", "B", "A" };
    private int _currentZoneIndex = 0;
    private string _currentZone = "D";
    private string _counterDisplay = "0/1";

    public string CurrentZone
    {
        get => _currentZone;
        set
        {
            if (_currentZone != value)
            {
                _currentZone = value;
                OnPropertyChanged();
            }
        }
    }

    public string CounterDisplay
    {
        get => _counterDisplay;
        set
        {
            if (_counterDisplay != value)
            {
                _counterDisplay = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

        public PakkettenInscannen(DataContext dataContext)
        {
            InitializeComponent();
            _dataContext = dataContext;
            scannedBarcodes = new ObservableCollection<ScannedPackageViewModel>();
            BindingContext = this;

        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            TryHarder = true
        };

        barcodeReader.BarcodesDetected += OnBarcodesDetected;
    }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (_dataContext == null)
                {
                    await DisplayAlert("Error", "Database context niet geïnitialiseerd", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to initialize database: {ex.Message}", "OK");
            }
        }

    public ObservableCollection<ScannedPackageViewModel> ScannedBarcodes => scannedBarcodes;

    public void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (_dataContext == null)
            {
                await DisplayAlert("Error", "Database context is nog niet ge�nitialiseerd", "OK");
                return;
            }

            foreach (var barcode in e.Results)
            {
                if (!scannedBarcodes.Any(p => p.BarcodeDisplay == barcode.Value))
                {
                    try
                    {
                        var package = await _dataContext.Packages
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Barcode == barcode.Value);

                        if (package != null)
                        {
                            scannedBarcodes.Add(new ScannedPackageViewModel { Package = package });
                            UpdateCounter();
                        }
                        else
                        {
                            await DisplayAlert("Info", $"Package met barcode {barcode.Value} niet gevonden in database", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Database error: {ex.Message}\n\nStackTrace: {ex.InnerException?.Message}", "OK");
                    }
                }
            }
        });
    }

    private void UpdateCounter()
    {
        _scannedCount = scannedBarcodes.Count;
        CounterDisplay = $"{_scannedCount}/{_maxPackagesPerZone}";
    }

    public async void ProbleemMelden(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ScannedPackageViewModel packagevm)
        {
            var barcode = packagevm.BarcodeDisplay;
            await Shell.Current.GoToAsync($"probleem-melden?barcode={barcode}");
        }
    }

    public async void VolgendeZone(object sender, EventArgs e)
    {
        if (_scannedCount >= _maxPackagesPerZone)
        {
            if (_currentZoneIndex < _zones.Count - 1)
            {
                _currentZoneIndex++;
                CurrentZone = _zones[_currentZoneIndex];

                scannedBarcodes.Clear();
                _scannedCount = 0;
                UpdateCounter();
                if(CurrentZone == "A")
                {
                    NextBtn.Text = "Start Shift";
                    NextBtn.BackgroundColor = Colors.Blue;
                    NextBtn.TextColor = Colors.White;
                }

                await DisplayAlert("Zone", $"Volgende zone: {CurrentZone}", "OK");
            }
            else
            {
                await DisplayAlert("Info", "Alle zones zijn afgerond!", "OK");
                await Shell.Current.GoToAsync("///RouteMap");
            }
        }
        else
        {
            await DisplayAlert("Info", $"Scan eerst alle pakketten. ({_scannedCount}/{_maxPackagesPerZone})", "OK");
        }
    }

    public async void VorigeZone(object sender, EventArgs e)
    {
        if (_currentZoneIndex > 0)
        {
            _currentZoneIndex--;
            CurrentZone = _zones[_currentZoneIndex];

            scannedBarcodes.Clear();
            _scannedCount = 0;
            UpdateCounter();

            await DisplayAlert("Zone", $"Vorige zone: {CurrentZone}", "OK");
        }
        else
        {
            await DisplayAlert("Info", "Dit is de eerste zone", "OK");
        }
    }
}