using System;
using Microsoft.EntityFrameworkCore;
using InterportCargo.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC/Razor
builder.Services.AddRazorPages();

// EF Core (SQLite). Uses appsettings.json if present, else falls back.
builder.Services.AddDbContext<InterportContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default") ?? "Data Source=interport.db"
    )
);

// Session (required for login state)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// For @inject IHttpContextAccessor in _Layout
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapRazorPages();

if (!args.Contains("--testhost", StringComparer.OrdinalIgnoreCase))
{
    app.Run();
}
