
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using d7p4n4Namespace.Final.Class;

namespace CSAc4yStoredProceduresSerialization
{
    class TablaProgram
    {/*

        #region konstans

        private static readonly log4net.ILog _naplo = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string APPSETTINGS_SERVER = "SERVER";
        private const string APPSETTINGS_USER = "USER";
        private const string APPSETTINGS_PASSWORD = "PASSWORD";
        private const string APPSETTINGS_DATABASE = "DATABASE";
        private const string APPSETTINGS_CONNECTIONPARAMETER = "CONNECTIONPARAMETER";

        #endregion // konstans

        public SqlConnection DatabaseConnection { get; set; }

        public TablaProgram()
        {

            try
            {

                DatabaseConnection = SetupDatabaseConnection();

                DatabaseConnection.Open();

                if (!DatabaseConnection.State.Equals(ConnectionState.Open))
                    throw new Exception("Nem kapcsolódik az adatbázishoz!");

            }
            catch (Exception exception)
            {
                _naplo.Error(exception.Message);
                //_naplo.Error(exception.Message+"\n"+ exception.StackTrace);

            }

        }

        public string GetAsXml(Object aObject)
        {

            string result = "";

            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(aObject.GetType());

                using (StringWriter stringWriter = new StringWriter())
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        xmlSerializer.Serialize(xmlWriter, aObject);
                    }

                    result = stringWriter.ToString();
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }

            return result;

        } // GetAsXml

        private SqlConnection SetupDatabaseConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[APPSETTINGS_CONNECTIONPARAMETER].ConnectionString);
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            Serializer serializer = new Serializer();

            List<string> tablaNev = new List<string>();
            List<Tabla> tablaLista = new List<Tabla>();

            string queryString = "Select TABLE_NAME from FNOVUM2.information_schema.tables where table_name like 'tblUgy%' or table_name like 'tblPar%'; ";

            using (SqlConnection connection = new SqlConnection("Data Source=arnteszt;Integrated Security=False;uid=Fejl_zsberces;pwd=Szappan60;Initial Catalog=FNOVUM2;"))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
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
                    TablaProgram program = new TablaProgram();

                    Tabla tablaObjektum = new Tabla();
                    tablaObjektum.Megnevezes = tabla;
                    tablaObjektum.TablaOszlopLista = (program.GetParameters(tabla));

                    tablaLista.Add(tablaObjektum);
                }
                Console.WriteLine(i);
                i++;
            }

            Console.WriteLine(">>SERIALIZE");

            foreach (var tabla in tablaLista)
            {
                serializer.serialize(tabla, tabla.Megnevezes, typeof(Tabla));
            }

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
        }*/
    }
}
