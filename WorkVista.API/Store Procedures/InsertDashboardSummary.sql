INSERT INTO MasterEmployee
(
    NetworkId,
    EmployeeCode,
    FullName,
    Email,
    DailyCapacity,
    MANAGER1,
    MANAGER1_EMPLOYEE_ID,
    CONSOLE_LOGIN_ID,
    ROLE_NAME,
    EMPLOYEE_ID,
    DESIGNATION,
    GEO,
    Department
)
VALUES
(
    'WV1001',
    'EMP1001',
    'Sanjay Bakshi',
    'sanjay.bakshi@workvista.com',
    8,
    NULL,
    NULL,
    'sbakshi',
    'Manager',
    '1001',
    'Reporting Manager',
    'Bengaluru',
    'Operations'
);

INSERT INTO MasterEmployee
(
    NetworkId,
    EmployeeCode,
    FullName,
    Email,
    DailyCapacity,
    MANAGER1,
    MANAGER1_EMPLOYEE_ID,
    CONSOLE_LOGIN_ID,
    ROLE_NAME,
    EMPLOYEE_ID,
    DESIGNATION,
    GEO,
    Department
)
VALUES
(
    'WV1101',
    'EMP1101',
    'Rahul Verma',
    'rahul.verma@workvista.com',
    8,
    'Sanjay Bakshi',
    '1001',
    'rverma',
    'Team Lead',
    '1101',
    'Team Lead - Analytics',
    'Bengaluru',
    'Operations'
),
(
    'WV1102',
    'EMP1102',
    'Neha Kapoor',
    'neha.kapoor@workvista.com',
    8,
    'Sanjay Bakshi',
    '1001',
    'nkapoor',
    'Team Lead',
    '1102',
    'Team Lead - HR Ops',
    'Chennai',
    'Operations'
);

INSERT INTO MasterEmployee
(
    NetworkId,
    EmployeeCode,
    FullName,
    Email,
    DailyCapacity,
    MANAGER1,
    MANAGER1_EMPLOYEE_ID,
    MANAGER2,
    MANAGER2_EMPLOYEE_ID,
    CONSOLE_LOGIN_ID,
    ROLE_NAME,
    EMPLOYEE_ID,
    DESIGNATION,
    GEO,
    Department
)
VALUES
(
    'WV2001',
    'EMP2001',
    'Manish Sharma',
    'manish.sharma@workvista.com',
    8,
    'Rahul Verma',
    '1101',
    'Sanjay Bakshi',
    '1001',
    'msharma',
    'Employee',
    '2001',
    'Enterprise Architect',
    'Bengaluru',
    'Operations'
),
(
    'WV2002',
    'EMP2002',
    'Aparna Sundar',
    'aparna.sundar@workvista.com',
    8,
    'Rahul Verma',
    '1101',
    'Sanjay Bakshi',
    '1001',
    'asundar',
    'Employee',
    '2002',
    'HR Compliance Lead',
    'Bengaluru',
    'Operations'
);


--1.) TOT_LOGGEDHOURS = 9.00 i.e. TOT_LOGGEDHOURS is total hours logged includes Productive + Non Productive + Idle + Neutral
--2.) TOT_ACTIVEHOURS = 8.20 i.e. TOT_ACTIVEHOURS is total hours logged includes Productive + Non Productive + Neutral
--3.) TOT_TIMEONSYSTEM = 8.20 i.e. Same as TOT_ACTIVEHOURS
--4.) TOT_TIMEAWAYFROMSYSTEM = 0.80 i.e. TOT_TIMEAWAYFROMSYSTEM is total hours logged includes Idle only
--5.) TOT_IDLETIME = 0.80 i.e. Same as TOT_TIMEAWAYFROMSYSTEM
DECLARE @StartDate DATE = '2026-04-07';
DECLARE @EndDate DATE = '2026-05-06';

WHILE @StartDate <= @EndDate
BEGIN
    INSERT INTO EmployeeActivityLogSummary
    (
        EmployeeId,
        LoggedDate,
        TOT_LOGGEDHOURS,
        TOT_ACTIVEHOURS,
        TOT_TIMEONSYSTEM,
        TOT_TIMEAWAYFROMSYSTEM,
        TOT_IDLETIME,
        FIRSTLOGIN,
        LASTLOGOUT,
        SHIFTNAME,
        NOOFDAYS,
        DAYTYPE,
        AttendanceStatus
    )
    SELECT
        MasterEmployeeId,
        @StartDate,

        -- Logged Hours
        CASE
            WHEN DATENAME(WEEKDAY,@StartDate) IN ('Saturday','Sunday') THEN 0
            WHEN ABS(CHECKSUM(NEWID())) % 10 = 1 THEN 0 -- leave
            ELSE ROUND((7 + RAND(CHECKSUM(NEWID())) * 3),2)
        END,

        -- Active Hours
        CASE
            WHEN DATENAME(WEEKDAY,@StartDate) IN ('Saturday','Sunday') THEN 0
            WHEN ABS(CHECKSUM(NEWID())) % 10 = 1 THEN 0 -- leave
            WHEN ABS(CHECKSUM(NEWID())) % 5 = 1
                THEN ROUND((3 + RAND(CHECKSUM(NEWID())) * 2.5),2) -- below 6 hrs
            ELSE ROUND((6.5 + RAND(CHECKSUM(NEWID())) * 2.5),2)
        END,

        -- Time on system
        CASE
            WHEN DATENAME(WEEKDAY,@StartDate) IN ('Saturday','Sunday') THEN 0
            WHEN ABS(CHECKSUM(NEWID())) % 10 = 1 THEN 0
            ELSE ROUND((7 + RAND(CHECKSUM(NEWID())) * 2),2)
        END,

        ROUND((RAND(CHECKSUM(NEWID()))),2),
        ROUND((RAND(CHECKSUM(NEWID()))),2),

        DATEADD(HOUR,9,CAST(@StartDate AS DATETIME)),
        DATEADD(HOUR,18,CAST(@StartDate AS DATETIME)),

        'General',
        '1',

        CASE
            WHEN DATENAME(WEEKDAY,@StartDate) IN ('Saturday','Sunday')
                THEN 'Weekend'
            ELSE 'Weekday'
        END,

        -- Attendance
        CASE
            WHEN DATENAME(WEEKDAY,@StartDate) IN ('Saturday','Sunday')
                THEN 'Holiday'
            WHEN ABS(CHECKSUM(NEWID())) % 10 = 1
                THEN 'Leave'
            ELSE 'Present'
        END
    FROM MasterEmployee
    WHERE ROLE_NAME IN ('Employee', 'Team Lead')
      AND IsActive = 1
      AND IsDelete = 0;

    SET @StartDate = DATEADD(DAY,1,@StartDate);
END;

--test