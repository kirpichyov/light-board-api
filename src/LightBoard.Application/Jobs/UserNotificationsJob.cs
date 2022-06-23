using LightBoard.Application.Abstractions.Services;
using LightBoard.DataAccess.Abstractions;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LightBoard.Application.Jobs;

public class UserNotificationsJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserNotificationsJob> _logger;

    public UserNotificationsJob(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<UserNotificationsJob> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // TODO: Implement me to the send the notifications. After successful send - delete notifications.
        _logger.LogInformation("Job is fired."); // <-- Remove me.
    }
}