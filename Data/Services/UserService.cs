using life_Book.Data.Models;
using life_Book.Data.Utils;
using Microsoft.EntityFrameworkCore;
namespace life_Book.Data.Services;

public class UserService
{
    private readonly DatabaseSchema _db;

    public UserService(DatabaseSchema db)
    {
        _db = db;
    }

    // Register a new user
    public async Task<(bool Success, string Message, User? User)> RegisterAsync(string username, string email, string password, string? fullName = null)
    {
        try
        {
            // Check if username already exists
       var existingUsername = await _db.Users.AnyAsync(u => u.Username == username);
   if (existingUsername)
         return (false, "Username already exists", null);

            // Check if email already exists
         var existingEmail = await _db.Users.AnyAsync(u => u.Email == email);
     if (existingEmail)
     return (false, "Email already exists", null);

 // Validate password
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
         return (false, "Password must be at least 6 characters", null);

// Create new user
    var user = new User
       {
     Username = username,
        Email = email,
    PasswordHash = PasswordHasher.HashPassword(password),
  FullName = fullName,
    CreatedAt = DateTime.UtcNow,
       IsActive = true
   };

        _db.Users.Add(user);
   await _db.SaveChangesAsync();

  return (true, "Registration successful", user);
        }
        catch (Exception ex)
 {
    return (false, $"Registration failed: {ex.Message}", null);
        }
    }

    // Login user
    public async Task<(bool Success, string Message, User? User)> LoginAsync(string usernameOrEmail, string password)
    {
  try
     {
          // Find user by username or email
            var user = await _db.Users
     .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

       if (user == null)
  return (false, "User not found", null);

     // Check if user is active
    if (!user.IsActive)
   return (false, "Account is inactive", null);

       // Verify password
            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
           return (false, "Invalid password", null);

     // Update last login time
  user.LastLoginAt = DateTime.UtcNow;
         await _db.SaveChangesAsync();

       return (true, "Login successful", user);
      }
        catch (Exception ex)
        {
  return (false, $"Login failed: {ex.Message}", null);
        }
    }

  // Get user by ID
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
      return await _db.Users.FindAsync(userId);
    }

    // Get user by username
  public async Task<User?> GetUserByUsernameAsync(string username)
    {
 return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    // Get user by email
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    // Update user profile
    public async Task<(bool Success, string Message)> UpdateProfileAsync(Guid userId, string? fullName, string? email)
    {
   try
   {
            var user = await _db.Users.FindAsync(userId);
      if (user == null)
        return (false, "User not found");

  // Check if email is being changed and if it's already taken
          if (!string.IsNullOrWhiteSpace(email) && email != user.Email)
          {
      var emailExists = await _db.Users.AnyAsync(u => u.Email == email && u.Id != userId);
       if (emailExists)
    return (false, "Email already exists");

 user.Email = email;
}

            if (!string.IsNullOrWhiteSpace(fullName))
  user.FullName = fullName;

   await _db.SaveChangesAsync();
            return (true, "Profile updated successfully");
        }
        catch (Exception ex)
        {
      return (false, $"Update failed: {ex.Message}");
        }
    }

    // Change password
    public async Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        try
        {
   var user = await _db.Users.FindAsync(userId);
       if (user == null)
         return (false, "User not found");

     // Verify current password
     if (!PasswordHasher.VerifyPassword(currentPassword, user.PasswordHash))
    return (false, "Current password is incorrect");

          // Validate new password
    if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
  return (false, "New password must be at least 6 characters");

user.PasswordHash = PasswordHasher.HashPassword(newPassword);
         await _db.SaveChangesAsync();

       return (true, "Password changed successfully");
      }
    catch (Exception ex)
        {
       return (false, $"Password change failed: {ex.Message}");
        }
    }

    // Deactivate user account
    public async Task<(bool Success, string Message)> DeactivateAccountAsync(Guid userId)
    {
    try
      {
      var user = await _db.Users.FindAsync(userId);
      if (user == null)
      return (false, "User not found");

            user.IsActive = false;
   await _db.SaveChangesAsync();

  return (true, "Account deactivated successfully");
  }
        catch (Exception ex)
    {
     return (false, $"Deactivation failed: {ex.Message}");
   }
    }
}
