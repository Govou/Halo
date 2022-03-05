CREATE TABLE [armada].[VehicleDTSMasters] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [AvailabilityStart] DATETIME2 (7)  NOT NULL,
    [AvailablilityEnd]  DATETIME2 (7)  NOT NULL,
    [CreatedById]       BIGINT         NOT NULL,
    [CreatedAt]         DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdatedAt]         DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [IsDeleted]         BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [Caption]           NVARCHAR (MAX) NULL,
    [VehicleResourceId]   BIGINT         NULL,
    CONSTRAINT [PK_VehicleDTSMasters] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VehicleDTSMasters_UserProfiles_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VehicleDTSMasters_Vehicles_VehicleResourceId] FOREIGN KEY ([VehicleResourceId]) REFERENCES [armada].[Vehicles] ([Id])
);




