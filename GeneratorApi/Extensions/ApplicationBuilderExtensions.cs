using GeneratorApi.Context;
using GeneratorApi.Extensions;
using GeneratorApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GeneratorApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHsts(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        Assert.NotNull(app, nameof(app));
        Assert.NotNull(env, nameof(env));

        if (!env.IsDevelopment())
            app.UseHsts();

        return app;
    }

    public static IApplicationBuilder IntializeDatabase(this IApplicationBuilder app)
    {
        Assert.NotNull(app, nameof(app));

        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

        dbContext!.Database.Migrate();

        
        return app;
    }
}
