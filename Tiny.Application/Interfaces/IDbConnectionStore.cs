using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Application.Interfaces;

public interface IDbConnectionStore
{
    string Default { get; }
}
