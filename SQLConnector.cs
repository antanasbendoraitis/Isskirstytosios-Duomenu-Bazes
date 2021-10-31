using Išskirstytosios_duomenų_bazės.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Išskirstytosios_duomenų_bazės
{
    class SQLConnector
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public SQLConnector()
        {
            builder.DataSource = "serverforlearning.database.windows.net";
            builder.UserID = "ServerAdministrator";
            builder.Password = "Slibinukas1634";
            builder.InitialCatalog = "booksdb";
        }

        /// <summary>
        /// Metodas grąžinantis sąrašą lentelės įrašų arba atitinkamą lentelės įrašą
        /// </summary>
        /// <typeparam name="T">Objektas tipas</typeparam>
        /// <param name="sql">SQL užklausa</param>
        /// <param name="oneOrList">Grąžinti masyvą ar vieną įrašą, pasirinkti priklausomai pagal SQL užklausą</param>
        /// <returns></returns>
        public Object ReadRecords<T>(string sql, bool oneOrList) where T : new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (oneOrList)
                            {
                                List<T> temp = new List<T>();
                                while (reader.Read())
                                {
                                    temp.Add((T)Activator.CreateInstance(typeof(T), reader));
                                }
                                return temp;
                            }
                            else {
                                T temp = new T();
                                while (reader.Read())
                                {
                                    temp = (T)Activator.CreateInstance(typeof(T), reader);
                                }
                                return temp;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool InsertRecord<T>(string sql, T obj) where T: AbstractSQL {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        obj.AddParameters(command);

                        int rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool UpdateRecord<T>(string sql, T obj) where T : AbstractSQL
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        obj.AddParameters(command);

                        int rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool RemoveRecord(string sql)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        int rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}



/*public Object ReadRecords<T, R>(string sql) where R : new()
{
    try
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = "serverforlearning.database.windows.net";
        builder.UserID = "ServerAdministrator";
        builder.Password = "Slibinukas1634";
        builder.InitialCatalog = "booksdb";

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (typeof(R).IsAssignableFrom(typeof(T)))
                    {
                        R temp = new R();
                        while (reader.Read())
                        {
                            temp = (R)Activator.CreateInstance(typeof(T), reader);
                        }
                        return temp;
                    }
                    else
                    {
                        List<T> temp = new List<T>();
                        while (reader.Read())
                        {
                            temp.Add((T)Activator.CreateInstance(typeof(T), reader));
                        }
                        return temp;
                    }

                }
            }
        }
    }
    catch (SqlException e)
    {
        return null;
    }
}
*/