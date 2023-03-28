namespace Tiny.Domain.UnitTests;

public class GLAccountTest
{
    [Fact]
    public void MarkAsDelete()
    {
        var glAccount = new GLAccount("code1", "codename", 0, 0);
        glAccount.TryMarkAsDelete();

        Assert.False(glAccount.Deleted);
    }
}
