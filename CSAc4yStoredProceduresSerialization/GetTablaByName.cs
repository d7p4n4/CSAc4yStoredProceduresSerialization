using d7p4n4Namespace.Final.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CSAc4yStoredProceduresSerialization
{
    public class GetTablaByName
    {
        private SqlConnection _Connection = new SqlConnection();
        public GetTablaByName(SqlConnection conn)
        {
            _Connection = conn;
        }
        public List<Tabla> GetTablaListByName(string tableName)
        {

            List<string> tablaNev = new List<string>();
            List<Tabla> tablaLista = new List<Tabla>();

            string queryString = "Select TABLE_NAME from FNOVUM2.information_schema.tables where table_name = @aTablaNev; ";

            using (SqlConnection connection = _Connection)
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                command.Parameters.Add("@aTablaNev", SqlDbType.VarChar).Value = tableName;
                if (connection.State.Equals("Closed"))
                {
                    connection.Open();
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine(reader.FieldCount);
                    while (reader.Read())
                    {
                        tablaNev.Add(reader.GetValue(0).ToString());
                        Console.WriteLine(reader.GetValue(0).ToString());
                    }
                }
            }
            int i = 0;
            foreach (var tabla in tablaNev)
            {
                if (i >= 0)
                {
                    Tabla tablaObjektum = new Tabla();
                    tablaObjektum.Megnevezes = tabla;
                    tablaObjektum.TablaOszlopLista = (this.GetParameters(tabla));

                    tablaLista.Add(tablaObjektum);
                }
                Console.WriteLine(i);
                i++;
            }
            _Connection.Close();
            return tablaLista;
        }

        public List<TablaOszlop> GetParameters(string name)
        {
            string sql = "Select COLUMN_NAME, DATA_TYPE From FNOVUM2.information_schema.COLUMNS WHERE TABLE_NAME = @aTablaNev; ";
            List<TablaOszlop> lista = new List<TablaOszlop>();
            SqlConnection connection = new SqlConnection("Data Source=arnteszt;Integrated Security=False;uid=Fejl_zsberces;pwd=Szappan60;Initial Catalog=FNOVUM2;");
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@aTablaNev", SqlDbType.VarChar).Value = name;
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    TablaOszlop tablaOszlop = new TablaOszlop();
                    tablaOszlop.Kod = reader.GetValue(0).ToString();
                    Console.WriteLine(tablaOszlop.Kod);
                    tablaOszlop.Adattipus = reader.GetValue(1).ToString();
                    lista.Add(tablaOszlop);
                }
            }

            return lista;
        }
    }
}
