namespace BezorgApplicatie
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.ProbleemMeldenPage), typeof(Views.ProbleemMeldenPage));
        }
    }
}
