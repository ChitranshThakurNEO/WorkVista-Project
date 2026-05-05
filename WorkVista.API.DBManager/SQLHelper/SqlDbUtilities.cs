using Serilog;
using System.Data;
using System.Data.SqlClient;

namespace WorkVista.API.DBManager.SQLHelper
{
    public class SqlDbUtilities
    {
        private string ConnectionString { get; }

        public SqlDbUtilities(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private IEnumerable<string> GetTableColumns(string tableName, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                string query = "SELECT COLUMN_NAME"
                                + " FROM INFORMATION_SCHEMA.COLUMNS"
                                + $" WHERE TABLE_NAME = '{tableName}'"
                                + " ORDER BY ORDINAL_POSITION;";

                var data = GetList(query, null, false, out errorMessage);

                if (data == null || data.Rows == null || data.Rows.Count == 0)
                {
                    return null;
                }

                var columns = (from DataRow row in data.Rows
                               select Convert.ToString(row["COLUMN_NAME"])
                               )
                               .ToList();

                return columns;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
        }

        public DataTable GetList(string query, Dictionary<string, object> parameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            return GetList(query, parameters, true, out errorMessage, commandTimeoutInSeconds);
        }

        public DataTable GetList(string query, Dictionary<string, object> parameters, bool validateSqlInjection, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (validateSqlInjection && SqlInjectionValidation.SqlInjectionFoundForSelectQuery(query))
                {
                    errorMessage = "Sql Injection found in query!";
                    return null;
                }

                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);

                connection.Open();

                //SqlDataAdapter dataAdapter = new SqlDataAdapter
                //{
                //    SelectCommand = new SqlCommand(query, connection)
                //};

                //DataTable table = new DataTable();
                //dataAdapter.Fill(table);

                DataTable table = null;

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)
                    {
                        foreach (var _parameter in parameters)
                        {
                            cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                        }
                    }

                    //cmd.Transaction = connection.BeginTransaction();
                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            dataAdapter.Fill(table);
                            //cmd.Transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        //cmd.Transaction.Rollback();
                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return null;
                    }
                }
                ;

                return table;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public Dictionary<string, object> Get(string query, Dictionary<string, object> parameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            return Get(query, parameters, true, out errorMessage, commandTimeoutInSeconds);
        }

        public Dictionary<string, object> Get(string query, Dictionary<string, object> parameters, bool validateSqlInjection, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (validateSqlInjection && SqlInjectionValidation.SqlInjectionFoundForSelectQuery(query))
                {
                    errorMessage = "Sql Injection found in query!";
                    return null;
                }

                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);

                connection.Open();

                //SqlDataAdapter dataAdapter = new SqlDataAdapter
                //{
                //    SelectCommand = new SqlCommand(query, connection)
                //};

                //DataTable table = new DataTable();
                //dataAdapter.Fill(table);

                DataTable table = null;

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)
                    {
                        foreach (var _parameter in parameters)
                        {
                            cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                        }
                    }

                    cmd.Transaction = connection.BeginTransaction();
                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            dataAdapter.Fill(table);
                            cmd.Transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();
                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return null;
                    }
                }
                ;

                if (table == null || table.Rows == null || table.Rows.Count == 0)
                {
                    return null;
                }

                var row = table.Rows[0];

                var data = table.Columns
                            .Cast<DataColumn>()
                            .ToDictionary(c => c.ColumnName, c => row[c]);

                return data;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data">Fields with Values (Key = FieldName, Value = FieldValue)</param>
        /// <returns></returns>
        public int Insert(string tableName, Dictionary<string, object> data, out string errorMessage, string queryBeforeInsert = "")
        {
            errorMessage = string.Empty;
            SqlConnection connection = null;

            try
            {
                if (data == null || data.Count == 0)
                {
                    errorMessage = "Please provide a data.";
                    return 0;
                }

                if (SqlInjectionValidation.SqlInjectionFound(data))
                {
                    errorMessage = "Sql Injection found in data!";
                    return 0;
                }

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    errorMessage = "Please provide a table name";
                    return 0;
                }

                var tableColumns = GetTableColumns(tableName, out errorMessage);

                if (tableColumns == null || tableColumns.Count() == 0)
                {
                    errorMessage = "Please provide valid columns.";
                    return 0;
                }

                var _data = data
                            .Where(d => tableColumns.Contains(d.Key))
                            .Select(d => d)
                            .ToDictionary(k => k.Key, v => v.Value);

                if (_data == null || _data.Count == 0)
                {
                    return 0;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection
                };

                List<string> columnNames = new List<string>();
                List<string> columnParameters = new List<string>();

                foreach (var d in _data)
                {
                    columnNames.Add(d.Key);
                    columnParameters.Add($"@{d.Key}");
                }

                string _columnNames = string.Join(",", columnNames);
                string _columnParameters = string.Join(",", columnParameters);

                if (!string.IsNullOrWhiteSpace(queryBeforeInsert))
                {
                    if (!queryBeforeInsert.EndsWith(";"))
                    {
                        queryBeforeInsert = $"{queryBeforeInsert};";
                    }
                }
                else
                {
                    queryBeforeInsert = string.Empty;
                }

                cmd.CommandText = $"{queryBeforeInsert}INSERT INTO {tableName}({_columnNames}) VALUES({_columnParameters});Select @@Identity;";
                //cmd.Prepare();

                foreach (var d in _data)
                {
                    cmd.Parameters.AddWithValue($"@{d.Key}", d.Value);
                }

                cmd.Transaction = connection.BeginTransaction();
                try
                {
                    //cmd.ExecuteNonQuery();
                    int autoRecordId = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Transaction.Commit();
                    return autoRecordId;
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();

                    errorMessage = ex.Message;
                    if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                    {
                        errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return 0;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data">List of <Fields with Values (Key = FieldName, Value = FieldValue)></Fields></param>
        /// <returns></returns>
        public bool InsertRanage(string tableName, List<Dictionary<string, object>> dataList, out string errorMessage)
        {
            errorMessage = string.Empty;
            SqlConnection connection = null;

            try
            {
                if (dataList == null || dataList.Count == 0)
                {
                    errorMessage = "Please provide list of data.";
                    return false;
                }

                if (SqlInjectionValidation.SqlInjectionFound(dataList))
                {
                    errorMessage = "Sql Injection found in data!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    errorMessage = "Please provide a table name";
                    return false;
                }

                var tableColumns = GetTableColumns(tableName, out errorMessage);

                if (tableColumns == null || tableColumns.Count() == 0)
                {
                    errorMessage = "Please provide valid columns.";
                    return false;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                try
                {
                    var _tableColumns = tableColumns.Select(d => d.ToLower()).ToList();

                    var firstRowData = dataList
                                    .FirstOrDefault()
                                    .Where(d => _tableColumns.Contains(d.Key.ToLower()))
                                    .Select(d => d)
                                    .ToDictionary(k => k.Key, v => v.Value);

                    if (firstRowData == null || firstRowData.Count == 0)
                    {
                        return false;
                    }


                    List<string> columnNames = new List<string>();
                    List<string> columnParameters = new List<string>();

                    foreach (var d in firstRowData)
                    {
                        columnNames.Add(d.Key);
                        columnParameters.Add($"@{d.Key}{{0}}");
                    }

                    string _columnNames = string.Join(",", columnNames);
                    string _columnParameters = string.Join(",", columnParameters);

                    string commandText = $"INSERT INTO {tableName}({_columnNames}) VALUES({_columnParameters})";

                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = connection
                    };

                    cmd.Transaction = connection.BeginTransaction();

                    int rowIndex = 0;
                    foreach (var data in dataList)
                    {
                        var _data = data
                                    .Where(d => _tableColumns.Contains(d.Key.ToLower()))
                                    .Select(d => d)
                                    .ToDictionary(k => k.Key, v => v.Value);

                        if (_data == null || _data.Count == 0)
                        {
                            return false;
                        }

                        cmd.CommandText = string.Format(commandText, rowIndex);
                        cmd.Parameters.Clear();

                        foreach (var d in _data)
                        {
                            cmd.Parameters.AddWithValue($"@{d.Key}{rowIndex}", d.Value);
                        }

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            cmd.Transaction.Rollback();

                            errorMessage = ex.Message;
                            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                            {
                                errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                            }

                            return false;
                        }

                        ++rowIndex;
                    }

                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                    {
                        errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                    }

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data">Fields with Values (Key = FieldName, Value = FieldValue)</param>
        /// <returns></returns>
        public bool Update(string primaryColumnName, int primaryColumnValue, string tableName, Dictionary<string, object> data, out string errorMessage)
        {
            errorMessage = string.Empty;
            SqlConnection connection = null;

            try
            {
                if (data == null || data.Count == 0)
                {
                    errorMessage = "Please provide a data.";
                    return false;
                }

                if (SqlInjectionValidation.SqlInjectionFound(data))
                {
                    errorMessage = "Sql Injection found in data!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    errorMessage = "Please provide a table name";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(primaryColumnName))
                {
                    errorMessage = "Please provide a primary column name";
                    return false;
                }

                if (primaryColumnValue <= 0)
                {
                    errorMessage = "Please provide a valid primary column value";
                    return false;
                }

                var tableColumns = GetTableColumns(tableName, out errorMessage);

                if (tableColumns == null || tableColumns.Count() == 0)
                {
                    errorMessage = "Please provide valid columns.";
                    return false;
                }

                var _data = data
                            .Where(d => tableColumns.Contains(d.Key))
                            .Select(d => d)
                            .ToDictionary(k => k.Key, v => v.Value);

                if (_data == null || _data.Count == 0)
                {
                    return false;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection
                };

                List<string> columnNamesAndParamaters = new List<string>();

                foreach (var d in _data)
                {
                    columnNamesAndParamaters.Add($"{d.Key} = @{d.Key}");
                }

                string _columnNamesAndParamaters = string.Join(",", columnNamesAndParamaters);

                cmd.CommandText = $"UPDATE {tableName} SET {_columnNamesAndParamaters} WHERE {primaryColumnName} = {primaryColumnValue}";
                //cmd.Prepare();

                foreach (var d in _data)
                {
                    cmd.Parameters.AddWithValue($"@{d.Key}", d.Value);
                }

                cmd.Transaction = connection.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    errorMessage = ex.Message;
                    if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                    {
                        errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                    }

                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ExectuteStoredProcedure(string storedProcedureName, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            return ExectuteStoredProcedure(storedProcedureName, null, out errorMessage, commandTimeoutInSeconds);
        }

        public DataTable ExectuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return null;
                }

                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                DataTable table = null;

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)
                    {
                        foreach (var _parameter in parameters)
                        {
                            if (_parameter.Value is List<string>)
                            {
                                var _values = _parameter.Value as List<string>;
                                var valueTable = new DataTable();
                                valueTable.Columns.Add("Value", typeof(string));
                                foreach (var value in _values)
                                {
                                    valueTable.Rows.Add(value);
                                }

                                var valyeTableParameter = new SqlParameter($"@{_parameter.Key}", SqlDbType.Structured);
                                valyeTableParameter.TypeName = "ValueTableType";
                                valyeTableParameter.Value = valueTable;

                                cmd.Parameters.Add(valyeTableParameter);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                            }
                        }

                    }

                    cmd.Transaction = connection.BeginTransaction();
                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            dataAdapter.Fill(table);
                            cmd.Transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();
                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }
                        Log.Error(errorMessage);
                        return null;
                    }

                }

                return table;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }
                Log.Error(errorMessage);
                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ExectuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters, out string errorMessage, out int totalRecords, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;
            totalRecords = 0;
            SqlConnection connection = null;
            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return null;
                }


                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);

                connection.Open();

                DataTable table = null;
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)
                    {

                        foreach (var _parameter in parameters)
                        {
                            cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                        }
                    }
                    SqlParameter totalRowsParam = cmd.Parameters.Add("@TotalRows", SqlDbType.Int);
                    totalRowsParam.Direction = ParameterDirection.Output;
                    cmd.Transaction = connection.BeginTransaction();

                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            dataAdapter.Fill(table);
                            cmd.Transaction.Commit();
                        }

                        totalRecords = (int)totalRowsParam.Value;
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();

                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return null;
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public Array ExectuteStoredProcedureToArray(string storedProcedureName, Dictionary<string, object> inParameters, string[,] outParameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;
            string Result;

            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return null;
                }

                if (outParameters == null)
                {
                    errorMessage = "Please specify out parameters.";
                    return null;
                }

                if (inParameters == null)
                {
                    errorMessage = "Please specify in parameters.";
                    return null;
                }


                if (SqlInjectionValidation.SqlInjectionFound(outParameters) || SqlInjectionValidation.SqlInjectionFound(inParameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);

                connection.Open();

                DataTable table = null;
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    //input parameters  
                    //for (int i = 0; i < inParameters.Length / 2; i++)
                    //{
                    //    cmd.Parameters.AddWithValue(inParameters[i, 0], inParameters[i, 1]);
                    //}


                    foreach (var _parameter in inParameters)
                    {
                        cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                    }

                    //outparameters  
                    for (int i = 0; i < outParameters.Length / 2; i++)
                    {
                        if (outParameters[i, 1] == "SqlDbType.VarChar")
                        {
                            cmd.Parameters.Add(outParameters[i, 0], SqlDbType.VarChar, -1);
                        }
                        else if (outParameters[i, 1] == "SqlDbType.DateTime")
                        {
                            cmd.Parameters.Add(outParameters[i, 0], SqlDbType.DateTime);
                        }
                        else if (outParameters[i, 1] == "SqlDbType.Bit")
                        {
                            cmd.Parameters.Add(outParameters[i, 0], SqlDbType.Bit);
                        }
                        else
                        {
                            cmd.Parameters.Add(outParameters[i, 0], SqlDbType.Int);
                        }
                        cmd.Parameters[outParameters[i, 0]].Direction = ParameterDirection.Output;
                    }

                    cmd.Transaction = connection.BeginTransaction();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                        string[,] Temparray = new string[outParameters.Length / 2, 2];

                        for (int i = 0; i < outParameters.Length / 2; i++)
                        {
                            Result = Convert.ToString(cmd.Parameters[outParameters[i, 0]].Value);
                            Temparray[i, 0] = outParameters[i, 0];
                            Temparray[i, 1] = Result;
                        }

                        return Temparray;
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();

                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return null;
                    }


                }

                return null;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataSet ExectuteStoredProcedureForMultipleTables(string storedProcedureName, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            return ExectuteStoredProcedureForMultipleTables(storedProcedureName, null, out errorMessage, commandTimeoutInSeconds);
        }

        public DataSet ExectuteStoredProcedureForMultipleTables(string storedProcedureName, Dictionary<string, object> parameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return null;
                }

                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);

                connection.Open();

                DataSet dataSet = null;
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)
                    {
                        foreach (var _parameter in parameters)
                        {
                            cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                        }

                    }

                    cmd.Transaction = connection.BeginTransaction();

                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            cmd.Transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();

                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return null;
                    }
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public bool ExecuteNonQuery(string storedProcedureName, Dictionary<string, object> parameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return false;
                }

                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return false;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)
                    {
                        foreach (var _parameter in parameters)
                        {
                            if (_parameter.Value is List<string>)
                            {
                                var _values = _parameter.Value as List<string>;
                                var valueTable = new DataTable();
                                valueTable.Columns.Add("Value", typeof(string));
                                foreach (var value in _values)
                                {
                                    valueTable.Rows.Add(value);
                                }

                                var valyeTableParameter = new SqlParameter($"@{_parameter.Key}", SqlDbType.Structured);
                                valyeTableParameter.TypeName = "ValueTableType";
                                valyeTableParameter.Value = valueTable;

                                cmd.Parameters.Add(valyeTableParameter);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@{_parameter.Key}", _parameter.Value);
                            }
                        }

                    }

                    cmd.Transaction = connection.BeginTransaction();
                    try
                    {
                        int noOfRecordsEffected = cmd.ExecuteNonQuery();
                        cmd.Transaction.Commit();
                        return (noOfRecordsEffected > 0);
                    }
                    catch (Exception ex)
                    {
                        cmd.Transaction.Rollback();
                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ExectuteTable(string tableName, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    errorMessage = "Please specify table name.";
                    return null;
                }

                if (SqlInjectionValidation.SqlInjectionFound(new List<string> { tableName }))
                {
                    errorMessage = "Sql Injection found in table name!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                DataTable table = null;

                using (SqlCommand cmd = new SqlCommand($"select * from {tableName}", connection))
                {
                    //cmd.CommandType = CommandType.TableDirect;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    try
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            dataAdapter.Fill(table);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                        {
                            errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                        }

                        return null;
                    }

                }

                return table;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ExecuteNonQueryWithOutParamenter(string storedProcedureName, Dictionary<string, object> parameters, IEnumerable<string> outParameters, out string errorMessage, int commandTimeoutInSeconds = 30)
        {
            errorMessage = string.Empty;

            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return null;
                }

                bool hasParameters = (parameters != null && parameters.Count != 0);
                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))
                {
                    errorMessage = "Sql Injection found in parameters!";
                    return null;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                DataTable table = null;
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (hasParameters)
                    {
                        var _outParameters = outParameters.Select(d => d.ToLower()).ToList();

                        foreach (var _paramter in parameters)
                        {
                            cmd.Parameters.AddWithValue($"@{_paramter.Key}", _paramter.Value);

                            if (_outParameters.Contains(_paramter.Key.ToLower()))
                            {
                                cmd.Parameters[$"@{_paramter.Key}"].Direction = ParameterDirection.Output;
                            }

                        }
                    }

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                    {
                        table = new DataTable();
                        dataAdapter.Fill(table);
                    }


                    foreach (var outParameter in outParameters)
                    {
                        parameters[outParameter] = cmd.Parameters[$"@{outParameter}"].Value;
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = string.Format("{0}{1}{2}", errorMessage, Environment.NewLine, ex.InnerException.Message);
                }

                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public int ExecuteStoredProcedureToInt(string storedProcedureName, Dictionary<string, object> inParameters, out string errorMessage, int commandTimeoutInSeconds = 300)
        {
            errorMessage = string.Empty;
            int result = 0;
            SqlConnection connection = null;

            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                {
                    errorMessage = "Please specify stored procedure name.";
                    return 0;
                }

                if (inParameters == null)
                {
                    errorMessage = "Please specify input parameters.";
                    return 0;
                }

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    // Add input parameters
                    foreach (var parameter in inParameters)
                    {
                        cmd.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value);
                    }

                    // Execute the command and retrieve the result
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}{ex.InnerException.Message}";
                }
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return result;
        }

        public int ExecuteStoredProcedureToIntwithoutparam(string storedProcedureName, Dictionary<string, object> parameters, out string errorMessage, int commandTimeoutInSeconds = 300)

        {

            errorMessage = string.Empty;

            int result = 0;

            SqlConnection connection = null;

            try

            {

                if (string.IsNullOrWhiteSpace(storedProcedureName))

                {

                    errorMessage = "Please specify stored procedure name.";

                    return 0;

                }

                bool hasParameters = (parameters != null && parameters.Count != 0);

                if (hasParameters && SqlInjectionValidation.SqlInjectionFound(parameters))

                {

                    errorMessage = "Sql Injection found in parameters!";

                    return 0;

                }


                connection = new SqlConnection(ConnectionString);

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandTimeout = commandTimeoutInSeconds <= 0 ? 300 : commandTimeoutInSeconds;

                    if (hasParameters)

                    {

                        // Add input parameters

                        foreach (var parameter in parameters)

                        {

                            cmd.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value);

                        }

                    }

                    // Execute the command and retrieve the result

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                }

            }

            catch (Exception ex)

            {

                errorMessage = ex.Message;

                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))

                {

                    errorMessage = $"{errorMessage}{Environment.NewLine}{ex.InnerException.Message}";

                }

            }

            finally

            {

                if (connection != null && connection.State == ConnectionState.Open)

                {

                    connection.Close();

                }

            }

            return result;

        }
    }
}
