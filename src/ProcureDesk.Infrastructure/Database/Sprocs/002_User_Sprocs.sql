USE ProcureDesk;
GO

-- =========================================
-- user_insert
-- =========================================
CREATE OR ALTER PROC dbo.user_insert
    @Id UNIQUEIDENTIFIER,
    @Username VARCHAR(50),
    @Email VARCHAR(200),
    @PasswordHash VARCHAR(200),
    @Role VARCHAR(50),
    @CreateUser VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @now DATETIME2 = SYSUTCDATETIME();

    INSERT INTO dbo.[User] (
        Id,
        Username,
        Email,
        PasswordHash,
        Role,
        IsActive,
        CreatedDate,
        EditDate,
        CreateUser,
        EditUser
    )
    VALUES (
        @Id,
        @Username,
        @Email,
        @PasswordHash,
        @Role,
        1,
        @now,
        @now,
        @CreateUser,
        @CreateUser
    );
END
GO


-- =========================================
-- user_getByUsername
-- =========================================
CREATE OR ALTER PROC dbo.user_getByUsername
    @Username VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        Email,
        PasswordHash,
        Role,
        IsActive,
        CreatedDate,
        EditDate,
        CreateUser,
        EditUser
    FROM dbo.[User]
    WHERE Username = @Username;
END
GO


-- =========================================
-- user_getByEmail
-- =========================================
CREATE OR ALTER PROC dbo.user_getByEmail
    @Email VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        Email,
        PasswordHash,
        Role,
        IsActive,
        CreatedDate,
        EditDate,
        CreateUser,
        EditUser
    FROM dbo.[User]
    WHERE Email = @Email;
END
GO