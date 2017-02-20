USE [master]
CREATE DATABASE [SecureChat]
ALTER DATABASE [SecureChat] SET COMPATIBILITY_LEVEL = 100
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SecureChat].[dbo].[sp_fulltext_database] @action = 'enable'
end
ALTER DATABASE [SecureChat] SET ANSI_NULL_DEFAULT OFF 
ALTER DATABASE [SecureChat] SET ANSI_NULLS OFF 
ALTER DATABASE [SecureChat] SET ANSI_PADDING OFF 
ALTER DATABASE [SecureChat] SET ANSI_WARNINGS OFF 
ALTER DATABASE [SecureChat] SET ARITHABORT OFF 
ALTER DATABASE [SecureChat] SET AUTO_CLOSE OFF 
ALTER DATABASE [SecureChat] SET AUTO_CREATE_STATISTICS ON 
ALTER DATABASE [SecureChat] SET AUTO_SHRINK OFF 
ALTER DATABASE [SecureChat] SET AUTO_UPDATE_STATISTICS ON 
ALTER DATABASE [SecureChat] SET CURSOR_CLOSE_ON_COMMIT OFF 
ALTER DATABASE [SecureChat] SET CURSOR_DEFAULT  GLOBAL 
ALTER DATABASE [SecureChat] SET CONCAT_NULL_YIELDS_NULL OFF 
ALTER DATABASE [SecureChat] SET NUMERIC_ROUNDABORT OFF 
ALTER DATABASE [SecureChat] SET QUOTED_IDENTIFIER OFF 
ALTER DATABASE [SecureChat] SET RECURSIVE_TRIGGERS OFF 
ALTER DATABASE [SecureChat] SET  DISABLE_BROKER 
ALTER DATABASE [SecureChat] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
ALTER DATABASE [SecureChat] SET DATE_CORRELATION_OPTIMIZATION OFF 
ALTER DATABASE [SecureChat] SET TRUSTWORTHY OFF 
ALTER DATABASE [SecureChat] SET ALLOW_SNAPSHOT_ISOLATION OFF 
ALTER DATABASE [SecureChat] SET PARAMETERIZATION SIMPLE 
ALTER DATABASE [SecureChat] SET READ_COMMITTED_SNAPSHOT OFF 
ALTER DATABASE [SecureChat] SET HONOR_BROKER_PRIORITY OFF 
ALTER DATABASE [SecureChat] SET  READ_WRITE 
ALTER DATABASE [SecureChat] SET RECOVERY FULL 
ALTER DATABASE [SecureChat] SET  MULTI_USER 
ALTER DATABASE [SecureChat] SET PAGE_VERIFY CHECKSUM  
ALTER DATABASE [SecureChat] SET DB_CHAINING OFF 

USE [SecureChat]

/****** Table [dbo].[Users] ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Users](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](max) NULL,
 CONSTRAINT [id_PK] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


/****** Table [dbo].[MessagesHistory]  ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[MessagesHistory](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[sender] [nvarchar](50) NOT NULL,
	[recepient] [nvarchar](50) NOT NULL,
	[message] [ntext] NULL,
	[senddate] [datetime] NOT NULL,
	[delivered] [bit] NULL,
	[recieveddate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



/****** Table [dbo].[Messages]   ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Messages](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[sender] [nvarchar](50) NOT NULL,
	[recepient] [nvarchar](50) NOT NULL,
	[message] [ntext] NULL,
	[senddate] [datetime] NOT NULL,
	[delivered] [bit] NULL,
	[recieveddate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


/****** Table [dbo].[Login]  ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Login](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[logindate] [datetime] NOT NULL,
	[sessionid] [nvarchar](max) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[lastmessagetime] [datetime] NULL,
	[useripaddress] [nvarchar](50) NULL,
	[loggedin] [bit] NULL,
 CONSTRAINT [UK_Login_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_Login_username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



/****** StoredProcedure [dbo].[ReadMessage]   ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE PROCEDURE [dbo].[ReadMessage]
     @recepient nvarchar(50),
     @message ntext out,
     @sender nvarchar(50) out
AS 
BEGIN
	SET NOCOUNT ON;
	--declare @message varchar(max)
	declare @id numeric
	declare @currentdatetime datetime
	declare @lasthistory numeric

	set @currentdatetime = GETDATE()
	select @message=message,@id=id,@sender=sender from Messages where recepient=@recepient and (delivered <>'true' or delivered is null) order by senddate asc
	--insert into MessagesHistory (sender,recepient,message,senddate) select sender,recepient,message,senddate  from Messages where id=@id
	--set @lasthistory = @@IDENTITY
	--update MessagesHistory set recieveddate=@currentdatetime , delivered='true' where id=@lasthistory
	delete from Messages where id=@id
END

/****** ForeignKey [FK_username]   ******/
ALTER TABLE [dbo].[Login]  WITH CHECK ADD  CONSTRAINT [FK_username] FOREIGN KEY([username])
REFERENCES [dbo].[Users] ([username])
ALTER TABLE [dbo].[Login] CHECK CONSTRAINT [FK_username]

/****** ForeignKey [FK_recepient]    ******/
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_recepient] FOREIGN KEY([recepient])
REFERENCES [dbo].[Users] ([username])
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_recepient]

/****** ForeignKey [FK_sender]    ******/
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_sender] FOREIGN KEY([sender])
REFERENCES [dbo].[Users] ([username])
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_sender]

/****** ForeignKey [MH_FK_recepient]    ******/
ALTER TABLE [dbo].[MessagesHistory]  WITH CHECK ADD  CONSTRAINT [MH_FK_recepient] FOREIGN KEY([recepient])
REFERENCES [dbo].[Users] ([username])
ALTER TABLE [dbo].[MessagesHistory] CHECK CONSTRAINT [MH_FK_recepient]

/****** ForeignKey [MH_FK_sender]    ******/
ALTER TABLE [dbo].[MessagesHistory]  WITH CHECK ADD  CONSTRAINT [MH_FK_sender] FOREIGN KEY([sender])
REFERENCES [dbo].[Users] ([username])
ALTER TABLE [dbo].[MessagesHistory] CHECK CONSTRAINT [MH_FK_sender]