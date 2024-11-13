using CarStore.Data;
using CarStore.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("CarStore");
builder.Services.AddSqlite<CarStoreContext>(connString);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.MapCarsEndPoints();
app.MapCompanyEndpoints();
app.MapModelEndpoints();
app.MapColorEndpoints();
app.MapReviewEndpoints();
app.MapUserEndpoints();

await app.MigrateDbAsync();

app.Run();
