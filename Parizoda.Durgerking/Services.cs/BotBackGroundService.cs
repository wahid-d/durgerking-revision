using Telegram.Bot;
using Telegram.Bot.Polling;

namespace parizoda.durgerking.Services;

public class BotBackGroundService : BackgroundService
{
    private readonly ILogger<BotBackGroundService> logger;
    private readonly ITelegramBotClient botClient;
    private readonly IUpdateHandler updateHandler;

    public BotBackGroundService(
        ILogger<BotBackGroundService> logger,
        ITelegramBotClient botClient,
        IUpdateHandler updateHandler)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.updateHandler = updateHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var telegrambot = await botClient.GetMeAsync(cancellationToken);
            logger.LogInformation("Bot {username} started pooling update", telegrambot.Username);

            botClient.StartReceiving(
                updateHandler: updateHandler,
                receiverOptions: default,
                cancellationToken: cancellationToken
            );
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error occured, failed to connect server");
        }
    }
}