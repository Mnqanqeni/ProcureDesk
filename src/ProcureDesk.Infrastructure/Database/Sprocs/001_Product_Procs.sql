USE ProcureDesk;
GO

CREATE OR ALTER PROC dbo.product_getAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Code, Name, CreatedDate, EditDate, CreateUser, EditUser
    FROM dbo.Product
    ORDER BY Code;
END
GO

CREATE OR ALTER PROC dbo.product_getByCode
    @Code VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Code, Name, CreatedDate, EditDate, CreateUser, EditUser
    FROM dbo.Product
    WHERE Code = @Code;
END
GO

CREATE OR ALTER PROC dbo.product_insert
    @Code VARCHAR(50),
    @Name VARCHAR(200),
    @User VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Product (Code, Name, CreatedDate, EditDate, CreateUser, EditUser)
    VALUES (@Code, @Name, SYSUTCDATETIME(), SYSUTCDATETIME(), @User, @User);
END
GO

CREATE OR ALTER PROC dbo.product_updateName
    @Code VARCHAR(50),
    @Name VARCHAR(200),
    @User VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Product
    SET Name = @Name,
        EditDate = SYSUTCDATETIME(),
        EditUser = @User
    WHERE Code = @Code;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE OR ALTER PROC dbo.product_delete
    @Code VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Product
    WHERE Code = @Code;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO