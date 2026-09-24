USE [AirlineBookingSystem]
GO
/****** Object:  Table [dbo].[Bookings]    Script Date: 1/10/2025 10:31:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookings](
	[Id] [uniqueidentifier] NOT NULL,
	[FlightId] [uniqueidentifier] NULL,
	[PassengerName] [nvarchar](100) NULL,
	[SeatNumber] [nvarchar](10) NULL,
	[BookingDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flights]    Script Date: 1/10/2025 10:31:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flights](
	[Id] [uniqueidentifier] NOT NULL,
	[FlightNumber] [nvarchar](50) NULL,
	[Origin] [nvarchar](50) NULL,
	[Destination] [nvarchar](50) NULL,
	[DepartureTime] [datetime] NULL,
	[ArrivalTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 1/10/2025 10:31:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [uniqueidentifier] NOT NULL,
	[Recipient] [nvarchar](100) NULL,
	[Message] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[SentAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 1/10/2025 10:31:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [uniqueidentifier] NOT NULL,
	[BookingId] [uniqueidentifier] NULL,
	[Amount] [decimal](18, 2) NULL,
	[PaymentDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Bookings] ([Id], [FlightId], [PassengerName], [SeatNumber], [BookingDate]) VALUES (N'ddd990fb-fdcc-4f48-9faf-065226e0ceee', N'c9a4fe61-96f6-4a01-a119-7da31f2bdc1c', N'John Doe', N'12A', CAST(N'2025-01-10T22:28:06.183' AS DateTime))
GO
INSERT [dbo].[Bookings] ([Id], [FlightId], [PassengerName], [SeatNumber], [BookingDate]) VALUES (N'9d18714a-86ee-4413-abec-98648b3f07c9', N'a5c5dc35-731a-4465-8d92-1eaa0501bdc5', N'Jane Smith', N'7B', CAST(N'2025-01-10T22:28:06.183' AS DateTime))
GO
INSERT [dbo].[Flights] ([Id], [FlightNumber], [Origin], [Destination], [DepartureTime], [ArrivalTime]) VALUES (N'a5c5dc35-731a-4465-8d92-1eaa0501bdc5', N'FL002', N'Chicago', N'Miami', CAST(N'2025-01-11T14:00:00.000' AS DateTime), CAST(N'2025-01-11T17:00:00.000' AS DateTime))
GO
INSERT [dbo].[Flights] ([Id], [FlightNumber], [Origin], [Destination], [DepartureTime], [ArrivalTime]) VALUES (N'c9a4fe61-96f6-4a01-a119-7da31f2bdc1c', N'FL001', N'New York', N'Los Angeles', CAST(N'2025-01-10T09:00:00.000' AS DateTime), CAST(N'2025-01-10T12:00:00.000' AS DateTime))
GO
INSERT [dbo].[Flights] ([Id], [FlightNumber], [Origin], [Destination], [DepartureTime], [ArrivalTime]) VALUES (N'985495d8-12bd-42fe-b07a-b1ea8f70346f', N'FL003', N'San Francisco', N'Houston', CAST(N'2025-01-12T06:00:00.000' AS DateTime), CAST(N'2025-01-12T10:00:00.000' AS DateTime))
GO
INSERT [dbo].[Notifications] ([Id], [Recipient], [Message], [Type], [SentAt]) VALUES (N'f49c3819-0da3-4797-a45b-5de6b9bc079a', N'jane.smith@example.com', N'Your payment was successful!', N'SMS', CAST(N'2025-01-10T22:28:06.190' AS DateTime))
GO
INSERT [dbo].[Notifications] ([Id], [Recipient], [Message], [Type], [SentAt]) VALUES (N'b7676068-956b-415b-a2f8-de3d9a466bd6', N'john.doe@example.com', N'Your booking is confirmed!', N'Email', CAST(N'2025-01-10T22:28:06.190' AS DateTime))
GO
INSERT [dbo].[Payments] ([Id], [BookingId], [Amount], [PaymentDate]) VALUES (N'88b77bc8-2e79-41f0-9df8-9c7f6f9d3cf1', N'ddd990fb-fdcc-4f48-9faf-065226e0ceee', CAST(200.00 AS Decimal(18, 2)), CAST(N'2025-01-10T22:28:06.190' AS DateTime))
GO
INSERT [dbo].[Payments] ([Id], [BookingId], [Amount], [PaymentDate]) VALUES (N'd9d9c724-4e38-4e9b-b7b6-c8ef62d3dc44', N'9d18714a-86ee-4413-abec-98648b3f07c9', CAST(150.00 AS Decimal(18, 2)), CAST(N'2025-01-10T22:28:06.190' AS DateTime))
GO
