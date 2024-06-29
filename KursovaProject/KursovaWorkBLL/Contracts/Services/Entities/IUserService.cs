using KursovaWork.Domain.Entities;

namespace KursovaWork.Application.Contracts.Services.Entities;

/// <summary>
/// Interface for business logic related to users
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Method to get a user by their identifier
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>User</returns>
    User GetUserById(int id);

    /// <summary>
    /// Method to get a user by their email address
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>User</returns>
    User GetUserByEmail(string email);

    /// <summary>
    /// Method to get the currently logged-in user
    /// </summary>
    /// <returns>User</returns>
    User GetLoggedInUser();

    /// <summary>
    /// Method to add a new user to the database
    /// </summary>
    /// <param name="user">User</param>
    void AddUser(User user);

    /// <summary>
    /// Method to update user information in the database
    /// </summary>
    /// <param name="user">User</param>
    void UpdateUser(User user);

    /// <summary>
    /// Method to update user password in the database
    /// </summary>
    /// <param name="password">Password</param>
    /// <param name="confirmPassword">Password confirmation</param>
    /// <param name="user">User</param>
    void UpdatePasswordOfUser(string password, string confirmPassword, User user);

    /// <summary>
    /// Method to delete a user from the database
    /// </summary>
    /// <param name="user">User</param>
    void DeleteUser(User user);

    /// <summary>
    /// Method to get all users
    /// </summary>
    /// <returns>List of users</returns>
    IEnumerable<User> GetAllUsers();

    /// <summary>
    /// Method to validate a user
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="password">Password</param>
    /// <returns>Returns the user if the password and login are the same, otherwise null</returns>
    User ValidateUser(string email, string password);
}