// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Tiny.Infrastructure.Abstract.Exceptions;

namespace Tiny.Infrastructure.Abstract.MultiTenant;

public abstract class MultiTenantStore<T> : IMultiTenantStore<T> where T : class, ITenantInfo
{
    //TODO : 테넌트 DB 관리 서비스 구현, DBCreate, DbMigration, Active, Inactive

    private readonly ILogger _logger;
    private const string LogIdentifier = "MultiTenantStore";

    private const string LoggingErrorMessageTemplate =
        "[{ModuleName}]: [{CallerMemberName}] Operation Error : {Message}";

    protected IMultiTenantManagerDbContext<T> DbContext { get; }

    protected MultiTenantStore(ILogger logger, IMultiTenantManagerDbContext<T> dbContext)
    {
        _logger = logger;
        DbContext = dbContext;
    }

    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() => DbContext.TenantInfo.AsNoTracking().AsEnumerable());
    }

    public Task<T?> TryGetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return DbContext.TenantInfo.AsNoTracking().FirstOrDefaultAsync(tenant => tenant.Id == id, cancellationToken);
    }

    public async Task<bool> TryAddAsync(T tenantInfo, CancellationToken cancellationToken = default)
    {
        await DbContext.TenantInfo.AddAsync(tenantInfo);
        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception exception)
        {
            LoggingErrorWith(exception.Message, exception);
            return false;
        }
    }

    private void LoggingErrorWith(string message, Exception? exception = null,
        [CallerMemberName] string callerName = "")
    {
        //TODO : 로깅 방법.. callerName 제대로 들어오는지 확인 필요
        if (exception is null)
            _logger.LogError(LoggingErrorMessageTemplate, LogIdentifier, callerName, message);
        else
            _logger.LogError(exception, LoggingErrorMessageTemplate, LogIdentifier, callerName, message);
    }

    public async Task<bool> TryChangeName(string id, string name, CancellationToken cancellationToken = default)
    {
        //TODO : 테스트필요
        var tenant = await DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id);
        if (tenant is null)
        {
            LoggingErrorWith($"Can not found Tenant(id:{id})");
            return false;
        }

        try
        {
            tenant.ChangeName(name);
            await DbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception exception)
        {
            LoggingErrorWith(exception.Message, exception);
            return false;
        }
    }

    public async Task<bool> TryChangeConnectionString(string id, string connectionString,
        CancellationToken cancellationToken = default)
    {
        //TODO : 테스트필요
        var tenant = await DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id);
        if (tenant is null)
        {
            LoggingErrorWith($"Can not found Tenant(id:{id})");
            return false;
        }

        try
        {
            tenant.ChangeConnectionString(connectionString);
            await DbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception exception)
        {
            LoggingErrorWith(exception.Message, exception);
            return false;
        }
    }

    public async Task ActiveTenantAsync(string id, CancellationToken cancellationToken = default)
    {
        var tenant = await DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id);
        if (tenant is null)
            throw new TenantOperationException($"Can not found Tenant(id:{id})");

        if (tenant.IsActive)
            throw new TenantOperationException($"Already activated Tenant(id:{id})");

        await DbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            tenant.Active();
            //TODO : Active가 되면 DB생성 유무를 체크하여 DB를 생성하고 Migration 실행.

            await DbContext.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            DbContext.RollbackTransaction();
            throw new TenantOperationException("Tenant Active Failed.", ex);
        }
    }

    public async Task InactiveTenantAsync(string id, CancellationToken cancellationToken = default)
    {
        var tenant = await DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id);
        if (tenant is null)
            throw new TenantOperationException($"Can not found Tenant(id:{id})");

        if (!tenant.IsActive)
            throw new TenantOperationException($"Already inactivated Tenant(id:{id})");
        try
        {
            tenant.Inactive();
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new TenantOperationException("Tenant Inactive Failed.", ex);
        }
    }
}
