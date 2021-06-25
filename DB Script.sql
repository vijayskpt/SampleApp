USE [SampleDB]
GO
/****** Object:  Table [dbo].[Price_info]    Script Date: 24-06-2021 08:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Price_info](
	[ProductID] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [UQ__Price_info__0BC6C43E] UNIQUE NONCLUSTERED 
(
	[ProductID] ASC,
	[Price] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_info]    Script Date: 24-06-2021 08:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](200) NOT NULL,
	[ProductDescription] [varchar](300) NOT NULL,
 CONSTRAINT [PK_Product_info] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ProductName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Price_info]  WITH CHECK ADD  CONSTRAINT [FK_Price_info_Product_info] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product_info] ([ID])
GO
ALTER TABLE [dbo].[Price_info] CHECK CONSTRAINT [FK_Price_info_Product_info]
GO
/****** Object:  StoredProcedure [dbo].[AddProductInfo]    Script Date: 24-06-2021 08:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddProductInfo]
(
@ProductName varchar(200),
@ProductDescription varchar(300),
@Price float
) as
begin
if(@ProductName = '')
begin
Select 1 as ErrorCode,'Please Enter the ProductName' as ErrorDesc
return
end
if(@ProductDescription = '')
begin
Select 1 as ErrorCode,'Please Enter the ProductDescription' as ErrorDesc
return
end
if(@Price = '')
begin
Select 1 as ErrorCode,'Please Enter the Price' as ErrorDesc
return
end

declare @ProductID int
Insert into [Product_info]([ProductName],[ProductDescription]) values (@ProductName,@ProductDescription)
set @ProductID=@@identity
insert into [Price_info]([ProductID],[Price]) values (@ProductID,@Price)
 Select 0 as ErrorCode,'Added Sucessfully' as ErrorDesc
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteProductInfo]    Script Date: 24-06-2021 08:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[DeleteProductInfo]
(
@ID int
) as
begin
delete from [Price_info] where [ProductID]=@ID
delete from [Product_info] where ID=@ID
Select 0 as ErrorCode,'Deleted Sucessfully' as ErrorDesc
end
GO
/****** Object:  StoredProcedure [dbo].[GetAllItems]    Script Date: 24-06-2021 08:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetAllItems]
as
begin
select PD.[ID] as ItemCode,PD.[ProductName] as ItemName,PD.[ProductDescription] as Description,PR.[Price] from [Product_info] PD inner join [Price_info] PR on PR.[ProductID]=PD.ID
 ORDER BY PD.[ID]
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateProductInfo]    Script Date: 24-06-2021 08:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[UpdateProductInfo]
(
@ID int,
@ProductName varchar(200),
@ProductDescription varchar(300),
@Price float
) as
begin

Update [Product_info] set [ProductName]=@ProductName,[ProductDescription]=@ProductDescription where ID=@ID
Update [Price_info] set [Price]=@Price where [ProductID]=@ID
 Select 0 as ErrorCode,'Updated Sucessfully' as ErrorDesc
end
GO
