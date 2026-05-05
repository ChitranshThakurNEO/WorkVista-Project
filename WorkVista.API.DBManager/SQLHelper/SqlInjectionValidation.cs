namespace WorkVista.API.DBManager.SQLHelper
{
    public class SqlInjectionValidation
    {
        public static bool SqlInjectionFound(Dictionary<string, object> data)
        {
            if (data == null || data.Count == 0)
            {
                return false;
            }

            var _data = data
                        .Where(d =>
                        d.GetType() == typeof(string)
                        && !string.IsNullOrWhiteSpace(Convert.ToString(d.Value)))
                        .Select(d => Convert.ToString(d.Value).ToLower())
                        .ToList();

            if (_data == null || _data.Count == 0)
            {
                return false;
            }

            return SqlInjectionFound(_data);
        }

        public static bool SqlInjectionFound(string[,] data)
        {
            if (data == null)
            {
                return false;
            }

            var _data = data.Cast<string>().ToList();

            if (_data == null || _data.Count == 0)
            {
                return false;
            }

            return SqlInjectionFound(_data);
        }

        public static bool SqlInjectionFound(List<Dictionary<string, object>> data)
        {
            if (data == null || data.Count == 0)
            {
                return false;
            }

            foreach (var d in data)
            {
                var _data = d
                            .Where(x =>
                            x.GetType() == typeof(string)
                            && !string.IsNullOrWhiteSpace(Convert.ToString(x.Value)))
                            .Select(x => Convert.ToString(x.Value).ToLower())
                            .ToList();

                if (_data != null && _data.Count != 0)
                {
                    if (SqlInjectionFound(_data))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool SqlInjectionFound(IEnumerable<string> data)
        {
            if (data == null || data.Count() == 0)
            {
                return false;
            }

            var _data = data
                        .Where(d => !string.IsNullOrWhiteSpace(d))
                        .Select(d => d.ToLower())
                        .ToList();

            if (_data == null || _data.Count == 0)
            {
                return false;
            }

            bool isSqlInjectionFound = false;
            foreach (var d in _data)
            {
                isSqlInjectionFound = d.Contains("insert into")
                                    || (d.Contains("insert") && d.Contains(" values "))
                                    || (d.Contains("insert") && d.Contains(" select "))
                                    || (d.Contains("select") && d.Contains(" into "))
                                    || (d.Contains("select") && d.Contains(" from "))
                                    || (d.Contains("delete") && d.Contains(" from "))
                                    || d.Contains("drop table")
                                    || d.Contains("drop database")
                                    || d.Contains("show database")
                                    || d.Contains("create table")
                                    || d.Contains("create procedure")
                                    || d.Contains("create view")
                                    || d.Contains("create trigger")
                                    || d.Contains("create database")
                                    || d.Contains("create user");

                if (isSqlInjectionFound)
                {
                    break;
                }

            }

            return isSqlInjectionFound;
        }


        public static bool SqlInjectionFoundForSelectQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return false;
            }

            bool isSqlInjectionFound = query.Contains("insert into")
                                    || (query.Contains("insert") && query.Contains(" values "))
                                    || (query.Contains("insert") && query.Contains(" select "))
                                    || (query.Contains("select") && query.Contains(" into "))
                                    //|| (query.Contains("select") && query.Contains(" from "))
                                    || (query.Contains("delete") && query.Contains(" from "))
                                    || query.Contains("drop table")
                                    || query.Contains("drop database")
                                    || query.Contains("show database")
                                    || query.Contains("create table")
                                    || query.Contains("create procedure")
                                    || query.Contains("create view")
                                    || query.Contains("create trigger")
                                    || query.Contains("create database")
                                    || query.Contains("create user");

            return isSqlInjectionFound;
        }
    }
}
