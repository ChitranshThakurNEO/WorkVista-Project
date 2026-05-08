CREATE OR ALTER PROCEDURE sp_ManagerDashboard_GetEmployeeDayDetails
(
    @EmployeeId INT,
    @LoggedDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @TotalLoggedHours DECIMAL(10,2) = 0;

	SELECT
        @TotalLoggedHours = TOT_LOGGEDHOURS
    FROM EmployeeActivityLogSummary
    WHERE EmployeeId = @EmployeeId
      AND LoggedDate = @LoggedDate
      AND IsDelete = 0;

    ---------------------------------------------------
    -- RESULT SET 1 : SUMMARY
    ---------------------------------------------------
    SELECT
        EAS.EmployeeId,
        ME.FullName,
        EAS.LoggedDate,

        EAS.TOT_LOGGEDHOURS,
        EAS.TOT_ACTIVEHOURS,
        EAS.TOT_TIMEONSYSTEM,
        EAS.TOT_TIMEAWAYFROMSYSTEM,
        EAS.TOT_IDLETIME,

        EAS.FIRSTLOGIN,
        EAS.LASTLOGOUT,
        EAS.AttendanceStatus,

        ROUND(
            CASE
                WHEN EAS.TOT_LOGGEDHOURS = 0 THEN 0
                ELSE (EAS.TOT_ACTIVEHOURS * 100.0) / EAS.TOT_LOGGEDHOURS
            END
        , 2) AS ProductivityPercentage

    FROM EmployeeActivityLogSummary EAS
    INNER JOIN MasterEmployee ME
        ON EAS.EmployeeId = ME.MasterEmployeeId
    WHERE EAS.EmployeeId = @EmployeeId
      AND EAS.LoggedDate = @LoggedDate
      AND EAS.IsDelete = 0;


    ---------------------------------------------------
    -- RESULT SET 2 : APPLICATION BREAKDOWN
    ---------------------------------------------------
    SELECT
        MAC.SubCategoryName AS ApplicationName,
        MAC.ProcessName,

        SUM(MAL.DurationSeconds) AS DurationSeconds,
        ROUND(SUM(MAL.DurationSeconds) / 60.0, 2) AS DurationMinutes,
        ROUND(SUM(MAL.DurationSeconds) / 3600.0, 2) AS DurationHours,

        CASE
            WHEN MAC.IsProductive = 1 THEN 'Productive'
            WHEN MAC.IsNonProductive = 1 THEN 'Non-Productive'
			WHEN MAC.IsNoImpact = 1 AND (MAC.SubCategoryName in ('Idle', 'Locked')) THEN 'Idle'
            ELSE 'Neutral'
        END AS ProductivityType,

        MAC.ColorHex

    FROM MasterActivityLog MAL
    INNER JOIN MasterActivityCategory MAC
        ON MAL.CategoryId = MAC.MasterActivityCategoryId
    WHERE MAL.EmployeeId = @EmployeeId
      AND MAL.LogDate = @LoggedDate
      AND MAL.IsDelete = 0
    GROUP BY
        MAC.SubCategoryName,
        MAC.ProcessName,
        MAC.IsProductive,
        MAC.IsNonProductive,
		MAC.IsNoImpact,
        MAC.ColorHex
    --ORDER BY SUM(MAL.DurationSeconds) DESC;


    ---------------------------------------------------
-- RESULT SET 3 : CATEGORY PERCENTAGES
---------------------------------------------------
SELECT
    ROUND(
        SUM(
            CASE
                WHEN MAC.IsProductive = 1
                THEN MAL.DurationSeconds
                ELSE 0
            END
        ) / 3600.0 * 100 / NULLIF(@TotalLoggedHours, 0)
    , 2) AS ProductivePercentage,

    ROUND(
        SUM(
            CASE
                WHEN MAC.IsNonProductive = 1
                THEN MAL.DurationSeconds
                ELSE 0
            END
        ) / 3600.0 * 100 / NULLIF(@TotalLoggedHours, 0)
    , 2) AS NonProductivePercentage,

    ROUND(
        SUM(
            CASE
                WHEN MAC.IsNoImpact = 1
                     AND MAC.SubCategoryName NOT IN ('Idle', 'Locked')
                THEN MAL.DurationSeconds
                ELSE 0
            END
        ) / 3600.0 * 100 / NULLIF(@TotalLoggedHours, 0)
    , 2) AS NeutralPercentage,

    ROUND(
        SUM(
            CASE
                WHEN MAC.SubCategoryName IN ('Idle', 'Locked')
                THEN MAL.DurationSeconds
                ELSE 0
            END
        ) / 3600.0 * 100 / NULLIF(@TotalLoggedHours, 0)
    , 2) AS IdlePercentage

FROM MasterActivityLog MAL
INNER JOIN MasterActivityCategory MAC
    ON MAL.CategoryId = MAC.MasterActivityCategoryId
WHERE MAL.EmployeeId = @EmployeeId
  AND MAL.LogDate = @LoggedDate
  AND MAL.IsDelete = 0;
END;
GO