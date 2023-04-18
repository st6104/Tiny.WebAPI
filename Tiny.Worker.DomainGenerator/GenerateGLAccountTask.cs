// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Http.Json;
using AutoBogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Tiny.Api.Controllers;
using Tiny.Application.Handlers.Commands;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Infrastructure;
using Tiny.Infrastructure.MultiTenant;
using Tiny.MultiTenant.Interfaces;
using Tiny.Shared.Extensions;

namespace Tiny.Worker.DomainGenerator;

public interface IGenerateGLAccountTask
{
    Task ExecuteAsync(string tenantId, int generateCount,
        CancellationToken cancellationToken = default);
}

public class GenerateGLAccountTask : IGenerateGLAccountTask
{
    private readonly ILogger<GenerateGLAccountTask> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;

    public GenerateGLAccountTask(ILogger<GenerateGLAccountTask> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
    }

    private async Task<int?> GetLastGLAccountCode(string tenantId)
    {
        using var scoped = _serviceProvider.CreateScope();
        var mutlTenantStore =
            scoped.ServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
        var tenantInfo = await mutlTenantStore.TryGetByIdAsync(tenantId);
        var multiTenantService = scoped.ServiceProvider.GetRequiredService<IMultiTenantService>();
        multiTenantService.Current = tenantInfo!;

        var dbContext = scoped.ServiceProvider.GetRequiredService<TinyDbContext>();

        var lastCode = await dbContext.GLAccount.OrderByDescending(x => x.Code).Take(1)
            .Select(x => x.Code)
            .FirstOrDefaultAsync();

        if (int.TryParse(lastCode, out var lastCodeAsNumber))
            return lastCodeAsNumber;
        else
            return null;
    }

    public async Task ExecuteAsync(string tenantId, int generateCount,
        CancellationToken cancellationToken = default)
    {
        var startCode = await GetLastGLAccountCode(tenantId) ?? 1000000000;
        var chunkCount = Math.Min(2000, generateCount);
        var lastGeneratedCode = startCode;
        var generatedGLAccountCount = 0;

        _httpClient.DefaultRequestHeaders.Add("X-Tenant-ID", tenantId);
        while (generatedGLAccountCount < generateCount)
        {
            lastGeneratedCode =
                await GenerateGLAccountNReturnLastCode(tenantId, lastGeneratedCode, chunkCount);
            generatedGLAccountCount += chunkCount;
        }
    }

    private async Task<int> GenerateGLAccountNReturnLastCode(string tenantId, int startCode,
        int generateCount, CancellationToken cancellationToken = default)
    {
        var lorem = new Lorem(locale: "ko");

        var fakes = new AutoFaker<GLAccount>()
            .StrictMode(true)
            .RuleFor(o => o.Code, f => (++startCode).ToString())
            .RuleFor(o => o.Name, f => lorem.Sentence(3))
            .RuleFor(o => o.PostableId, f => f.PickRandom<Postable>(Postable.List).Value)
            .RuleFor(o => o.AccountingTypeId,
                f => f.PickRandom<AccountingType>(AccountingType.List).Value)
            .Ignore(o => o.Id)
            .Ignore(o => o.AccountingType)
            .Ignore(o => o.Balance)
            .Ignore(o => o.Deleted);

        var glAccounts = fakes.Generate(generateCount);
        var lastGLAccountCode = -1;
        int.TryParse(glAccounts.Last().Code, out lastGLAccountCode);

        foreach (var glAccount in glAccounts.AsEnumerable())
        {
            var requestObject = new GLAccountAddCommand(glAccount.Code, glAccount.Name,
                glAccount.PostableId, glAccount.AccountingTypeId);
            
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5191/api/GLAccount", requestObject, cancellationToken);
            
            if(!response.IsSuccessStatusCode)
                Console.WriteLine(response.Content.ToString());
        }

        // using (var scoped = _serviceProvider.CreateScope())
        // {
            // var mutlTenantStore =
            //     scoped.ServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
            // var tenantInfo = await mutlTenantStore.TryGetByIdAsync(tenantId, cancellationToken);
            // var multiTenantService = scoped.ServiceProvider.GetRequiredService<IMultiTenantService>();
            // multiTenantService.Current = tenantInfo!;
            //
            // using (var dbContext = scoped.ServiceProvider.GetRequiredService<TinyDbContext>())
            // {
            //     await dbContext.GLAccount.AddRangeAsync(glAccounts, cancellationToken);
            //     await dbContext.SaveEntitiesAsync(cancellationToken);
            // }
        // }
        
        return lastGLAccountCode;
    }
}
