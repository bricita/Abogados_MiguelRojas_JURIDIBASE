using Abogados_MiguelRojas_JURIDIBASE.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
});
//Sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);//segundos
    options.Cookie.HttpOnly = true;//almacenar informacion temporal(rol, nombre, zonas calientes(mas interaccion) de una pagina)
    options.Cookie.IsEssential = true;//es obligatorio
});
//Autenticaciones(rol) y añadir cookie con las siguientes opciones
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.LoginPath = "/Login/Login";
    options.AccessDeniedPath = "/Home/AccesoDenegado";
});
//Autorizaciones
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

///////////////////////////////////siempre debe tener ese orden

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

///////////////////////////////////

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
