using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace muhammadbilol.durgerking.Services;

public class UpdateHandler : IUpdateHandler
{
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}