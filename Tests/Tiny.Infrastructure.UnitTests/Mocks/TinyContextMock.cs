using MediatR;
using Microsoft.Extensions.Logging;
using MockQueryable.FakeItEasy;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract.MultiTenant;
using UnitTest.Common;

namespace Tiny.Infrastructure.UnitTests.Mocks;

internal static class TinyContextMock
{
    private const string TestTenantId = "1000"; 
    
    public static Mock<TinyDbContext> Get()
    {
        var accounts = GLAccountStore.GetGLAccounts().AsQueryable().BuildMockDbSet();
        var mediator = GetMediatorMock().Object;
        var loggerFactory = GetLoggerFactoryMock().Object;
        var currTenantInfo = GetTenantInfoMock(TestTenantId);
        var multiTenantService = GetMultiTenantServiceMock(currTenantInfo.Object).Object;
        
        var dbContextMock = new Mock<TinyDbContext>(mediator, loggerFactory, multiTenantService);
        dbContextMock.Setup(x => x.AccountingType).Returns(AccountingType.List.AsQueryable().BuildMockDbSet());
        dbContextMock.Setup(x => x.Postable).Returns(Postable.List.AsQueryable().BuildMockDbSet());
        dbContextMock.Setup(x => x.JournalEntryStatus).Returns(JournalEntryStatus.List.AsQueryable().BuildMockDbSet());
        dbContextMock.Setup(x => x.GLAccount).Returns(accounts);

        dbContextMock.Object.GLAccount.Add(GLAccountStore.CreateGLAccountWithId(10, "1010", "계정과목10", 1, 1));

        return dbContextMock;
    }

    public static Mock<IMultiTenantService> GetMultiTenantServiceMock(ITenantInfo tenantInfo)
    {
        var mock = new Mock<IMultiTenantService>();
        mock.Setup(x => x.Current).Returns(tenantInfo);
        return mock;
    }
    
    public static Mock<ITenantInfo> GetTenantInfoMock(string id, string name ="", string connectionString = "")
    {
        var mock = new Mock<ITenantInfo>();
        mock.Setup(x => x.Id).Returns(id);
        mock.Setup(x => x.Name).Returns(name);
        mock.Setup(x => x.ConnectionString).Returns(connectionString);
        return mock;
    }

    public static Mock<IMediator> GetMediatorMock()
    {
        var mock = new Mock<IMediator>();
        return mock;
    }

    public static Mock<ILoggerFactory> GetLoggerFactoryMock()
    {
        var mock = new Mock<ILoggerFactory>();
        return mock;
    }
}
