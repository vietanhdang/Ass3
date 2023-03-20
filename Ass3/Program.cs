using Ass3.Hubs;
using Ass3.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddRazorPages(options => {
//    options.RootDirectory = "/Pages";
//});
builder.Services.AddSignalR();

builder.Services.AddDbContext<PostDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString")));
builder.Services.AddScoped<PostDbContext>();
builder.Services.AddTransient<SignalRServer>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapHub<SignalRServer>("/signalRServer");
app.MapRazorPages();
app.Run();
