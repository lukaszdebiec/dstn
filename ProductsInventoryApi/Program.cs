using Microsoft.EntityFrameworkCore;
using ProductsInventoryApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ProductDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseCors("customPolicy");

app.MapControllers();

try
{
    DbSeeder.InitDb(app);
}
catch (Exception)
{
    Console.WriteLine("Something went wrong with seeding the database!");
}

app.Run();