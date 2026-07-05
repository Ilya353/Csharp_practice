USE [FitnessTrackerDB]
GO
/****** Объект:  Table [dbo].[Activities]    Дата создания скрипта: 05.07.2026 22:25:38 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExerciseId] [int] NOT NULL,
	[ActivityDate] [date] NOT NULL,
	[Minutes] [int] NOT NULL,
	[Notes] [nvarchar](200) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[Exercises]    Дата создания скрипта: 05.07.2026 22:25:38 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exercises](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ProgramId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[TrainingPrograms]    Дата создания скрипта: 05.07.2026 22:25:38 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingPrograms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Activities] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Activities] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Exercises] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Exercises] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Exercises] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[TrainingPrograms] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TrainingPrograms] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TrainingPrograms] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_Exercise] FOREIGN KEY([ExerciseId])
REFERENCES [dbo].[Exercises] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Exercise]
GO
ALTER TABLE [dbo].[Exercises]  WITH CHECK ADD  CONSTRAINT [FK_Exercises_Program] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[TrainingPrograms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Exercises] CHECK CONSTRAINT [FK_Exercises_Program]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [CHK_Minutes_Max] CHECK  (([Minutes]<=(1440)))
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [CHK_Minutes_Max]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [CHK_Minutes_Positive] CHECK  (([Minutes]>(0)))
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [CHK_Minutes_Positive]
GO
