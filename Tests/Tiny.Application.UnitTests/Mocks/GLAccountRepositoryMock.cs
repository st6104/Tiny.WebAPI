using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;
using UnitTest.Common;

namespace Tiny.Application.UnitTests.Mocks;

internal static class GLAccountRepositoryMock
{
    public static Mock<IGLAccountRepository> Get()
    {
        var glAccounts = GLAccountStore.GetGLAccounts();
        var firstGLAccount = glAccounts.First();
        var mock = new Mock<IGLAccountRepository>();
        mock.Setup(x => x.GetOneAsync(It.IsAny<GLAccountByIdSpec>(), default)).ReturnsAsync(firstGLAccount);
        mock.Setup(x => x.GetAllAsync(default)).ReturnsAsync(glAccounts);
        mock.Setup(x => x.AddAsync(It.IsAny<GLAccount>(), default)).Callback(() => { });
        mock.Setup(x => x.UpdateAsync(It.IsAny<GLAccount>(), default)).Callback(() => { });
        mock.Setup(x => x.DeleteAsync(It.IsAny<long>(), default)).Callback(() => { });
        mock.Setup(x => x.IsSatisfiedByAsync(It.IsAny<GLAccountCodeExistSpec>(), default)).ReturnsAsync(() => true);

        return mock;
    }

}
