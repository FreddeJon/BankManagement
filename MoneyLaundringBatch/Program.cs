using Persistence;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        {
            var configuration = services.ConfigureMoneyLaunderingBatch();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DbConnection"), contextBuilder => contextBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });
        }
    ).Build();