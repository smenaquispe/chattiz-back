using System.Net.WebSockets;
using chattiz_back.Data;
using chattiz_back.Services;
using chattiz_back.Utils;
using Microsoft.EntityFrameworkCore;


public class Startup {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services) {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        /* MySQL */
        var configurationString = _configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine(configurationString);
        
        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseMySql(configurationString, ServerVersion.AutoDetect(configurationString));
        });

        /* CORS */
        services.AddCors(options => {
            options.AddPolicy("AllowAllOrigins", builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });

        /* Añadiendo servicios */

        services.AddScoped<IUserRepository, UserService>();
        services.AddScoped<IMessageRepository, MessageService>();
        services.AddScoped<IChatRepository, ChatService>();
        
    }

    public void Configure(WebApplication app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        /* CORS */
        app.UseCors("AllowAllOrigins");

        /* Web Sockets */
        app.UseWebSockets();
        app.UseWebSocketHandler();
    }

};