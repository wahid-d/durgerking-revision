using Abdulvosid.Durgerking.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Abdulvosid.Durgerking.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> logger;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private IAppDbContext dbContext;

    public UpdateHandler(ILogger<UpdateHandler> logger,
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
            "Update {updateType} received from {userId}",
            update.Type,
            update.Message?.From?.Id);

        using(var scope = serviceScopeFactory.CreateScope())
        {
            dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var user = await UpsertUserAsync(update, cancellationToken);
        }
    }

    private async Task<Entity.User> UpsertUserAsync(Update update, CancellationToken cancellationToken)
    {
        var tgUser = GetUserFromUpdate(update);
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == tgUser.Id, cancellationToken);

        if(user is null)
        {
            user = new Abdulvosid.Durgerking.Entity.User
            {
                Id = tgUser.Id,
                Fullname = $"{tgUser.FirstName} {tgUser.LastName}",
                Username = tgUser.Username,
                Language = tgUser.LanguageCode,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            dbContext.Users.Add(user);
            logger.LogInformation("user with Id {id} added.", tgUser.Id);
        }
        else
        {
            user.Fullname = $"{tgUser.FirstName} {tgUser.LastName}";
            user.Username = tgUser.Username;
            user.ModifiedAt = DateTime.UtcNow;
            logger.LogInformation("user with Id {id} updated.", tgUser.Id);
        }
        await dbContext.SaveChangesAsync();
        return user;
    }
    private static User GetUserFromUpdate(Update update)
        => update.Type switch
    {
        UpdateType.Message => update.Message.From,
        UpdateType.EditedMessage => update.Message.From,
        UpdateType.CallbackQuery => update.CallbackQuery.From,
        UpdateType.InlineQuery => update.InlineQuery.From,
        _ => throw new Exception("We dont supportas update type {update.type} yet")
    };
}
