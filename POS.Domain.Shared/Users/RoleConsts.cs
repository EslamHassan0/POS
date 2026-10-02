using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.Users;

public static class RoleConsts
{
    public const string User = nameof(RoleType.User);
    public const string Admin = nameof(RoleType.Admin);
    public const string Cashier = nameof(RoleType.Cashier);
    public const string Accountant = nameof(RoleType.Accountant);
}
