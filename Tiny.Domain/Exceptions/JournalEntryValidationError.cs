using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Shared.Exceptions;

namespace Tiny.Domain.Exceptions;

public sealed class JournalEntryValidationError : DomainValidationErrorException
{
    public override string DomainName => nameof(JournalEntry);

    public JournalEntryValidationError(string identifier, string message, Exception? innerException = null) : base(identifier, message, innerException)
    {
    }
}
