// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tiny.MultiTenant.Exceptions;
using Tiny.MultiTenant.Extensions;
using Tiny.MultiTenant.Interfaces;

namespace Tiny.MultiTenant.Services;

internal sealed class MultiTenantStore<TDbContext, TTenantEntity> : IMultiTenantStore<TTenantEntity>
    where TTenantEntity : class, ITenantInfo
    where TDbContext : IMultiTenantManagerDbContext<TTenantEntity>
{
    //TODO : 테넌트 DB 관리 서비스 구현, DBCreate, DbMigration, Active, Inactive
    private readonly ILogger _logger;
    private readonly IMultiTenantSettings _multiTenantSettings;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDistributedCache? _cache;
    private const string LogIdentifier = "MultiTenantStore";
    private const string TenantStoreCacheKey = "TenantInfo";

    private const string LoggingErrorMessageTemplate =
        "[{ModuleName}]: [{CallerMemberName}] Operation Error : {Message}";

    public MultiTenantStore(ILogger<MultiTenantStore<TDbContext, TTenantEntity>> logger,
        IMultiTenantSettings multiTenantSettings, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _multiTenantSettings = multiTenantSettings;
        _serviceProvider = serviceProvider;
        if (multiTenantSettings.UseDistributedCache)
            _cache = _serviceProvider.GetRequiredService<IDistributedCache>();
    }

    private async Task<IEnumerable<TTenantEntity>> GetTenantEntities(
        CancellationToken cancellationToken)
    {
        IList<TTenantEntity>? entities;

        if (_multiTenantSettings.UseDistributedCache && _cache != null)
        {
            entities =
                (await _cache.GetAsync<List<TTenantEntity>>(TenantStoreCacheKey,
                    cancellationToken));

            if (entities != null)
                return entities;

            entities = await GetTenantEntitiesFromDbAsync(cancellationToken);
            if (entities.Count > 0)
                await _cache.SetAsync(TenantStoreCacheKey, entities,
                    InternalOptions.DistributedCacheEntryOptions,
                    cancellationToken);
        }
        else
        {
            entities = await GetTenantEntitiesFromDbAsync(cancellationToken);
        }

        return entities.AsEnumerable();
    }

    private async Task<List<TTenantEntity>> GetTenantEntitiesFromDbAsync(
        CancellationToken cancellationToken = default)
    {
        using var scoped = CreateScopedDbContext(_serviceProvider);
        var list = await scoped.DbContext.TenantInfo.AsNoTracking().ToListAsync(cancellationToken);
        return list;
    }

    public async Task<IEnumerable<TTenantEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        //return Task.Run(() => DbContext.TenantInfo.AsNoTracking().AsEnumerable());
        return await GetTenantEntities(cancellationToken);
    }

    public async Task<TTenantEntity?> TryGetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        //return DbContext.TenantInfo.AsNoTracking().FirstOrDefaultAsync(tenant => tenant.Id == id, cancellationToken);
        return (await GetTenantEntities(cancellationToken)).FirstOrDefault(
            tenant => tenant.Id == id);
    }

    public async Task<bool> TryAddAsync(TTenantEntity tenantInfo,
        CancellationToken cancellationToken = default)
    {
        using var scoped = CreateScopedDbContext(_serviceProvider);
        await scoped.DbContext.TenantInfo.AddAsync(tenantInfo, cancellationToken);
        try
        {
            await scoped.DbContext.SaveChangesAsync(cancellationToken);

            if (_multiTenantSettings.UseDistributedCache && _cache != null)
                await _cache.RemoveAsync(TenantStoreCacheKey, cancellationToken);
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
            _logger.LogError(exception, LoggingErrorMessageTemplate, LogIdentifier, callerName,
                message);
    }

    public async Task<bool> TryChangeName(string id, string name,
        CancellationToken cancellationToken = default)
    {
        //TODO : 테스트필요
        using var scoped = CreateScopedDbContext(_serviceProvider);
        var tenant =
            await scoped.DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id,
                cancellationToken);
        if (tenant is null)
        {
            LoggingErrorWith($"Can not found Tenant(id:{id})");
            return false;
        }

        try
        {
            tenant.ChangeName(name);
            await scoped.DbContext.SaveChangesAsync(cancellationToken);

            if (_multiTenantSettings.UseDistributedCache && _cache != null)
                await _cache.RemoveAsync(TenantStoreCacheKey, cancellationToken);

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
        using var scoped = CreateScopedDbContext(_serviceProvider);
        var tenant =
            await scoped.DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id,
                cancellationToken);
        if (tenant is null)
        {
            LoggingErrorWith($"Can not found Tenant(id:{id})");
            return false;
        }

        try
        {
            tenant.ChangeConnectionString(connectionString);
            await scoped.DbContext.SaveChangesAsync(cancellationToken);

            if (_multiTenantSettings.UseDistributedCache && _cache != null)
                await _cache.RemoveAsync(TenantStoreCacheKey, cancellationToken);

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
        using var scoped = CreateScopedDbContext(_serviceProvider);
        var tenant =
            await scoped.DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id,
                cancellationToken);
        if (tenant is null)
            throw new TenantOperationException($"Can not found Tenant(id:{id})");

        if (tenant.IsActive)
            throw new TenantOperationException($"Already activated Tenant(id:{id})");

        await scoped.DbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            tenant.Active();
            //TODO : Active가 되면 DB생성 유무를 체크하여 DB를 생성하고 Migration 실행.

            await scoped.DbContext.CommitTransactionAsync(cancellationToken);

            if (_multiTenantSettings.UseDistributedCache && _cache != null)
                await _cache.RemoveAsync(TenantStoreCacheKey, cancellationToken);
        }
        catch (Exception ex)
        {
            scoped.DbContext.RollbackTransaction();
            throw new TenantOperationException("Tenant Active Failed.", ex);
        }
    }

    public async Task InactiveTenantAsync(string id, CancellationToken cancellationToken = default)
    {
        using var scoped = CreateScopedDbContext(_serviceProvider);
        var tenant =
            await scoped.DbContext.TenantInfo.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (tenant is null)
            throw new TenantOperationException($"Can not found Tenant(id:{id})");

        if (!tenant.IsActive)
            throw new TenantOperationException($"Already inactivated Tenant(id:{id})");
        try
        {
            tenant.Inactive();
            await scoped.DbContext.SaveChangesAsync(cancellationToken);

            if (_multiTenantSettings.UseDistributedCache && _cache != null)
                await _cache.RemoveAsync(TenantStoreCacheKey, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new TenantOperationException("Tenant Inactive Failed.", ex);
        }
    }

    // private static Task<T?> GetFromCacheAsync<T>(IDistributedCache cache, string key,
    //     CancellationToken cancellationToken = default) where T : class
    // {
    //     return cache.GetAsync<T>(key, cancellationToken);
    // }
    //
    // private static Task SetToCacheFromAsync<T>(IDistributedCache cache, string key, T value,
    //     CancellationToken cancellationToken = default) where T : class
    // {
    //     return cache.SetAsync(key, value, InternalOptions.DistributedCacheEntryOptions,
    //         cancellationToken);
    // }

    private static ScopedDbContext<TDbContext> CreateScopedDbContext(
        IServiceProvider serviceProvider) => new(serviceProvider);

    class ScopedDbContext<TScopedDbContext> : IDisposable where TScopedDbContext : TDbContext
    {
        private readonly IServiceScope _serviceScope;
        public TScopedDbContext DbContext { get; }

        public ScopedDbContext(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
            DbContext = _serviceScope.ServiceProvider.GetRequiredService<TScopedDbContext>();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
