SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

drop table if exists dbo.MatchMove

drop table if exists dbo.Piece
drop table if exists dbo.PieceType
drop table if exists dbo.Color

drop table if exists dbo.Match
drop table if exists dbo.Player
drop table if exists dbo.Gamemode




create table dbo.Player(
	ID int identity(1,1) not null,
	rating int not null,
	username varchar(100) not null,
	realName varchar(100) not null,

	primary key clustered(ID)
        WITH (PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON
	)
)

create table dbo.Gamemode(
	ID int identity(1,1),
	gamemodeName varchar(100) not null,

	primary key clustered(ID)
	    WITH (PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON
	)
)

create table dbo.Color(
	ID int identity(1,1),
	colorName varchar(10),

	primary key clustered(ID)
	    WITH (PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON
	)
)

create table dbo.Match(
	ID int identity(1,1),
	playerBlackID int not null,
	playerWhiteID int not null,
	gamemodeID int not null,
	winningColorID int not null,

	primary key clustered(ID)        
		WITH (PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON
	),
	foreign key(playerBlackID) references dbo.Player(ID),
	foreign key(playerWhiteID) references dbo.Player(ID),
	foreign key(gamemodeID) references dbo.Gamemode(ID),  
	foreign key(winningColorID) references dbo.Color(ID)  
)



create table dbo.PieceType(
	ID int identity(1,1),
	pieceTypeName varchar(20) not null,

	primary key clustered(ID)
		WITH (PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON
	)
)

create table dbo.Piece(
	ID int identity(1,1),
	pieceTypeID int not null,
	colorID int not null,
	matchID int not null,
	positionX int,
	positionY int,
	

	primary key clustered(ID)
		WITH (PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON
	),
	foreign key(pieceTypeID) references dbo.PieceType(ID),
	foreign key(colorID) references dbo.Color(ID),
	foreign key(matchID) references dbo.Match(ID),
)




GO
SET ANSI_PADDING OFF
GO

SET IDENTITY_INSERT [dbo].[Player] ON

insert into dbo.Player (ID, rating, username, realName) values (0, 501, 'Player 1', 'Joppe Peeters')
insert into dbo.Player (ID, rating, username, realName) values (1, 500, 'Player 2', 'Trine Druyts')

SET IDENTITY_INSERT [dbo].[Player] OFF
SET IDENTITY_INSERT [dbo].[Gamemode] ON

insert into dbo.Gamemode (ID, gamemodeName) values (0, 'Normal')
insert into dbo.Gamemode (ID, gamemodeName) values (1, 'Ranked')

SET IDENTITY_INSERT [dbo].[Gamemode] OFF
SET IDENTITY_INSERT [dbo].[Color] ON

insert into dbo.Color (ID, colorName) values (0, 'Black')
insert into dbo.Color (ID, colorName) values (1, 'White')

SET IDENTITY_INSERT [dbo].[Color] OFF
SET IDENTITY_INSERT [dbo].[PieceType] ON

insert into dbo.PieceType (ID, pieceTypeName) values (0, 'Pawn')
insert into dbo.PieceType (ID, pieceTypeName) values (1, 'Rook')
insert into dbo.PieceType (ID, pieceTypeName) values (2, 'Bishop')
insert into dbo.PieceType (ID, pieceTypeName) values (3, 'Knight')
insert into dbo.PieceType (ID, pieceTypeName) values (4, 'King')
insert into dbo.PieceType (ID, pieceTypeName) values (5, 'Queen')

SET IDENTITY_INSERT [dbo].[PieceType] OFF

