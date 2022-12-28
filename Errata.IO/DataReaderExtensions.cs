using System.Data;

namespace System.IO.Errata
{
    public static class DataReaderExtensions
    {
        public static DataTable ToDataTable(this IDataReader reader)
        {
            var fieldCount = reader.FieldCount;
            DataTable dt = new DataTable();
            for (int i = 0; i < fieldCount; i++)
            {
                var name = reader.GetName(i);
                var ft = reader.GetFieldType(i);
                dt.Columns.Add(new DataColumn(name, ft));

            }
            while (reader.Read())
            {
                var row = dt.NewRow();
                for (int i = 0; i < fieldCount; i++)
                {
                    row[i] = reader.GetValue(i);
                }

                dt.Rows.Add(row);
            }
            return dt;
        }

        public static  string GetString(this IDataReader reader,  string key , String defaultValue = "")
        {
            var value = reader[key];

            if (value == DBNull.Value)
                return defaultValue;

            return (string)value;

        }

    }
}
