using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;


namespace BezorgApplicatie.Views;

public partial class VehicleDamagePage : ContentPage
{
    private readonly DataContext _context;
    private Shift _currentShift;
    private Vehicle _replacementVehicle;

    public Shift CurrentShift
    {
        get => _currentShift;
        set
        {
            _currentShift = value;
            OnPropertyChanged();
        }
    }

    public VehicleDamagePage(DataContext context)
    {
        InitializeComponent();
        _context = context;
        BindingContext = this; 
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

       
        CurrentShift = await _context.Shifts
            .Include(s => s.Vehicle)
            .OrderBy(s => s.StartTime)
            .FirstOrDefaultAsync(s => s.StartTime >= DateTime.Now);

        OnPropertyChanged(nameof(CurrentShift));


     
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (_currentShift == null)
        {
            await DisplayAlert("Fout", "Geen shift gevonden", "OK");
            return;
        }

        if (pickerLocation.SelectedItem == null)
        {
            await DisplayAlert("Fout", "Selecteer locatie", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtDescription.Text))
        {
            await DisplayAlert("Fout", "Vul een beschrijving in", "OK");
            return;
        }

      
        var originalVehicleId = _currentShift.VehicleId;

        
        var vehicles = await _context.Vehicles
            .Where(v => v.Id != originalVehicleId)
            .ToListAsync();

        if (vehicles.Count == 0)
        {
            await DisplayAlert("Fout", "Geen andere voertuigen beschikbaar", "OK");
            return;
        }

        var random = new Random();
        _replacementVehicle = vehicles[random.Next(vehicles.Count)];

       
        _currentShift.VehicleId = _replacementVehicle.Id;
        _context.Shifts.Update(_currentShift);

        double latitude = 0;
        double longitude = 0;


        if (pickerLocation.SelectedItem?.ToString() == "Onderweg")
        {
            Location locationBus = await Geolocation.Default.GetLastKnownLocationAsync();
            latitude = locationBus.Latitude;
            longitude = locationBus.Longitude; 

        }


            var damage = new VehicleDamage
        {
            VehicleId = originalVehicleId,   
            ShiftId = _currentShift.Id,
            Description = txtDescription.Text,
            Location = pickerLocation.SelectedItem?.ToString(),
            Time = DateTime.Now,
            Latitude = latitude,
            Longitude = longitude
        };

        _context.VehicleDamages.Add(damage);

        await _context.SaveChangesAsync();

       
        string location = pickerLocation.SelectedItem?.ToString();

        string message = location switch
        {
            "Onderweg" => $"Bus {_replacementVehicle.Id} komt als vervanging",
            "Depot" => $" Pak bus {_replacementVehicle.Id} in het depot",
            _ => $"Bus {_replacementVehicle.Id} vervangen"
        };

        await DisplayAlert("Melding", message, "OK"); 

        //// Pushmeldingen: Deze is na het mergen kapot gegaan en hebben we ook niet meer werkend  gekregen. 
        //var request = new NotificationRequest
        //{
        //    NotificationId = 1337,
        //    Title = "Nieuwe Bus",
        //    Subtitle = "Bus Update",
        //    Description = message,

        //    Schedule = new NotificationRequestSchedule
        //    {
        //        NotifyTime = DateTime.Now.AddSeconds(5)
        ////    },



        //}; 
        //LocalNotificationCenter.Current.Show(request);

        txtDescription.Text = string.Empty;
        pickerLocation.SelectedItem = null;
 
        await Shell.Current.GoToAsync("//MainPage");
    }
}