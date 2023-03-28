// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny.Infrastructure.Abstract.Exceptions
{
    public sealed class TenantNotFoundException : Exception
    {
        public string TenantId { get; }

        public TenantNotFoundException(string tenantId, string? message = null, Exception? innerException = null) : base(message, innerException)
        {
            TenantId = tenantId;
        }
    }
}
