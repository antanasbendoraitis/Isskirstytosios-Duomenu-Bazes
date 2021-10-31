using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Išskirstytosios_duomenų_bazės.Model
{
    public abstract class AbstractSQL
    {
        public string SQLInsert { get; set; }
        public abstract void AddParameters(SqlCommand command);
    }
}
