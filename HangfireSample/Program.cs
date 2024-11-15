using Hangfire;
using Hangfire.SqlServer;
using HangfireSample.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
builder.Services.AddHangfire(config => config
        .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage("Server=DESKTOP-IQHGQHK;Database=Hangfire;User Id=sa;Password=#sa123;TrustServerCertificate=true;", new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5), //timeout for command batches that Hangfire will execute when interacting with the database
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero, //Hangfire checks for new jobs in the queues
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true,
           PrepareSchemaIfNecessary = true, //Hangfire will automatically create or update the necessary database schema if it does not exist
       }));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IMailService, MailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/dashboard");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
