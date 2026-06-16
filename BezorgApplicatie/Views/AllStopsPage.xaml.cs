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
        _dataContext = dataContext;
        BindingContext = this;
        LaadStops();
        OrderStatus = "Onderweg";
        Filter();
        
	}

    public async void LaadStops()
    {
        var stops = await _dataContext.Orders
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

}