using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync() => _userRepository.GetAllAsync();

        public Task<User?> GetUserByIdAsync(int id) => _userRepository.GetByIdAsync(id);

        public Task CreateUserAsync(User user) => _userRepository.AddAsync(user);

        public Task UpdateUserAsync(User user) => _userRepository.UpdateAsync(user);

        public Task DeleteUserAsync(int id) => _userRepository.DeleteAsync(id);
    }
}