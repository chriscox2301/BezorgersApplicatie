using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views;
public partial class RouteMap : ContentPage
{
    private readonly DataContext _dataContext;
    public Order Order { get; set; }
    public Shift Shift { get; set; }

    public RouteMap(DataContext dataContext)
	{
		InitializeComponent();
        _dataContext = dataContext;
        BindingContext = this;
        Order = new Order();
        Shift = new Shift();
        InitPage();
    }

    private async void InitPage()
    {
        await LoadShift();
        LoadOrder();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadShift();
        LoadOrder();
    }

    private async Task LoadShift()
    {
        //Placeholder -> Takes the first Order in the db.
        try
        {
            await _dataContext.Database.EnsureCreatedAsync();
            Shift = await _dataContext.Shifts.FirstAsync();
            OnPropertyChanged(nameof(Shift));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Er ging iets fout bij het laden van de shift: {ex.Message}", "OK");
        }
    }

    private void LoadOrder()
    {
        Order = Shift.Orders.Where(o => o.Status == "Onderweg").First();
        OnPropertyChanged(nameof(Order));
    }

    private async void DeliverButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeliveryPage(_dataContext, Order, Shift));
    }
}