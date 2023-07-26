using Microsoft.EntityFrameworkCore;
using Sirojiddin.Durgerking.Data;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Sirojiddin.Durgerking.Services;

public partial class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> logger;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private IAppDbContext dbContext;

    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        this.logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Polling error happened.");
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation(
        "Update {updateType} received  from {userId}.",
        update.Type,
        update.Message?.From?.Id);

        using (var scope = serviceScopeFactory.CreateScope())
        {
            dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var user = await UpsertUserAsync(update, cancellationToken);
        }
    }
    private async Task<Entity.User> UpsertUserAsync(Update update, CancellationToken cancellationToken)
    {
        var telegramUser = GetUserFromUpdate(update);
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == telegramUser.Id, cancellationToken);
        if (user is null)
        {
            user = new Entity.User
            {
                Id = telegramUser.Id,
                Fullname = $"{telegramUser.FirstName} {telegramUser.LastName}",
                Username = telegramUser.Username,
                Language = telegramUser.LanguageCode,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            dbContext.Users.Add(user);
            logger.LogInformation("New user with ID {id} added.", telegramUser.Id);
        }
        else
        {
            user.Fullname = $"{telegramUser.FirstName} {telegramUser.LastName}";
            user.Username = telegramUser.Username;
            user.ModifiedAt = DateTime.UtcNow;
            logger.LogInformation("user with ID {id} updated.", telegramUser.Id);
        }
        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    private static User GetUserFromUpdate(Update update)
        => update.Type switch
        {
            UpdateType.Message => update.Message.From,
            UpdateType.EditedMessage => update.EditedMessage.From,
            UpdateType.CallbackQuery => update.CallbackQuery.From,
            UpdateType.InlineQuery => update.InlineQuery.From,
            _ => throw new Exception("We dont support as  update type {update.Type} yet")
        };
}

