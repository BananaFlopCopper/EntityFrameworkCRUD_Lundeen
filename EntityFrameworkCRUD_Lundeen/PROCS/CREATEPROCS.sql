Use Sales
GO

CREATE OR ALTER PROCEDURE AddCustomer
@Id			INT = null,
@FirstName	NVARCHAR(40),
@LastName	NVARCHAR(40),
@City		NVARCHAR(40) = null,
@Country	NVARCHAR(40) = null,
@Phone		NVARCHAR(20) = null
AS
BEGIN
	SET @Id = (SELECT MAX(Id) FROM Customer) + 1
	INSERT INTO Customer([Id], [FirstName], [LastName],[City],[Country],[Phone])
	VALUES (@Id,@FirstName,@LastName,@City,@Country,@Phone)
END
GO