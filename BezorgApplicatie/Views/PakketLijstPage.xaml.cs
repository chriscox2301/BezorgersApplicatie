using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;

namespace BezorgApplicatie.Views
{
    public partial class PakketLijstPage : ContentPage
    {
        private readonly DataContext _context;
        private List<Package> _allePakketten;

        public PakketLijstPage(DataContext context)
        {
            InitializeComponent();
            _context = context;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _allePakketten = await _context.Packages.ToListAsync();
            PakkettenLijst.ItemsSource = _allePakketten;
        }

        private void OnZoekenClicked(object sender, EventArgs e)
        {
            var zoekterm = BarcodeEntry.Text?.Trim();
            if (string.IsNullOrEmpty(zoekterm))
            {
                PakkettenLijst.ItemsSource = _allePakketten;
                return;
            }

            var gefilterd = _allePakketten
                .Where(p => p.Barcode != null && p.Barcode.Contains(zoekterm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            PakkettenLijst.ItemsSource = gefilterd;
        }

        private async void OnPakketGekozen(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Package gekozenPakket)
                return;

            PakkettenLijst.SelectedItem = null;

            await Shell.Current.GoToAsync($"{nameof(ProbleemMeldenPage)}?pakketId={gekozenPakket.Id}");
        }
    }
}
