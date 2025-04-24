using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates;

public sealed record UserId(Guid Value) : BaseId<UserId>(Value), IBaseId<UserId> {
    public static UserId New() => new(Guid.NewGuid());
    public static UserId Create(Guid value) => new(value);
}