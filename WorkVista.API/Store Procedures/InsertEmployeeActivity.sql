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
    4,
    NULL,
    '2026-04-07 09:00:00',
    '2026-04-07 18:00:00',
    '2026-04-07',
    GETUTCDATE()
);

-- Activity Log

INSERT INTO MasterActivityLog
(
    EmployeeId,
    CategoryId,
    SessionId,
    StartTime,
    EndTime,
    DurationSeconds,
    Status,
    LogDate,
    CreatedDate
)
VALUES
-- Visual Studio Code (2h)
(4, 3, 1, '2026-04-07 09:00:00', '2026-04-07 11:00:00', 7200, 'Completed', '2026-04-07', GETUTCDATE()),

-- Microsoft Teams (1h 30m)
(4, 4, 1, '2026-04-07 11:00:00', '2026-04-07 12:30:00', 5400, 'Completed', '2026-04-07', GETUTCDATE()),

-- Confluence (1h 10m)
(4, 5, 1, '2026-04-07 12:30:00', '2026-04-07 13:40:00', 4200, 'Completed', '2026-04-07', GETUTCDATE()),

-- Jira (1h)
(4, 6, 1, '2026-04-07 13:40:00', '2026-04-07 14:40:00', 3600, 'Completed', '2026-04-07', GETUTCDATE()),

-- Outlook (50m)
(4, 7, 1, '2026-04-07 14:40:00', '2026-04-07 15:30:00', 3000, 'Completed', '2026-04-07', GETUTCDATE()),

-- Chrome Work (45m)
(4, 9, 1, '2026-04-07 15:30:00', '2026-04-07 16:15:00', 2700, 'Completed', '2026-04-07', GETUTCDATE()),

-- Excel (35m)
(4, 8, 1, '2026-04-07 16:15:00', '2026-04-07 16:50:00', 2100, 'Completed', '2026-04-07', GETUTCDATE()),

-- YouTube (22m)
(4, 10, 1, '2026-04-07 16:50:00', '2026-04-07 17:12:00', 1320, 'Completed', '2026-04-07', GETUTCDATE()),

-- Idle
(4, 1, 1, '2026-04-07 17:12:00', '2026-04-07 18:00:00', 2880, 'Completed', '2026-04-07', GETUTCDATE());