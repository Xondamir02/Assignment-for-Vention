using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers
{
    public class UserManager
    {
        private readonly AppDbContext _context;
        private ILogger<UserManager> _logger;
        private readonly JwtTokenManager _jwtTokenManager;

        public UserManager(AppDbContext context, ILogger<UserManager> logger, JwtTokenManager jwtTokenManager)
        {
            _context = context;
            _logger = logger;
            _jwtTokenManager = jwtTokenManager;
        }

        public async Task<User> Register(CreateUserModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                throw new UserAlreadyExistsException(model.Username);
            }

            var user = new User
            {
                Name = model.Name,
                Username = model.Username,

            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string> Login(LoginUserModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                throw new Exception("Username or Password is incorrect");
            }

            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Username or Password is incorrect");
            }

            var token = _jwtTokenManager.GenerateToken(user);

            return token;
        }

        public async Task<User?> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User?> GetUser(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }

    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string userName) : base($"User already exists with given username: {userName}")
        {
            
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId) : base($"User not found with given id: {userId}")
        {

        }

        public UserNotFoundException(string userName) : base("User not found with given username: {userName}")
        {
            
        }
    }

    public class Error
    {
        public string Message;
        public string Code;
    }

    public enum Errors
    {
        UserNotFound,
        UserNameAlreadyExists
    }
}
