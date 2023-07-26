using Microsoft.EntityFrameworkCore;
using parizoda.durgerking.Data;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace parizoda.durgerking.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<IUpdateHandler> logger;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private IAppDbContext dbContext;

    public UpdateHandler(
        ILogger<IUpdateHandler> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        this.logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
    }
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Pooling error happened");
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Update {updatetype} recieved from {user.Id}",
            update.Type,
            update.Message?.From?.Id);

        using (var scope =serviceScopeFactory.CreateScope())
        {
            dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var user = await UpsertUserAsync(update, cancellationToken);
        }
    }

    private async Task<Entity.User> UpsertUserAsync(Update update, CancellationToken cancellationToken)
    {
        var telegramuser = GetUserFromUpdate(update);
        var user = await dbContext.Users
            .FirstOrDefaultAsync(p => p.Id == telegramuser.Id, cancellationToken);

        if(user is null)
        {
            user = new parizoda.durgerking.Entity.User
            {
                Id = telegramuser.Id,
                Fullname = $"{telegramuser.FirstName} {telegramuser.LastName}",
                Username = telegramuser.Username,
                Language = telegramuser.LanguageCode,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            dbContext.Users.Add(user);
            logger.LogInformation("New user with Id {id} added", telegramuser.Id);
        }
        else
        {
            user.Fullname = $"{telegramuser.FirstName} {telegramuser.LastName}";
            user.Username = telegramuser.Username;
            user.ModifiedAt = DateTime.Now;
            logger.LogInformation("user with ID {id} updated", telegramuser.Id);
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
            _ => throw new Exception("We do not support this update type {update.Type} yet")
        };
}