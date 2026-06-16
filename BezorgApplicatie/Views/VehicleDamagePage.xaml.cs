using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views;

public partial class VehicleDamagePage : ContentPage
{
    private readonly DataContext _context;
    private Shift _currentShift;
    private Vehicle _replacementVehicle;

    public VehicleDamagePage(DataContext context)
    {
        InitializeComponent();
        _context = context;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

       
        _currentShift = await _context.Shifts
            .Include(s => s.Vehicle)
            .OrderBy(s => s.StartTime)
            .FirstOrDefaultAsync(s => s.StartTime >= DateTime.Now);

        if (_currentShift == null)
        {
            lblBus.Text = "Geen actieve shift gevonden";
            return;
        }

        
        lblBus.Text = $"Huidige bus: {_currentShift.Vehicle.Id}";
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

    
        var damage = new VehicleDamage
        {
            VehicleId = originalVehicleId,   
            ShiftId = _currentShift.Id,
            Description = txtDescription.Text,
            Location = pickerLocation.SelectedItem?.ToString(),
            Time = DateTime.Now
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

        txtDescription.Text = string.Empty;
        pickerLocation.SelectedItem = null;
 
        await Shell.Current.GoToAsync("//MainPage");
    }
}