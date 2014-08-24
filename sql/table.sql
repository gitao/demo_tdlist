CREATE TABLE [dbo].[tbl_todolist] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [item]     VARCHAR(150) NOT NULL,
    [complete] BIT             DEFAULT 0 NOT NULL,
    [priority] INT             NOT NULL,
    [dt_added] DATETIME        DEFAULT getdate() NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

