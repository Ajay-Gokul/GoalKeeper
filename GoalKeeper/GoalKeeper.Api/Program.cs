using GoalKeeper.BusinessLogic.Interface;
using GoalKeeper.BusinessLogic.ProcessControllers;
using GoalKeeper.Repository.Interface;
using GoalKeeper.Repository.Repository;
using Model.DTO;
using TokenService.Interface;
using TokenService.Service;

namespace GoalKeeper.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddControllers();

            // Register CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyCors",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // Register custom services
            builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();
            builder.Services.AddScoped<IAuthProcessController<UserDTO>, AuthProcessController>();

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

            // Use CORS policy
            app.UseCors("MyCors");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
