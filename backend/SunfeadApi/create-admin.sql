-- SQL Script to Create Admin User
-- Run this after registering a user through the signup page

-- Step 1: Create the Admin role (if it doesn't exist)
IF NOT EXISTS (SELECT 1 FROM roles WHERE NormalizedName = 'ADMIN')
BEGIN
    INSERT INTO roles (Id, Name, NormalizedName, Description, CreatedAt)
    VALUES (NEWID(), 'Admin', 'ADMIN', 'Administrator role with full access', GETUTCDATE());
    PRINT 'Admin role created successfully';
END
ELSE
BEGIN
    PRINT 'Admin role already exists';
END
GO

-- Step 2: Assign admin role to a user
-- REPLACE 'your-email@example.com' with the actual email of the user you want to make admin

DECLARE @UserEmail NVARCHAR(255) = 'admin@sunfead.com'; -- CHANGE THIS TO YOUR USER'S EMAIL
DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @RoleId UNIQUEIDENTIFIER;

-- Get the user ID
SELECT @UserId = Id FROM users WHERE Email = @UserEmail AND IsDeleted = 0;

IF @UserId IS NULL
BEGIN
    PRINT 'ERROR: User with email ' + @UserEmail + ' not found!';
    PRINT 'Please register the user first through the signup page, then run this script.';
END
ELSE
BEGIN
    -- Get the Admin role ID
    SELECT @RoleId = Id FROM roles WHERE NormalizedName = 'ADMIN';

    -- Check if user already has admin role
    IF EXISTS (SELECT 1 FROM user_roles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        PRINT 'User ' + @UserEmail + ' is already an admin';
    END
    ELSE
    BEGIN
        -- Assign admin role to user
        INSERT INTO user_roles (UserId, RoleId, AssignedAt)
        VALUES (@UserId, @RoleId, GETUTCDATE());
        
        PRINT 'SUCCESS: User ' + @UserEmail + ' has been granted admin access!';
        PRINT 'User ID: ' + CAST(@UserId AS NVARCHAR(50));
    END
END
GO

-- Optional: View all admin users
SELECT 
    u.Username,
    u.Email,
    r.Name as RoleName,
    ur.AssignedAt
FROM users u
INNER JOIN user_roles ur ON u.Id = ur.UserId
INNER JOIN roles r ON ur.RoleId = r.Id
WHERE r.NormalizedName = 'ADMIN'
AND u.IsDeleted = 0;
