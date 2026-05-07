-- Activity Category Type
INSERT INTO MasterActivityType
(
    Name,
    CreatedBy
)
VALUES
('AwayFromSystem', 1),
('OnSystem', 1);

-- Activity Category
INSERT INTO MasterActivityCategory
(
    ActivityTypeId,
    Geo,
    Company,
    Department,
    ProcessName,
    SubCategoryName,
    SubCategoryContent,
    ColorHex,
    SortOrder,
    IsProductive,
    IsNonProductive,
    IsNoImpact,
    CreatedBy
)
VALUES
-- Away From System
(1, 'India', 'Sagility', 'All', 'System', 'Idle', 'Away from keyboard', '#D3D3D3', 1, 0, 0, 1, 1),
(1, 'India', 'Sagility', 'All', 'System', 'Locked', 'System locked', '#C0C0C0', 2, 0, 0, 1, 1),

-- On System Productive
(2, 'India', 'Sagility', 'All', 'Development', 'Visual Studio Code', 'Coding', '#22C55E', 3, 1, 0, 0, 1),
(2, 'India', 'Sagility', 'All', 'Communication', 'Microsoft Teams', 'Meetings', '#22C55E', 4, 1, 0, 0, 1),
(2, 'India', 'Sagility', 'All', 'Documentation', 'Confluence', 'Wiki', '#22C55E', 5, 1, 0, 0, 1),
(2, 'India', 'Sagility', 'All', 'Project', 'Jira', 'Task management', '#22C55E', 6, 1, 0, 0, 1),
(2, 'India', 'Sagility', 'All', 'Mail', 'Outlook', 'Email', '#22C55E', 7, 1, 0, 0, 1),
(2, 'India', 'Sagility', 'All', 'Sheets', 'Excel', 'Spreadsheet', '#22C55E', 8, 1, 0, 0, 1),

-- Neutral
(2, 'India', 'Sagility', 'All', 'Browser', 'Chrome - Work', 'Web browsing', '#3B82F6', 9, 0, 0, 1, 1),

-- Non Productive
(2, 'India', 'Sagility', 'All', 'Entertainment', 'YouTube', 'Video streaming', '#F97316', 10, 0, 1, 0, 1);

-- Session
INSERT INTO MasterEmployeeSession
(
    EmployeeId,
    DeviceId,
    LoginTime,
    LogoutTime,
    SessionDate,
    CreatedDate
)
VALUES
(
    2,
    NULL,
    '2026-04-08 09:00:00',
    '2026-04-08 18:59:24',
    '2026-04-08',
    GETUTCDATE()
);

-- Activity Log

INSERT INTO MasterActivityLog
(
    EmployeeId,
    CategoryId,
    LogDate,
    StartTime,
    EndTime,
    DurationSeconds,
    IsActive,
    IsDelete,
    CreatedDate
)
VALUES

-- Visual Studio Code (productive)
(
    3,
    3,
    '2026-04-08',
    '2026-04-08 09:00:00',
    '2026-04-08 11:30:00',
    9000,
    1,
    0,
    GETDATE()
),

-- Microsoft Teams (productive)
(
    3,
    4,
    '2026-04-08',
    '2026-04-08 11:45:00',
    '2026-04-08 13:15:00',
    5400,
    1,
    0,
    GETDATE()
),

-- Jira (productive)
(
    3,
    6,
    '2026-04-08',
    '2026-04-08 13:30:00',
    '2026-04-08 15:00:00',
    5400,
    1,
    0,
    GETDATE()
),

-- Outlook (productive)
(
    3,
    7,
    '2026-04-08',
    '2026-04-08 15:15:00',
    '2026-04-08 16:00:00',
    2700,
    1,
    0,
    GETDATE()
),

-- Excel (productive)
(
    3,
    8,
    '2026-04-08',
    '2026-04-08 16:00:00',
    '2026-04-08 16:39:36',
    2376,
    1,
    0,
    GETDATE()
);