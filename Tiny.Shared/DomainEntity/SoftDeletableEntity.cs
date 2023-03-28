// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Shared.DomainEntity;
public abstract class SoftDeletableEntity : Entity, ISoftDeletable
{
    public bool Deleted { get; protected set; }

    public virtual bool TryMarkAsDelete()
    {
        if (IsTransient() || Deleted) return false;

        Deleted = true;
        return true;
    }
}
