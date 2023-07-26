using Sirojiddin.Durgerking.Services; 
using Telegram.Bot; 
using Telegram.Bot.Polling; 
 
var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddTransient<IUpdateHandler, UpdateHandler>(); 
builder.Services.AddHostedService<BotBackroundService>(); 
builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>( 
    p => new TelegramBotClient(builder.Configuration.GetValue("BotApiKey",string.Empty)));

var app = builder.Build();

app.Run();
