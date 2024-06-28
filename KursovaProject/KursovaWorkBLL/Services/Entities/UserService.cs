using KursovaWorkBLL.Contracts;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkDAL.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using KursovaWorkBLL.Services.AdditionalServices;
using KursovaWorkDAL.Entity.Service;
using System.Collections.Generic;

namespace KursovaWorkBLL.Services.Entities
{
    /// <summary>
    /// Implementation of the IUserService interface for business logic related to users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IDRetriever _idRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for users.</param>
        /// <param name="logger">Logger object for logging events.</param>
        /// <param name="idRetriever">Service to retrieve user identifier.</param>
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IDRetriever idRetriever)
        {
            _userRepository = userRepository;
            _logger = logger;
            _idRetriever = idRetriever;
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
            _logger.LogInformation("User successfully added");
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
            _logger.LogInformation("User successfully deleted");
        }

        public IEnumerable<User> GetAllUsers()
        {
            _logger.LogInformation("Retrieving list of all users");
            return _userRepository.GetAll();
        }

        public User GetLoggedInUser()
        {
            _logger.LogInformation("Retrieving identifier of logged-in user");
            int id = _idRetriever.GetLoggedInUserId();

            _logger.LogInformation("Retrieving logged-in user by their identifier");
            return _userRepository.GetById(id);
        }

        public User GetUserById(int id)
        {
            _logger.LogInformation("Retrieving user by their identifier");
            return _userRepository.GetById(id);
        }

        public User GetUserByEmail(string email)
        {
            _logger.LogInformation("Retrieving user by their email");
            return _userRepository.GetByEmail(email);
        }

        public void UpdatePasswordOfUser(string password, string confirmPassword, User user)
        {
            user.Password = password;
            user.ConfirmPassword = confirmPassword;

            _userRepository.Update(user);
            _logger.LogInformation("User password successfully updated");
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
            _logger.LogInformation("User data successfully updated");
        }

        public User ValidateUser(string email, string password)
        {
            var users = _userRepository.GetAll();
            string hashPassword = Encrypter.HashPassword(password);

            foreach (User user in users)
            {
                if (user.Email == email && user.Password == hashPassword)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
