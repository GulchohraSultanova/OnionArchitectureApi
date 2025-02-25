
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnionArchitectureApi.Persistence.Contexts;

namespace OnionArchitectureApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<OnionArchitectureAppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            //         builder.Services.AddControllers()

            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductDtoValidation>())
            //.ConfigureApiBehaviorOptions(options =>
            //{
            //    options.InvalidModelStateResponseFactory = context =>
            //    {
            //        var errors = context.ModelState
            //            .Where(e => e.Value.Errors.Count > 0)
            //            .SelectMany(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
            //            .ToArray();

            //        var errorResponse = new
            //        {
            //            StatusCode = 400,
            //            Error = errors
            //        };

            //        return new BadRequestObjectResult(errorResponse);
            //    };
            //});
            //builder.Services.AddAutoMapper(typeof(ProductMapProfilies));
            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
        }
    }
}
