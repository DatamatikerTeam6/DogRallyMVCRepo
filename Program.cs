using DogRallyMVC.Services;
using DogRallyMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DogRallyMVC.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DogRallyMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'DogRallyMVCContextConnection' not found.");

builder.Services.AddDbContext<DogRallyMVCContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<DogRallyMVCUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DogRallyMVCContext>();

builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IPostTrackToAPI, PostTrackToAPI>();
builder.Services.AddTransient<IGetExercisesFromAPI, GetExercisesFromAPI>();
builder.Services.AddTransient<IGetTrackFromAPI, GetTrackFromAPI>();
builder.Services.AddTransient<IGetUserTracksFromAPI, GetUserTracksFromAPI>();
builder.Services.AddTransient<IDeleteTrackFromAPI, DeleteTrackFromAPI>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IJWTTokenService, JWTTokenService>();


// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseRouting();

// Add session middleware 
app.UseSession(); 

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();