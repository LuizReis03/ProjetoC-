using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;

public class Access<T>
    where T : new()
{
    private SqlConnection conn;

    public Access()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.InitialCatalog = "projetop";
        builder.IntegratedSecurity = true;
        builder.DataSource = @"SNCCH01LABF120\TEW_SQLEXPRESS";
        conn = new SqlConnection(builder.ConnectionString);
        conn.Open();
    }

    public List<T> All
    {
        get
        {
            List<T> list = new List<T>();

            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from " + typeof(T).Name;
            var reader = comm.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);

            foreach(DataRow row in dt.Rows){
                T obj = new T();
                foreach (var prop in typeof(T).GetProperties())
                    prop.SetValue(obj, row[prop.Name]);
                list.Add(obj);
            }

            return list;     
        }
    }

    public void Add(T obj)
    {
        string fields = "",
               values = "";

        foreach (var prop in typeof(T).GetProperties())
        {
            if (prop.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                continue;
            fields += prop.Name + ", ";
            if (prop.PropertyType == typeof(string))
            {
                values += "'" + prop.GetValue(obj)?.ToString() + "', ";
            }
            else
            {
                values += prop.GetValue(obj)?.ToString() + ", ";
            }
        }
        fields = fields.Substring(0, fields.Length - 2);
        values = values.Substring(0, values.Length - 2);

        
        SqlCommand comm = new SqlCommand();
        comm.Connection = conn;
        comm.CommandType = CommandType.Text;
        comm.CommandText = $"insert {typeof(T).Name} ({fields}) values ({values})";
        comm.ExecuteNonQuery();
    }

    ~Access(){
        conn.Close();
    }
}