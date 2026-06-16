using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BezorgApplicatie.Views;

public partial class AllStopsPage : ContentPage
{
    private readonly DataContext _dataContext;
    public ObservableCollection<Order> Stops { get; set; } = new ObservableCollection<Order>();
	public AllStopsPage(DataContext dataContext)
	{
		InitializeComponent();
        _dataContext = dataContext;
        BindingContext = this;
        LaadStops();
	}

    public async void LaadStops()
    {
        var stops = await _dataContext.Orders
                        .Include(s => s.Packages)
                        //.Where(s => s.Date == DateTime.Now)
                        .ToArrayAsync();

        Stops.Clear();

        foreach (var stop in stops)
        {
            Stops.Add(stop);
        }
    }

}