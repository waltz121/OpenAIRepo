using ChatBot.Hubs;
using Microsoft.AspNetCore.Mvc.Routing;
using OpenAiCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddSignalR();

var app = builder.Build();

Config.Init(builder.Configuration["OpenAiApiKey"], 
    @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", 
    builder.Configuration["PineConeApiKey"], 
    builder.Configuration["SQLConnectionString"], 
    builder.Configuration["PineConeBaseUrl"], 
    builder.Configuration["PineConeNamespace"]);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<BatchUploadHub>("/batchuploadhub");

app.Run();
