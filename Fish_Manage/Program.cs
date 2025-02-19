using Fish_Manage;
using Fish_Manage.Models;
using Fish_Manage.Repository;
using Fish_Manage.Repository.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<FishManageContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
   .AddEntityFrameworkStores<FishManageContext>()
   .AddDefaultTokenProviders();
builder.Services.AddControllers(
    option =>
    {
        option.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
