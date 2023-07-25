using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Abdulvosid.Durgerking.Services;

public class BotStartingBackgroundService : BackgroundService
{
    private readonly ILogger<BotStartingBackgroundService> logger;
    private readonly ITelegramBotClient botClient;
    private readonly IUpdateHandler updateHandler;

    public BotStartingBackgroundService(ILogger<BotStartingBackgroundService> logger,
                    ITelegramBotClient botClient,
                    IUpdateHandler updateHandler)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.updateHandler = updateHandler;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await botClient.GetMeAsync(stoppingToken);
        logger.LogInformation("Bot started: {username}", me.Username);

        botClient.StartReceiving(
            updateHandler: updateHandler,
            receiverOptions: new ReceiverOptions
            {
                ThrowPendingUpdates = true,
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                    UpdateType.CallbackQuery
                }
            },
            cancellationToken: stoppingToken);
    }
}