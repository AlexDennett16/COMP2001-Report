using COMP_2001_Report.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddDbContext<UserDbContext>(options => 
                      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Swagger should be ued in dev and prod
app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();

app.UseAuthorization();

app.Run();

