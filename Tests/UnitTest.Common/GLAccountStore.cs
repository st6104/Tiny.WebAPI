using Moq;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace UnitTest.Common
{
    public static class GLAccountStore
    {
        public static List<GLAccount> GetGLAccounts()
        {
            return new List<GLAccount>
            {
                CreateGLAccountWithId(1, "1001", "계정과목01", 1, 1),
                CreateGLAccountWithId(2, "1002", "계정과목02", 1, 1),
                CreateGLAccountWithId(3, "1003", "계정과목03", 1, 1),
                CreateGLAccountWithId(4, "1004", "계정과목04", 1, 1),
                CreateGLAccountWithId(5, "1005", "계정과목05", 1, 1),
                CreateGLAccountWithId(6, "1006", "계정과목06", 1, 1),
                CreateGLAccountWithId(7, "1007", "계정과목07", 1, 1),
                CreateGLAccountWithId(8, "1008", "계정과목08", 1, 1),
                CreateGLAccountWithId(9, "1009", "계정과목09", 1, 1),
                CreateGLAccountWithId(10, "1010", "계정과목10", 1, 1)
            };
        }

        public static GLAccount CreateGLAccountWithId(int id, string code, string name, int postableId, int accountingTypeId)
        {
            var mock = new Mock<GLAccount>(code, name, postableId, accountingTypeId);
            mock.SetupProperty(x => x.Id, id);

            return mock.Object;
        }
    }
}
