using PlaylistService.Database;

namespace PlaylistService;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddSingleton<ISongsRepository, SongsRepository>();
        services.AddSingleton<IPlaylistManager, PlaylistManager>();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<Services.PlaylistService>();
        });
    }
}