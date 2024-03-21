using chattiz_back.Data;
using chattiz_back.Services;
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

        /* Añadiendo servicios */

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IChatRepository, ChatRepository>();

        
    }

    public void Configure(WebApplication app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }

};