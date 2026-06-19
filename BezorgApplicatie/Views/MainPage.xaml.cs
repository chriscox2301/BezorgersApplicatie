using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views;

public partial class MainPage : ContentPage
{
    private readonly DataContext _context;

    public Shift? Shift { get; set; }
  
    public MainPage(DataContext context)
    {
        InitializeComponent();
        _context = context;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        

         Shift = await _context.Shifts
            .Include(s => s.Warehouse)
            .Include(s => s.Vehicle)
            .Include(s => s.Orders)
            .Where(s => s.StartTime >= DateTime.Now)
            .OrderBy(s => s.StartTime)
            .FirstOrDefaultAsync();

      


      

        OnPropertyChanged(nameof(Shift)); 
        

    }
}