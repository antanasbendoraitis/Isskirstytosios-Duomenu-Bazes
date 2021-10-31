using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Išskirstytosios_duomenų_bazės.Model
{
    class BookSQL : AbstractSQL
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Cover { get; set; }
        public float Mass { get; set; }

        public BookSQL()
        {

        }

        public BookSQL(SqlDataReader reader)
        {
            Isbn = reader.GetString(0);
            Title = reader.GetString(1);
            Price = reader.GetDecimal(2);
            Cover = reader.GetInt32(3);
            Mass = float.Parse(reader[4].ToString());
        }

        public override string ToString()
        {
            return String.Format(" {0,8} | {1,80} | {2,8} | {3,8} | {4,8}", Isbn, Title, Price, Cover, Mass);
        }

        public override void AddParameters(SqlCommand command)
        {
            command.Parameters.Add("@isbn", SqlDbType.NVarChar).Value = Isbn;
            command.Parameters.Add("@title", SqlDbType.NVarChar).Value = Title;
            command.Parameters.Add("@price", SqlDbType.Decimal).Value = Price;
            command.Parameters.Add("@cover", SqlDbType.Int).Value = Cover;
            command.Parameters.Add("@mass", SqlDbType.Float).Value = Mass;
        }
    }
}
