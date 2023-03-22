using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Shared.DomainEntity;

public interface ISoftDeletable
{
    bool Deleted{ get; }

    DateTime? DaletedAt{ get; set; }
}
