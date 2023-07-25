using Telegram.Bot;
using Telegram.Bot.Polling;

public class BotBackroundService : BackgroundService
{
    private readonly ILogger<BotBackroundService> logger;
    private readonly IUpdateHandler updateHandler;
    private readonly ITelegramBotClient botClient;

    public BotBackroundService(
        ILogger<BotBackroundService> logger,
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
        logger.LogInformation("Bot {username} started receiving updates", bot.Username);

        botClient.StartReceiving
        (
            updateHandler : updateHandler,
            receiverOptions : default,
            cancellationToken : cancellationToken
        );
    }
}