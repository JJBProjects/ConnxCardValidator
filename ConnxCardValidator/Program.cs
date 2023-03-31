using ConnxCardValidator.Models;
using ConnxCardValidator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddScoped<ICardTypeDetailsRepository, MockCardTypeDetailsRepository>();
builder.Services.AddScoped<ICardNumberValidator, CardNumberValidator>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endPoints =>
{
    endPoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=CardValidation}/{action=Index}");
    endPoints.MapRazorPages();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();
