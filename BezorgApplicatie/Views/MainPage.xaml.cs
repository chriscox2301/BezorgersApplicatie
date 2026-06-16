using BezorgApplicatie.Data;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views;

public partial class MainPage : ContentPage
{
    private readonly DataContext _context;

    public MainPage(DataContext context)
    {
        InitializeComponent();
        _context = context;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var shift = await _context.Shifts
            .Include(s => s.Warehouse)
            .Include(s => s.Vehicle)
            .Include(s => s.Orders)
            .Where(s => s.StartTime >= DateTime.Now)
            .OrderBy(s => s.StartTime)
            .FirstOrDefaultAsync();

        if (shift == null)
        {
            lblDate.Text = "Geen dienst gevonden";
            lblStart.Text = "";
            lblEnd.Text = "";
            lblWarehouse.Text = "";
            lblVehicle.Text = "";
            lblOrders.Text = "";
            return;
        }

        var orderCount = shift.Orders?.Count() ?? 0;

        lblDate.Text = $"Datum: {shift.StartTime:dd-MM-yyyy}";
        lblStart.Text = $"Starttijd: {shift.StartTime:HH:mm}";
        lblEnd.Text = $"Eindtijd: {shift.EndTime:HH:mm}";
        lblWarehouse.Text = $"Warehouse: {shift.Warehouse.Location}";
        lblVehicle.Text = $"Busnummer: {shift.Vehicle.Id}";
        lblOrders.Text = $"Aantal stops: {orderCount}";
    }
}