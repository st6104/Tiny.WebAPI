// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Moq;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace UnitTest.Common
{
    public static class JournalEntryStore
    {
        public static JournalEntry CreateJournalEntryWithId(int id, DateTime postingDate, long departmentId, string description)
        {
            var mock = new Mock<JournalEntry>(postingDate, departmentId, description);
            mock.SetupProperty(x => x.Id, id);
            return mock.Object;
        }
    }
}
