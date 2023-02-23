CREATE TABLE [dbo].[PaymentMethod] (
    [PaymentMethodId] INT            NOT NULL,
    [UserId]          NVARCHAR (128) NOT NULL,
    [Description]     NVARCHAR (30)  NOT NULL,
    [DeadLine]        DATE           NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [UpdatedDate]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([PaymentMethodId] ASC)
);



