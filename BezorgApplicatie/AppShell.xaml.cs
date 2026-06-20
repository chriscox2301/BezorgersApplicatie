namespace BezorgApplicatie
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.ProblemPage), typeof(Views.ProblemPage));
        }
    }
}
