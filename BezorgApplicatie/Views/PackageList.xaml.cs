using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views
{
    public partial class PackageListPage : ContentPage
    {
        private readonly DataContext _context;
        private List<Package> _allPackages;

        public PackageListPage(DataContext context)
        {
            InitializeComponent();
            _context = context;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _allPackages = await _context.Packages.ToListAsync();
            PackageList.ItemsSource = _allPackages;
        }

        private void OnZoekenClicked(object sender, EventArgs e)
        {
            var zoekterm = BarcodeEntry.Text?.Trim();
            if (string.IsNullOrEmpty(zoekterm))
            {
                PackageList.ItemsSource = _allPackages;
                return;
            }

            var gefilterd = _allPackages
                .Where(p => p.Barcode != null && p.Barcode.Contains(zoekterm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            PackageList.ItemsSource = gefilterd;
        }

        private async void OnPakketGekozen(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Package gekozenPakket)
                return;

            PackageList.SelectedItem = null;

            await Shell.Current.GoToAsync($"{nameof(ProbleemMeldenPage)}?pakketId={gekozenPakket.Id}");
        }
    }
}