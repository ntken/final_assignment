using CarStoreUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("CarStoreClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5237"); // Thay thế với URL của API CarStore nếu cần
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddTransient<CarService>();

builder.Services.AddHttpClient<CarService>();

// builder.Services.AddHttpClient<CategoryService>()
//     .ConfigurePrimaryHttpMessageHandler(() =>
//     {
//         return new HttpClientHandler
//         {
//             ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//         };
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
