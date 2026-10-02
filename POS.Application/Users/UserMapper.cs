using AutoMapper;
using POS.Application.Contract.Users;
using POS.Domain.Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Users;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UsersWithRoles, UsersWithRolesDto>()
                                         .ReverseMap();
    }
}
