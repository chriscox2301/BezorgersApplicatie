using BezorgApplicatie.Data;
using PackageIssueModel = BezorgApplicatie.Models.PackageIssue;

namespace BezorgApplicatie.Views
{
    [QueryProperty(nameof(PakketId), "pakketId")]
    public partial class ProblemPage : ContentPage
    {
        private readonly DataContext _context;
        private BezorgApplicatie.Models.Package _pakket;

        public int PakketId { get; set; }

        public ProblemPage(DataContext context)
        {
            InitializeComponent();
            _context = context;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _pakket = await _context.Packages.FindAsync(PakketId) as BezorgApplicatie.Models.Package;
            if (_pakket != null)
                BarcodeLabel.Text = _pakket.Barcode;
        }

        private async void OnMeldenClicked(object sender, EventArgs e)
        {
            string issueType = null;
            if (RadioBeschadigd.IsChecked) issueType = "Beschadigd";
            else if (RadioVerkeerd.IsChecked) issueType = "Verkeerd pakket";
            else if (RadioMissend.IsChecked) issueType = "Missend pakket";

            if (issueType == null)
            {
                await DisplayAlert("Fout", "Kies een soort probleem.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(OmschrijvingEditor.Text))
            {
                await DisplayAlert("Fout", "Vul een omschrijving in.", "OK");
                return;
            }

            var issue = new PackageIssueModel
            {
                IssueType = issueType,
                Description = OmschrijvingEditor.Text.Trim(),
                Date = DateTime.Now,
                PackageId = _pakket.Id
            };

            _pakket.HasIssue = true;

            _context.PackageIssues.Add(issue);
            await _context.SaveChangesAsync();

            await DisplayAlert("Gelukt", "Het probleem is gemeld.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}