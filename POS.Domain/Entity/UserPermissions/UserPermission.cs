using POS.Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.UserPermissions;

public sealed class Permission : BaseEntity<Guid>
{
    public string Name { get; }

    public byte[]? RowVersion { get; }

    private Permission(string name)
    {
        Name = name;
    }
    private Permission()
    {
        
    }
    //factory method is used to create an instance of Permission to encapsulate
    public static Permission Create(string name)
        => new Permission(name);

}
