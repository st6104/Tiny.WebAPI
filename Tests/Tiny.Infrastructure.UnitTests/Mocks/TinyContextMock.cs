using MediatR;
using Microsoft.Extensions.Logging;
using MockQueryable.FakeItEasy;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using UnitTest.Common;

namespace Tiny.Infrastructure.UnitTests.Mocks;

internal static class TinyContextMock
{
    public static Mock<TinyContext> Get()
    {
        var accounts = GLAccountStore.GetGLAccounts().AsQueryable().BuildMockDbSet();
        var mediator = GetMediatorMock().Object;
        var loggerFactory = GetLoggerFactoryMock().Object;
        
        //TODO : 생성자 파라메터 새로 구성해야함!
        var dbContextMock = new Mock<TinyContext>(mediator, loggerFactory);
        dbContextMock.Setup(x => x.AccountingType).Returns(AccountingType.List.AsQueryable().BuildMockDbSet());
        dbContextMock.Setup(x => x.Postable).Returns(Postable.List.AsQueryable().BuildMockDbSet());
        dbContextMock.Setup(x => x.JournalEntryStatus).Returns(JournalEntryStatus.List.AsQueryable().BuildMockDbSet());
        dbContextMock.Setup(x => x.GLAccount).Returns(accounts);

        dbContextMock.Object.GLAccount.Add(GLAccountStore.CreateGLAccountWithId(10, "1010", "계정과목10", 1, 1));

        return dbContextMock;
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
