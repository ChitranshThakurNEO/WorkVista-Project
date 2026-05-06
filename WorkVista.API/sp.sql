CREATE OR ALTER PROCEDURE sp_ManagerDashboard_GetSummary
(
    @ManagerEmployeeId NVARCHAR(100),
    @FromDate DATE,
    @ToDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(DISTINCT ME.MasterEmployeeId) AS TeamMembers,

        ROUND(AVG(EAS.TOT_ACTIVEHOURS), 2) AS AvgDailyActiveHours,

        ROUND(
            CASE
                WHEN SUM(EAS.TOT_LOGGEDHOURS) = 0 THEN 0
                ELSE (SUM(EAS.TOT_ACTIVEHOURS) * 100.0) / SUM(EAS.TOT_LOGGEDHOURS)
            END
        , 2) AS ProductivePercentage,

        SUM(CASE WHEN EAS.TOT_ACTIVEHOURS < 6 AND EAS.AttendanceStatus = 'Present' THEN 1 ELSE 0 END)
            AS Below6HoursDays,

        SUM(CASE WHEN EAS.AttendanceStatus = 'Leave' THEN 1 ELSE 0 END)
            AS LeaveDays

    FROM EmployeeActivityLogSummary EAS
    INNER JOIN MasterEmployee ME
        ON EAS.EmployeeId = ME.MasterEmployeeId

    WHERE
        ME.IsActive = 1
        AND ME.IsDelete = 0
        AND (
            ME.MANAGER1_EMPLOYEE_ID = @ManagerEmployeeId
            OR ME.MANAGER2_EMPLOYEE_ID = @ManagerEmployeeId
        )
        AND EAS.LoggedDate BETWEEN @FromDate AND @ToDate;
END;