
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
    class Program
    {

        #region konstans

        private static readonly log4net.ILog _naplo = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string APPSETTINGS_SERVER = "SERVER";
        private const string APPSETTINGS_USER = "USER";
        private const string APPSETTINGS_PASSWORD = "PASSWORD";
        private const string APPSETTINGS_DATABASE = "DATABASE";
        private const string APPSETTINGS_CONNECTIONPARAMETER = "CONNECTIONPARAMETER";

        #endregion // konstans

        public SqlConnection DatabaseConnection { get; set; }

        public Program()
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
            Program program = new Program();
            Serializer serializer = new Serializer();
            
            List<string> storedProceduresName = new List<string>();
            List<TaroltEljaras> taroltEljarasLista = new List<TaroltEljaras>();

            string queryString = "select SPECIFIC_NAME from information_schema.routines where routine_type = 'PROCEDURE' and(SPECIFIC_NAME like 'Ugy%')";

            using (SqlConnection connection = program.DatabaseConnection)
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
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
                taroltEljaras.ArgumentumLista = (program.GetParameters(storProc));

                taroltEljarasLista.Add(taroltEljaras);
            }

            foreach(var TE in taroltEljarasLista)
            {
                serializer.serialize(TE, TE.Megnevezes, typeof(TaroltEljaras));
            }

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
