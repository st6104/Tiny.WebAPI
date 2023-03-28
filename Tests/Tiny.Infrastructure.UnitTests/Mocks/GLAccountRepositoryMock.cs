using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Infrastructure.UnitTests.Mocks;

internal static class GLAccountRepositoryMock
{
    public static Mock<GLAccountRepository> Get()
    {
        var tinyContext = TinyContextMock.Get().Object;
        var mock = new Mock<GLAccountRepository>(tinyContext);
        return mock;
    }
}
