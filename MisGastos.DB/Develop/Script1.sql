--drop TABLE dbo.PaymentMethod

CREATE TABLE dbo.PaymentMethod(
PaymentMethodId INT IDENTITY PRIMARY KEY NOT NULL,
UserId nvarchar(128) NOT NULL,
Description nvarchar(30) NOT NULL,
DeadLine date,
CreatedDate datetime  NOT NULL,
UpdatedDate datetime
)

INSERT INTO PaymentMethod (UserId, Description, DeadLine, CreatedDate)
VALUES (@UserId, @Description, @DeadLine, GETDATE())

INSERT INTO PaymentMethod (UserId, Description, DeadLine, CreatedDate)
VALUES ('123', 'Efectivo', NULL, GETDATE())

SELECT	PaymentMethodId,
		UserId,
		Description,
		DeadLine,
		CreatedDate,
		UpdatedDate
FROM PaymentMethod
WHERE UserId = '123'
Order by Description


TRUNCATE TABLE PaymentMethod