using Telegram.Bot;
using Telegram.Bot.Polling;

namespace muhammadbilol.durgerking.Services;

public class BotBackgrounService : BackgroundService
{
    private readonly ILogger<BotBackgrounService> logger;
    private readonly IUpdateHandler updateHandler;
    private readonly ITelegramBotClient botClient;

    public BotBackgrounService(
    ILogger<BotBackgrounService> logger,
    IUpdateHandler updateHandler,
    ITelegramBotClient botClient)
    {
        this.logger = logger;
        this.updateHandler = updateHandler;
        this.botClient = botClient;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var bot = await botClient.GetMeAsync(cancellationToken);
            logger.LogInformation("Bot {username} started polling update", bot.Username);

            botClient.StartReceiving(
                updateHandler: updateHandler,
                receiverOptions: default,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to connect bot server");
        }
    }
}