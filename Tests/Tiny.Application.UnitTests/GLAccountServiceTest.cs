using Tiny.Application.DomainServices;
using Tiny.Application.UnitTests.Mocks;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;
using Tiny.Domain.Exceptions;
using UnitTest.Common;

namespace Tiny.Application.UnitTests;

public class GLAccountServiceTest
{
    [Fact]
    public void CheckValidPostableThrowError()
    {
        var repository = GLAccountRepositoryMock.Get().Object;
        var service = new GLAccountService(repository);
        var glAccount = GLAccountStore.CreateGLAccountWithId(1, "1000", "test", 0, 1);

        Assert.Throws<GLAccountValidationError>(() => service.CheckValidPostable(glAccount));
    }

    [Fact]
    public void CheckValidAccountingTypeThrowError()
    {
        var repository = GLAccountRepositoryMock.Get().Object;
        var service = new GLAccountService(repository);
        var glAccount = GLAccountStore.CreateGLAccountWithId(1, "1000", "test", 0, 0);

        Assert.Throws<GLAccountValidationError>(() => service.CheckValidAccountingType(glAccount));
    }

    [Fact]
    public async void CheckDuplicatedCodeAsyncThrowError()
    {
        var repository = GLAccountRepositoryMock.Get().Object;
        var service = new GLAccountService(repository);
        var firstGLAccount = await repository.GetOneAsync(new GLAccountByIdSpec(1), default);

        await Assert.ThrowsAsync<GLAccountValidationError>(() => service.CheckDuplicatedCodeAsync(firstGLAccount.Code, default));
    }

    [Fact]
    public async void CheckDuplicatedCodeExcludedIdAsyncThrowError()
    {
        var repository = GLAccountRepositoryMock.Get().Object;
        var service = new GLAccountService(repository);
        var firstGLAccount = await repository.GetOneAsync(new GLAccountByIdSpec(1), default);

        await Assert.ThrowsAsync<GLAccountValidationError>(() => service.CheckDuplicatedCodeAsync(firstGLAccount.Code, firstGLAccount.Id, default));
    }
}
