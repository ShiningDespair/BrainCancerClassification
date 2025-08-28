-- ===========================================
-- ROLES
-- ===========================================
CREATE TABLE [dbo].[Roles] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [RoleName] NVARCHAR(50) UNIQUE NOT NULL
);

-- ===========================================
-- USERS
-- ===========================================
CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ClerkId] NVARCHAR(100) NOT NULL UNIQUE,
    [Email] NVARCHAR(100) NOT NULL UNIQUE,
    [FirstName] NVARCHAR(50) NULL,
    [LastName] NVARCHAR(50) NULL,
    [ProfileImageUrl] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [LastLoginAt] DATETIME2 NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,

    -- Ek alanlar
    [PhoneNumber] NVARCHAR(20) NULL,
    [DateOfBirth] DATE NULL,
    [Gender] NVARCHAR(10) NULL,
    [Country] NVARCHAR(50) NULL,
    [City] NVARCHAR(50) NULL,
    [RoleId] INT NOT NULL DEFAULT 1,
    [IsEmailVerified] BIT NOT NULL DEFAULT 0,
    [PreferredLanguage] NCHAR(5) NOT NULL DEFAULT 'tr-TR',

    -- Audit
    [CreatedBy] INT NULL,
    [UpdatedBy] INT NULL,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] INT NULL,

    CONSTRAINT FK_Users_Roles FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id]),
    CONSTRAINT FK_Users_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT FK_Users_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT FK_Users_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[Users]([Id])
);

-- Indexler
CREATE NONCLUSTERED INDEX IX_Users_Email ON [dbo].[Users] ([Email]);
CREATE NONCLUSTERED INDEX IX_Users_IsActive ON [dbo].[Users] ([IsActive]);

-- ===========================================
-- DISEASES
-- ===========================================
CREATE TABLE [dbo].[Diseases] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL
);

-- ===========================================
-- PREDICTION MODELS
-- ===========================================
CREATE TABLE [dbo].[PredictionModels] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ModelName] NVARCHAR(100) NOT NULL,       -- "ResNet50 Tumor Classifier"
    [Version] NVARCHAR(20) NOT NULL,          -- "v1.0"
    [Description] NVARCHAR(500) NULL,
    [FilePath] NVARCHAR(500) NULL,            -- h5 dosyası yolu veya blob URL
    [Framework] NVARCHAR(50) NOT NULL DEFAULT 'TensorFlow/Keras',
    [InputShape] NVARCHAR(50) NULL,           -- "128x128x3"
    [NumClasses] INT NOT NULL,                -- örn: 4
    [Labels] NVARCHAR(200) NULL,              -- "glioma, meningioma, notumor, pituitary"
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- ===========================================
-- UPLOADS
-- ===========================================
CREATE TABLE [dbo].[Uploads] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [FilePath] NVARCHAR(500) NOT NULL,
    [FileType] NVARCHAR(20) NOT NULL,  -- (png, jpeg, nii, parquet, etc.)
    [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Uploads_Users FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);

-- Index
CREATE NONCLUSTERED INDEX IX_Uploads_UserId ON [dbo].[Uploads] ([UserId]);

-- ===========================================
-- PREDICTIONS
-- ===========================================
CREATE TABLE [dbo].[Predictions] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [UploadId] INT NOT NULL,
    [ModelId] INT NOT NULL,
    [DiseaseId] INT NULL,
    [PredictionResult] NVARCHAR(100) NOT NULL, -- örn. "meningioma"
    [Confidence] DECIMAL(5,4) NULL,           -- 0.0000 - 1.0000
    [PredictedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Predictions_Uploads FOREIGN KEY ([UploadId]) REFERENCES [dbo].[Uploads]([Id]),
    CONSTRAINT FK_Predictions_Models FOREIGN KEY ([ModelId]) REFERENCES [dbo].[PredictionModels]([Id]),
    CONSTRAINT FK_Predictions_Diseases FOREIGN KEY ([DiseaseId]) REFERENCES [dbo].[Diseases]([Id])
);

-- Indexler
CREATE NONCLUSTERED INDEX IX_Predictions_UploadId ON [dbo].[Predictions] ([UploadId]);
CREATE NONCLUSTERED INDEX IX_Predictions_ModelId ON [dbo].[Predictions] ([ModelId]);

-- ===========================================
-- TOOLS (Frontend için)
-- ===========================================
CREATE TABLE [dbo].[Tools] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Icon] NVARCHAR(100) NOT NULL,     -- React element type karşılığı string
    [Title] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [Href] NVARCHAR(200) NULL
);

CREATE TABLE [dbo].[ToolImages] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ToolId] INT NOT NULL,
    [Src] NVARCHAR(500) NOT NULL,
    [Alt] NVARCHAR(200) NOT NULL,
    CONSTRAINT FK_ToolImages_Tools FOREIGN KEY ([ToolId]) REFERENCES [dbo].[Tools]([Id])
);

-- Index
CREATE NONCLUSTERED INDEX IX_ToolImages_ToolId ON [dbo].[ToolImages] ([ToolId]);
