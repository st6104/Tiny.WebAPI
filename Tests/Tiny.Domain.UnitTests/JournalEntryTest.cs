// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Domain.Exceptions;
using UnitTest.Common;

namespace Tiny.Domain.UnitTests
{
    public class JournalEntryTest
    {
        [Fact]
        public void NewJournalEntryStatusIsApplied()
        {
            var postingDate = DateTime.Now;
            var journalEntry = new JournalEntry(postingDate, 0, "");
            
            Assert.True(journalEntry.JournalEntryStatusId == JournalEntryStatus.Applied);
        }

        [Fact]
        public void NewJournalEntryStatusToApprovedFailed()
        {
            var postingDate = DateTime.Now;
            var journalEntry = new JournalEntry(postingDate, 0, "");

            Assert.Throws<JournalEntryValidationError>(() => journalEntry.ChangeJournalStatusToApproved());
        }

        [Fact]
        public void NewJournalEntryStatusToRejectFailed()
        {
            var postingDate = DateTime.Now;
            var journalEntry = new JournalEntry(postingDate, 0, "");

            Assert.Throws<JournalEntryValidationError>(() => journalEntry.ChangeJournalStatusToRejected());
        }

        [Fact]
        public void ExistingJournalEntryStatusApprovedToApprovedFail()
        {
            var postingDate = DateTime.Now;
            var journalEntry = JournalEntryStore.CreateJournalEntryWithId(1, postingDate, 1, "");
            journalEntry.ChangeJournalStatusToApproved();

            Assert.Throws<JournalEntryValidationError>(() => journalEntry.ChangeJournalStatusToApproved());
        }

        [Fact]
        public void ExistingJournalEntryStatusApprovedToRejectedFail()
        {
            var postingDate = DateTime.Now;
            var journalEntry = JournalEntryStore.CreateJournalEntryWithId(1, postingDate, 1, "");
            journalEntry.ChangeJournalStatusToApproved();

            Assert.Throws<JournalEntryValidationError>(() => journalEntry.ChangeJournalStatusToRejected());
        }


        [Fact]
        public void ExistingJournalEntryChangeDepartmentFailed()
        {
            var postingDate = DateTime.Now;
            var journalEntry = JournalEntryStore.CreateJournalEntryWithId(1, postingDate, 1, "");
            journalEntry.ChangeJournalStatusToApproved();

            Assert.Throws<JournalEntryValidationError>(() => journalEntry.ChangeDepartment(0));
        }
    }
}
