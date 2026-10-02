using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Users;

public interface ICurrentUser
{
    public Guid? UserId { get; }
    public string? Username { get; }
}
