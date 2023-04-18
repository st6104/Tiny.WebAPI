using Microsoft.Extensions.Logging;
using Tiny.Application.Abstract.Handlers;
using Tiny.Shared.Extensions;

namespace Tiny.Infrastructure.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandRequest<TResponse>
{
    private readonly TinyDbContext _dbDbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(TinyDbContext dbDbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _dbDbContext = dbDbContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = default(TResponse);

        var typeName = request.GetGenericTypeName();

        try
        {
            if (_dbDbContext.HasActiveTransaction)
            {
                return await next();
            }

            var stratagy = _dbDbContext.Database.CreateExecutionStrategy();

            await stratagy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbDbContext.BeginTransactionAsync();
                var transactionId = transaction.TransactionId;
                _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})",
                    transactionId, typeName, request);

                response = await next();

                await _dbDbContext.CommitTransactionAsync(transaction);
                _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}",
                    transactionId, typeName);
            });

            return response!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
            throw;
        }
    }
}
