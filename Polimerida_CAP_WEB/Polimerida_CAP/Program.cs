using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Helpers;
using Polimerida_CAP.Services;
using Polimerida_CAP.Services.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos MySQL usando la cadena de conexión de appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configure Device Settings
builder.Services.Configure<DeviceSettings>(builder.Configuration.GetSection("DeviceSettings"));

// Register HttpClient
builder.Services.AddHttpClient<IFileApiService, FileApiService>();

//add configuration for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de espera de la sesión
    options.Cookie.HttpOnly = true; // La cookie de sesión no es accesible desde JavaScript
    options.Cookie.IsEssential = true; // La cookie es esencial para el funcionamiento de la aplicación
});


// Configuración de la inyección de dependencias para los servicios personalizados
builder.Services.AddScoped<IAuthService, AuthService>();
//add IturnosServices scope
builder.Services.AddScoped<IturnosServices, TurnosServices>();
//add IDepartamentoServices scope
builder.Services.AddScoped<IDepartamentoServices, DepartamentoServices>();
//add IGrupoServices scope
builder.Services.AddScoped<IGrupoServices, GrupoServices>();
//add IPuestoServices scope
builder.Services.AddScoped<IPuestoServices, PuestoServices>();
//add FtpUploader scope
builder.Services.AddScoped<FtpUploader>();
//add IEmpleadoServices scope
builder.Services.AddScoped<IEmpleadoServices, EmpleadoServices>();
//add IDispositivoServices scope
builder.Services.AddScoped<IDispositivoServices, DispositivoServices>();
//add ISubgrupoServices scope
builder.Services.AddScoped<ISubgrupoServices, SubgrupoServices>();
//add IIncidenciaServices scope
builder.Services.AddScoped<IIncidenciaServices, IncidenciaServices>();
//add IIncidenciaEmpleadoServices scope
builder.Services.AddScoped<IIncidenciaEmpleadoServices, IncidenciaEmpleadoServices>();
//add IFestivoServices scope
builder.Services.AddScoped<IFestivoServices, FestivoServices>();
//add FileApiService scope
builder.Services.AddScoped<IFileApiService, FileApiService>();
// Configuracin de JwtHelper para la generacin de tokens JWT 

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Configurar límites de tamaño de archivos
    options.MaxModelBindingCollectionSize = 1;
});

// Configurar límites de tamaño de archivos
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024; // 200KB
    options.ValueLengthLimit = 200 * 1024; // 200KB
    options.MemoryBufferThreshold = 200 * 1024; // 200KB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Configurar límites de tamaño de solicitudes
app.Use(async (context, next) =>
{
    context.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 200 * 1024; // 200KB
    await next();
});

app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseCors("AllowAll"); // Aplica la política de CORS

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
