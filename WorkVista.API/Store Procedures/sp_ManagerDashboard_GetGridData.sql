CREATE OR ALTER PROCEDURE [dbo].[sp_ManagerDashboard_GetGridData]
(
    @ManagerEmployeeId NVARCHAR(100),
    @FromDate DATE,
    @ToDate DATE,
    @TeamLeadEmployeeId NVARCHAR(100) = NULL,
    @Geo NVARCHAR(50) = NULL,
    @SearchText NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF OBJECT_ID('tempdb..#EmployeeGridData') IS NOT NULL
        DROP TABLE #EmployeeGridData;

    SELECT
        ME.MasterEmployeeId AS EmployeeId,
        ME.EmployeeCode,
        ME.FullName AS EmployeeName,
        ME.DESIGNATION,
        ME.GEO,
        EAS.LoggedDate,
        EAS.TOT_ACTIVEHOURS,
        EAS.AttendanceStatus,
        EAS.DayType,

        CASE
            WHEN EAS.AttendanceStatus = 'Leave' THEN 'LV'
            WHEN EAS.DayType = 'Weekend'
                 OR EAS.AttendanceStatus = 'Holiday' THEN '-'
            ELSE
                RIGHT('0' + CAST(FLOOR(EAS.TOT_ACTIVEHOURS) AS VARCHAR), 2)
                + ':'
                + RIGHT(
                    '0' + CAST(
                        CAST((EAS.TOT_ACTIVEHOURS * 60) % 60 AS INT) AS VARCHAR
                    ), 2
                )
        END AS DisplayValue,

        CASE
            WHEN EAS.AttendanceStatus = 'Present'
            THEN CAST(EAS.TOT_ACTIVEHOURS * 60 AS INT)
            ELSE 0
        END AS ActiveMinutes

    INTO #EmployeeGridData
    FROM EmployeeActivityLogSummary EAS
    INNER JOIN MasterEmployee ME
        ON EAS.EmployeeId = ME.MasterEmployeeId
    WHERE
        ME.IsActive = 1
        AND ME.IsDelete = 0
        AND EAS.IsDelete = 0
        AND
        (
            ME.MANAGER1_EMPLOYEE_ID = @ManagerEmployeeId
            OR ME.MANAGER2_EMPLOYEE_ID = @ManagerEmployeeId
        )
        AND EAS.LoggedDate BETWEEN @FromDate AND @ToDate
        AND
        (
            @TeamLeadEmployeeId IS NULL
            OR ME.MANAGER1_EMPLOYEE_ID = @TeamLeadEmployeeId
            OR ME.EMPLOYEE_ID = @TeamLeadEmployeeId
        )
        AND
        (
            @Geo IS NULL
            OR ME.GEO = @Geo
        )
        AND
        (
            @SearchText IS NULL
            OR ME.FullName LIKE '%' + @SearchText + '%'
            OR ME.EmployeeCode LIKE '%' + @SearchText + '%'
        );

    ---------------------------------------------------
    -- RESULT SET 1 : EMPLOYEE GRID
    ---------------------------------------------------
    SELECT
        EmployeeId,
        EmployeeCode,
        EmployeeName,
        DESIGNATION,
        GEO,
        LoggedDate,
        DisplayValue,

        SUM(ActiveMinutes) OVER (PARTITION BY EmployeeId) AS TotalMinutes,

        CASE
            WHEN COUNT(
                CASE WHEN AttendanceStatus = 'Present' THEN 1 END
            ) OVER (PARTITION BY EmployeeId) = 0
            THEN 0
            ELSE
                SUM(ActiveMinutes) OVER (PARTITION BY EmployeeId)
                /
                COUNT(
                    CASE WHEN AttendanceStatus = 'Present' THEN 1 END
                ) OVER (PARTITION BY EmployeeId)
        END AS AvgMinutesPerDay

    FROM #EmployeeGridData
    ORDER BY EmployeeName, LoggedDate;


    ---------------------------------------------------
    -- RESULT SET 2 : TEAM AVERAGE
    ---------------------------------------------------
    SELECT
        LoggedDate,

        CASE
            WHEN COUNT(CASE WHEN AttendanceStatus = 'Present' THEN 1 END) = 0
            THEN '-'
            ELSE
                RIGHT(
                    '0' + CAST(
                        FLOOR(AVG(
                            CASE
                                WHEN AttendanceStatus = 'Present'
                                THEN TOT_ACTIVEHOURS
                            END
                        )) AS VARCHAR
                    ), 2
                )
                + ':'
                +
                RIGHT(
                    '0' + CAST(
                        CAST(
                            (
                                AVG(
                                    CASE
                                        WHEN AttendanceStatus = 'Present'
                                        THEN TOT_ACTIVEHOURS
                                    END
                                ) * 60
                            ) % 60 AS INT
                        ) AS VARCHAR
                    ), 2
                )
        END AS TeamAverageDisplay

    FROM #EmployeeGridData
    GROUP BY LoggedDate
    ORDER BY LoggedDate;

END
GO