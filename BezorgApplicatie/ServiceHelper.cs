namespace BezorgApplicatie;

public static class ServiceHelper
{
    public static TService GetService<TService>()
        where TService : class
    {
        if (Application.Current?.Handler?.MauiContext?.Services is IServiceProvider services)
        {
            return services.GetService(typeof(TService)) as TService;
        }

        throw new InvalidOperationException("Unable to resolve service from MauiContext");
    }
}
