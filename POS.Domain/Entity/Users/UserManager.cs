using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using POS.Domain.Identity;
using POS.Domain.Shared.Results;

namespace POS.Domain.Entity.Users
{
    public class UserManager : DomainServices
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityApplicationUser> _userManager;
        public UserManager(IUserRepository userRepository, 
            UserManager<IdentityApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
 
        public async Task<Result<IdentityApplicationUser>> CreateAsync(string userName , string lastName,string fristName, string email, string password)
        {

            if (await _userManager.FindByEmailAsync(email) is not null)
                return Result<IdentityApplicationUser>.Failure("Email is already registered! ");

            if (await _userManager.FindByNameAsync(userName) is not null)
                return Result<IdentityApplicationUser>.Failure("UserName is already registered! ");

            var user = new IdentityApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = fristName,
                Password = password,
                LastName = lastName,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {

                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return Result<IdentityApplicationUser>.Failure(errors);
            }
            //return user;

            return Result<IdentityApplicationUser>.Success(user);
        }
    }
}
