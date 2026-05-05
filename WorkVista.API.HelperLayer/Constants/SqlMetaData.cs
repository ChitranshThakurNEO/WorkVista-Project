namespace WorkVista.API.HelperLayer.Constants
{
    public class SqlMetaData
    {
        public partial struct WorkVistaStoreProcedures
        {
            #region ================= MASTER DICTIONARY =================
            public const string WorkVista_GetAllMasterDictionaries = "sp_MasterDictionary_GetAll";
            public const string WorkVista_GetMasterDictionaryDetailsById = "sp_MasterDictionary_GetById";
            public const string WorkVista_UpsertMasterDictionaryDetails = "sp_MasterDictionary_Upsert";
            public const string WorkVista_DeleteMasterDictionaryDetails = "sp_MasterDictionary_Delete";
            #endregion

            #region ================= MASTER SHIFT =================
            public const string WorkVista_GetAllMasterShifts = "sp_MasterShift_GetAll";
            public const string WorkVista_GetMasterShiftDetailsById = "sp_MasterShift_GetById";
            public const string WorkVista_UpsertMasterShiftDetails = "sp_MasterShift_Upsert";
            public const string WorkVista_DeleteMasterShiftDetails = "sp_MasterShift_Delete";
            #endregion

            #region ================= MASTER EMPLOYEE SYSTEM INFO =================
            public const string WorkVista_GetAllMasterEmployeeSystemInfos = "sp_MasterEmployeeSystemInfo_GetAll";
            public const string WorkVista_GetMasterEmployeeSystemInfoById = "sp_MasterEmployeeSystemInfo_GetById";
            public const string WorkVista_UpsertMasterEmployeeSystemInfo = "sp_MasterEmployeeSystemInfo_Upsert";
            public const string WorkVista_DeleteMasterEmployeeSystemInfo = "sp_MasterEmployeeSystemInfo_Delete";
            #endregion

            #region ================= MASTER ALERT =================
            public const string WorkVista_GetAllMasterAlerts = "sp_MasterAlert_GetAll";
            public const string WorkVista_GetMasterAlertById = "sp_MasterAlert_GetById";
            public const string WorkVista_UpsertMasterAlert = "sp_MasterAlert_Upsert";
            public const string WorkVista_DeleteMasterAlert = "sp_MasterAlert_Delete";
            public const string WorkVista_MarkMasterAlertAsRead = "sp_MasterAlert_MarkAsRead";
            public const string WorkVista_ResolveMasterAlert = "sp_MasterAlert_Resolve";
            #endregion

            #region ================= MASTER ACTIVITY TYPE =================
            public const string WorkVista_GetAllMasterActivityTypes = "sp_MasterActivityType_GetAll";
            public const string WorkVista_GetMasterActivityTypeById = "sp_MasterActivityType_GetById";
            public const string WorkVista_UpsertMasterActivityType = "sp_MasterActivityType_Upsert";
            public const string WorkVista_DeleteMasterActivityType = "sp_MasterActivityType_Delete";
            #endregion

            #region ================= MASTER ACTIVITY CATEGORY =================
            public const string WorkVista_GetAllMasterActivityCategories = "sp_MasterActivityCategory_GetAll";
            public const string WorkVista_GetMasterActivityCategoryById = "sp_MasterActivityCategory_GetById";
            public const string WorkVista_UpsertMasterActivityCategory = "sp_MasterActivityCategory_Upsert";
            public const string WorkVista_DeleteMasterActivityCategory = "sp_MasterActivityCategory_Delete";
            #endregion

            #region ================= MASTER EMPLOYEE SESSION =================
            public const string WorkVista_GetAllMasterEmployeeSessions = "sp_MasterEmployeeSession_GetAll";
            public const string WorkVista_GetMasterEmployeeSessionById = "sp_MasterEmployeeSession_GetById";

            public const string WorkVista_LoginEmployeeSession = "sp_MasterEmployeeSession_Login";
            public const string WorkVista_LogoutEmployeeSession = "sp_MasterEmployeeSession_Logout";

            public const string WorkVista_DeleteMasterEmployeeSession = "sp_MasterEmployeeSession_Delete";
            public const string WorkVista_ToggleMasterEmployeeSessionActive = "sp_MasterEmployeeSession_ToggleActive";
            #endregion

            #region ================= MASTER RATING APPLICATION =================
            public const string WorkVista_GetAllMasterRatingApplications = "sp_MasterRatingApplication_GetAll";
            public const string WorkVista_GetMasterRatingApplicationById = "sp_MasterRatingApplication_GetById";
            public const string WorkVista_UpsertMasterRatingApplication = "sp_MasterRatingApplication_Upsert";
            public const string WorkVista_DeleteMasterRatingApplication = "sp_MasterRatingApplication_Delete";
            public const string WorkVista_ToggleMasterRatingApplicationActive = "sp_MasterRatingApplication_ToggleActive";
            #endregion

            #region ================= MASTER APPLICATION LICENSE =================
            public const string WorkVista_GetAllMasterApplicationLicenses = "sp_MasterApplicationLicense_GetAll";
            public const string WorkVista_GetMasterApplicationLicenseById = "sp_MasterApplicationLicense_GetById";
            public const string WorkVista_UpsertMasterApplicationLicense = "sp_MasterApplicationLicense_Upsert";
            public const string WorkVista_DeleteMasterApplicationLicense = "sp_MasterApplicationLicense_Delete";
            #endregion

            #region ================= MASTER EMPLOYEE SHIFT =================
            public const string WorkVista_GetAllEmployeeShifts = "sp_EmployeeShift_GetAll";
            public const string WorkVista_GetEmployeeShiftById = "sp_EmployeeShift_GetById";
            public const string WorkVista_UpsertEmployeeShift = "sp_EmployeeShift_Upsert";
            public const string WorkVista_DeleteEmployeeShift = "sp_EmployeeShift_Delete";
            #endregion

            #region ================= License Assignment =================
            public const string WorkVista_GetAllLicenseAssignments = "sp_LicenseAssignment_GetAll";
            public const string WorkVista_GetLicenseAssignmentById = "sp_LicenseAssignment_GetById";
            public const string WorkVista_UpsertLicenseAssignment = "sp_LicenseAssignment_Upsert";
            public const string WorkVista_ReleaseLicenseAssignment = "sp_LicenseAssignment_Release";
            public const string WorkVista_DeleteLicenseAssignment = "sp_LicenseAssignment_Delete";
            #endregion

            #region ================= Master Activity Log =================
            public const string WorkVista_GetAllMasterActivityLogs = "sp_MasterActivityLog_GetAll";
            public const string WorkVista_GetMasterActivityLogById = "sp_MasterActivityLog_GetById";
            public const string WorkVista_UpsertMasterActivityLog = "sp_MasterActivityLog_Upsert";
            public const string WorkVista_DeleteMasterActivityLog = "sp_MasterActivityLog_Delete";

            public const string WorkVista_StartMasterActivityLog = "sp_MasterActivityLog_Start";
            public const string WorkVista_StopMasterActivityLog = "sp_MasterActivityLog_Stop";

            public const string WorkVista_GetLogsByEmployee = "sp_MasterActivityLog_GetByEmployee";
            public const string WorkVista_GetLogsByEmployeeAndDate = "sp_MasterActivityLog_GetByEmployeeAndDate";
            #endregion

            #region ================= Master Employee DEX =================
            public const string WorkVista_GetAllMasterEmployeeDEX = "sp_MasterEmployeeDEX_GetAll";
            public const string WorkVista_GetMasterEmployeeDEXById = "sp_MasterEmployeeDEX_GetById";
            #endregion

            #region ================= Master Activity Log Summary =================
            public const string WorkVista_GetAllEmployeeActivityLogSummary = "sp_EmployeeActivityLogSummary_GetAll";
            public const string WorkVista_GetEmployeeActivityLogSummaryById = "sp_EmployeeActivityLogSummary_GetById";
            public const string WorkVista_GetEmployeeActivityLogSummaryByEmployeeDate = "sp_EmployeeActivityLogSummary_GetByEmployeeDate";
            public const string WorkVista_UpsertEmployeeActivityLogSummary = "sp_EmployeeActivityLogSummary_Upsert";
            public const string WorkVista_DeleteEmployeeActivityLogSummary = "sp_EmployeeActivityLogSummary_Delete";
            #endregion

            #region ================= Master Audit Log =================
            public const string WorkVista_AddMasterAuditLog = "sp_MasterAuditLog_Add";
            #endregion
        }

        public partial struct AIgniteStoreProcedures
        {
            #region ================= BANDWIDTH CRITERIA =================
            public const string AIgnite_GetAllBandwidthCriteria = "sp_AIgnite_BandwidthCriteria_GetAll";
            #endregion

            #region ================= MASTER MENU ================= 
            public const string AIgnite_GetAllMasterMenu = "sp_AIgnite_MasterMenu_GetAll";
            #endregion

            #region ================= MASTER APP =================
            public const string AIgnite_GetAllMasterAppByMenuId = "sp_AIgnite_MasterApp_GetAllByMenuId";
            #endregion

            #region ================= MASTER APP STEP =================
            public const string AIgnite_GetAllMasterAppStepByMasterAppId = "sp_AIgnite_MasterAppStep_GetAllByMasterAppId";
            #endregion
        }
    }
}
