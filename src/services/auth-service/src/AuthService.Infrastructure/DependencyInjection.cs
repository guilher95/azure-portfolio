using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuthService.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

        services.AddDbContext<AuthDbContext>(opts =>
            opts.UseSqlServer(cs, sql =>
            {
                sql.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName);
            }));

        return services;
    }
}
