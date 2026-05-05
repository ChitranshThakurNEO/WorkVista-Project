using System.Data;

namespace WorkVista.API.HelperLayer.Extensions
{
    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T item = new T();

                foreach (DataColumn column in table.Columns)
                {
                    var property = typeof(T).GetProperty(column.ColumnName);
                    if (property != null && row[column] != DBNull.Value)
                    {
                        //property.SetValue(item, Convert.ChangeType(row[column], property.PropertyType));
                        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var safeValue = Convert.ChangeType(row[column], propertyType);
                        property.SetValue(item, safeValue, null);
                    }
                }

                list.Add(item);
            }

            return list;
        }



        public static T ToModel<T>(this DataTable table) where T : new()
        {
            if (table.Rows.Count == 0)
                return default(T);

            var row = table.Rows[0];
            T item = new T();

            foreach (DataColumn column in table.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);
                if (property != null && row[column] != DBNull.Value)
                {
                    //property.SetValue(item, Convert.ChangeType(row[column], property.PropertyType));
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var safeValue = Convert.ChangeType(row[column], propertyType);
                    property.SetValue(item, safeValue, null);
                }
            }

            return item;
        }
    }
}
