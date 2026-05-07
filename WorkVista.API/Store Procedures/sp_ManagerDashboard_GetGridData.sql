CREATE OR ALTER PROCEDURE sp_ManagerDashboard_GetGridData
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

    SELECT
        ME.MasterEmployeeId,
        ME.EmployeeCode,
        ME.FullName,
        ME.DESIGNATION,
        ME.ROLE_NAME,
        ME.GEO,
        ME.MANAGER1,
        ME.MANAGER2,

        EAS.LoggedDate,
        EAS.TOT_LOGGEDHOURS,
        EAS.TOT_ACTIVEHOURS,
        EAS.TOT_TIMEONSYSTEM,
        EAS.TOT_TIMEAWAYFROMSYSTEM,
        EAS.TOT_IDLETIME,
        EAS.AttendanceStatus

    FROM EmployeeActivityLogSummary EAS

    INNER JOIN MasterEmployee ME
        ON EAS.EmployeeId = ME.MasterEmployeeId

    WHERE
        ME.IsActive = 1
        AND ME.IsDelete = 0

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
        )

    ORDER BY
        ME.FullName,
        EAS.LoggedDate;
END;

--EXEC sp_ManagerDashboard_GetGridData
--    @ManagerEmployeeId = '1001',
--    @FromDate = '2026-04-07',
--    @ToDate = '2026-05-06',
--    @TeamLeadEmployeeId = NULL,
--    @Geo = NULL,
--    @SearchText = NULL;