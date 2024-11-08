using CarStore.Data;
using CarStore.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("CarStore");
builder.Services.AddSqlite<CarStoreContext>(connString);

var app = builder.Build();

app.MapCarsEndPoints();
app.MapCompanyEndpoints();
app.MapModelEndpoints();
app.MapColorEndpoints();
app.MapReviewEndpoints();

await app.MigrateDbAsync();

app.Run();
