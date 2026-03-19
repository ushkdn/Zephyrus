using Zephyrus.Logger;

namespace Zephyrus.Identity.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = SerilogFactory.CreateLogger(builder.Services, builder.Configuration);
        
        var app = builder.Build();

        app.Run();
    }
}