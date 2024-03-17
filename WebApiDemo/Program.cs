using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiDemo.Data;
using WebApiDemo.Interfaces;
using WebApiDemo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// jwt implementation 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "http://localhost",
        ValidAudience = "http://localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Carrotcarrotcarrot0.")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<ProductContext>(opt =>
{
    opt.UseSqlServer("server = (localdb)\\mssqllocaldb; database = WebApiDb; integrated security = true;");
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// add cors to builder. allow all domain requests
// if you want specific domain can reach add opt.WithOrigins(string[])
builder.Services.AddCors(cors => 
{
    cors.AddPolicy("WebApiCorsPolicy", opt => { opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Reference Loop Handling
builder.Services.AddControllers().AddNewtonsoftJson(opt => 
{
    // for preventing json's not comeback problem(loop)
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();

// CORS - cross origin requests
app.UseCors("WebApiCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
