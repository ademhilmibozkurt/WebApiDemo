var builder = WebApplication.CreateBuilder(args);

// add client for requests
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
