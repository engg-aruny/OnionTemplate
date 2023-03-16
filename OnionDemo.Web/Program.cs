using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnionDemo.Persistance;
using OnionDemo.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("pingongDB");

// Add services to the container.
builder.Services.AddControllers()
               .AddApplicationPart(typeof(OnionDemo.Presentation.AssemblyReference).Assembly);

builder.Services.AddSwaggerGen(c =>
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }));

builder.Services.AddDbContextPool<RepositoryDbContext>(option =>
{
    option.UseSqlServer(connectionString);
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Call ConfigureContainer on the Host sub property 
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new PersistanceAutofacModule());
    builder.RegisterModule(new PresentationAutofacModule());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();