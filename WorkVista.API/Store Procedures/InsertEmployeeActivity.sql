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
    Status,
    IsActive,
    IsDelete,
    CreatedDate
)
VALUES

-- VS Code (productive)
(3, 3, '2026-04-08',
 '2026-04-08 09:00:00',
 '2026-04-08 11:00:00',
 7200,
 'Productive',
 1,0,GETDATE()),

-- Teams (productive)
(3, 4, '2026-04-08',
 '2026-04-08 11:15:00',
 '2026-04-08 12:15:00',
 3600,
 'Productive',
 1,0,GETDATE()),

-- Jira (productive)
(3, 6, '2026-04-08',
 '2026-04-08 13:00:00',
 '2026-04-08 14:30:00',
 5400,
 'Productive',
 1,0,GETDATE()),

-- Chrome Work (neutral / no impact)
(3, 9, '2026-04-08',
 '2026-04-08 14:45:00',
 '2026-04-08 15:30:00',
 2700,
 'No Impact',
 1,0,GETDATE()),

-- Idle (neutral)
(3, 1, '2026-04-08',
 '2026-04-08 15:30:00',
 '2026-04-08 16:00:00',
 1800,
 'No Impact',
 1,0,GETDATE()),

-- YouTube (non-productive)
(3, 10, '2026-04-08',
 '2026-04-08 16:15:00',
 '2026-04-08 16:35:00',
 1200,
 'Non-Productive',
 1,0,GETDATE()),

-- Excel (productive)
(3, 8, '2026-04-08',
 '2026-04-08 16:40:00',
 '2026-04-08 17:39:36',
 2976,
 'Productive',
 1,0,GETDATE());