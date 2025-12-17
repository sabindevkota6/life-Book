# Database Schema Fix Guide

## Problem
The database was created before the `PasswordHash` field was added to the User model, so the existing database table doesn't have the PasswordHash column.

## Solution Options

### Option 1: Reset Database (RECOMMENDED - Easiest)
**?? Warning: This will delete all existing data!**

1. Run your app
2. Navigate to `/database-test`
3. Click the **"?? Reset Database (Delete & Recreate)"** button
4. The database will be deleted and recreated with the correct schema including PasswordHash

**Result:** Fresh database with correct schema ?

---

### Option 2: Manual Database Deletion

1. Run your app and navigate to `/database-test`
2. Click **"Get Database Path"** to see where the database is stored
3. Close your app completely
4. Delete the `lifebook.db` file from that location
5. Restart your app - it will automatically create a new database with the correct schema

**Common paths:**
- **Windows:** `C:\Users\[YourName]\AppData\Local\Packages\[AppId]\LocalState\lifebook.db`
- **Android:** `/data/data/com.companyname.lifebook/files/lifebook.db`

---

### Option 3: Using Migrations (Advanced - For Production)

If you want to preserve existing data, you would need to use Entity Framework migrations:

```bash
# Install EF Core Tools
dotnet tool install --global dotnet-ef

# Add migration
dotnet ef migrations add AddPasswordHash

# Update database
dotnet ef database update
```

**Note:** This requires additional setup and is typically used for production apps with existing user data.

---

## Verification

After resetting the database:

1. Go to `/auth-test`
2. Try to register a new user
3. If successful, the PasswordHash column is working correctly!

## Database Schema

The User table now includes:
- `Id` (Guid) - Primary Key
- `Username` (string, unique, required)
- `Email` (string, unique, required)
- `PasswordHash` (string, required) ? **NEW**
- `FullName` (string, optional)
- `CreatedAt` (DateTime)
- `LastLoginAt` (DateTime, nullable)
- `IsActive` (bool)

---

## Next Steps

After fixing the database:
1. Test registration at `/auth-test`
2. Test login at `/auth-test`
3. Verify user data is stored correctly
