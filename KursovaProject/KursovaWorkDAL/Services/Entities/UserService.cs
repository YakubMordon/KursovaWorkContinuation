using Serilog;
using KursovaWork.Infrastructure.Services.DB;
using KursovaWork.Domain.Entities;
using KursovaWork.Application.Contracts.Repositories;
using KursovaWork.Application.Contracts.Services.Entities;
using KursovaWork.Application.Contracts.Services.Helpers.Transient;

namespace KursovaWork.Infrastructure.Services.Entities;

/// <summary>
/// Implementation of the IUserService interface for business logic related to users
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IIdRetriever _idRetriever;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">Repository for users.</param>
    /// <param name="idRetriever">Service to retrieve user identifier.</param>
    public UserService(IUserRepository userRepository, IIdRetriever idRetriever)
    {
        _userRepository = userRepository;
        _idRetriever = idRetriever;
    }

    public void AddUser(User user)
    {
        _userRepository.Add(user);
        Log.Information("User successfully added");
    }

    public void DeleteUser(User user)
    {
        _userRepository.Delete(user);
        Log.Information("User successfully deleted");
    }

    public IEnumerable<User> GetAllUsers()
    {
        Log.Information("Retrieving list of all users");
        return _userRepository.GetAll();
    }

    public User GetLoggedInUser()
    {
        Log.Information("Retrieving identifier of logged-in user");
        var id = _idRetriever.GetLoggedInUserId();

        Log.Information("Retrieving logged-in user by their identifier");
        return _userRepository.GetById(id);
    }

    public User GetUserById(int id)
    {
        Log.Information("Retrieving user by their identifier");
        return _userRepository.GetById(id);
    }

    public User GetUserByEmail(string email)
    {
        Log.Information("Retrieving user by their email");
        return _userRepository.GetByEmail(email);
    }

    public void UpdatePasswordOfUser(string password, string confirmPassword, User user)
    {
        user.Password = password;
        user.ConfirmPassword = confirmPassword;

        _userRepository.Update(user);
        Log.Information("User password successfully updated");
    }

    public void UpdateUser(User user)
    {
        _userRepository.Update(user);
        Log.Information("User data successfully updated");
    }

    public User ValidateUser(string email, string password)
    {
        var users = _userRepository.GetAll();
        var hashPassword = Encrypter.HashPassword(password);

        return users.FirstOrDefault(user => user.Email == email && user.Password == hashPassword);
    }
}