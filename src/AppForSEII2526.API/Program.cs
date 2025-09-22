using Microsoft.Data.Sqlite;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
//show definitions of enums as strings
.AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add service for managing a sqlserver database that will be managed using ApplicationDBContext
// the connection to the database was defined in appsettings

string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use");

// If we are using the Production Environment, then the AZURE DB should be used,
// otherwise the localdb or SQLite should be used
//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?source=recommendations&view=aspnetcore-7.0
switch (connection2Database) {
    case "SQLite":
        DbConnection _connection = new SqliteConnection("Filename=:memory:");
        //connection in case a persistent database is required
        //DbConnection _connection = new SqliteConnection("Data Source=Application.db;Cache=Shared");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break;

    case "AzureSQL":
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                       opt.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));

        break;
    default:
        //the localdb is used
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}

//Add Identity services to the container
builder.Services.AddAuthorization();
//Activate Identity APIs 
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1",
    new OpenApiInfo {
        Title = "AppForMovies.API",
        Version = "v1",
        Description = "This API provides services for renting and purchasing movies",
        License = new OpenApiLicense { Name = "MIT License", Url = new Uri("https://opensource.org/license/mit/") },
        Contact = new OpenApiContact { Name = "Software Engineering II Team", Email = "isii@on.uclm.es" },
    });
    //this assign operation names, as the actual names they have
    options.CustomOperationIds(apiDescription => {
        return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });

});


var app = builder.Build();




//Map Identity routes
//app.MapIdentityApi<IdentityUser>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope()) {
    try {

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //it creates the DB in case it does not exist
        //this is used only while developing the system
        if (connection2Database == "SQLite")
            db.Database.EnsureCreated();
        else
            db.Database.Migrate();


        //it sees the database
        //SeedData.Initialize(db, scope.ServiceProvider, logger);
    }
    catch (Exception ex) {
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        //this facilitates to generate unique ids for the operations
        c.DisplayOperationId();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Expose the implicitly defined Program class to the test project by doing:
public partial class Program { }