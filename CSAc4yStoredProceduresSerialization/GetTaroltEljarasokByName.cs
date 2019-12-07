using d7p4n4Namespace.Final.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CSAc4yStoredProceduresSerialization
{
    class GetTaroltEljarasokByName
    {
        private SqlConnection _Connection = new SqlConnection();
        public GetTaroltEljarasokByName(SqlConnection conn)
        {
            _Connection = conn;
        }
        public List<TaroltEljaras> GetTaroltEljaras(string name)
        {
            string taroltEljarasName = name;
            List<string> storedProceduresName = new List<string>();
            List<TaroltEljaras> taroltEljarasLista = new List<TaroltEljaras>();

            string queryString = "select SPECIFIC_NAME from information_schema.routines where routine_type = 'PROCEDURE' and(SPECIFIC_NAME = @aTaroltEljarasNev)";

            using (SqlConnection connection = _Connection)
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                command.Parameters.Add("@aTaroltEljarasNev", SqlDbType.VarChar).Value = taroltEljarasName;
                //connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine(reader.FieldCount);
                    while (reader.Read())
                    {
                        storedProceduresName.Add(reader[0].ToString());

                    }
                }
            }

            foreach (var storProc in storedProceduresName)
            {
                TaroltEljaras taroltEljaras = new TaroltEljaras();
                taroltEljaras.Megnevezes = storProc;
                taroltEljaras.ArgumentumLista = (this.GetParameters(storProc));

                taroltEljarasLista.Add(taroltEljaras);
            }

            return taroltEljarasLista;

        }

        public List<TaroltEljarasArgumentum> GetParameters(string name)
        {
            Program program = new Program();
            List<TaroltEljarasArgumentum> lista = new List<TaroltEljarasArgumentum>();
            SqlConnection connection = program.DatabaseConnection;
            SqlCommand cmd = new SqlCommand(name, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            foreach (SqlParameter p in cmd.Parameters)
            {
                TaroltEljarasArgumentum taroltEljarasArgumentum = new TaroltEljarasArgumentum();
                taroltEljarasArgumentum.BelsoNev = p.ParameterName;
                taroltEljarasArgumentum.Adattipus = p.SqlDbType.ToString();
                taroltEljarasArgumentum.Iranya = p.Direction.ToString();
                lista.Add(taroltEljarasArgumentum);

            }

            return lista;
        }
    }
}
