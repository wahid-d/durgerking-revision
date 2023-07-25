using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Sarvar.Durgerking.Services;

public class BotBackgroundService : BackgroundService
{
    private readonly ILogger<BotBackgroundService> logger;
    private readonly IUpdateHandler updateHandler;
    private readonly ITelegramBotClient botClient;

    public BotBackgroundService(
        ILogger<BotBackgroundService> logger,
        IUpdateHandler updateHandler,
        ITelegramBotClient botClient)
    {
        this.logger = logger;
        this.updateHandler = updateHandler;
        this.botClient = botClient;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bot = await botClient.GetMeAsync(cancellationToken);
        logger.LogInformation("Bot {username} started", bot.Username);

        botClient.StartReceiving(
            updateHandler: updateHandler,
            receiverOptions: default,
            cancellationToken: cancellationToken
        );
    }
}