
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
            string taroltEljarasName = "";

            Program program = new Program();
            Serializer serializer = new Serializer();

            //Tárolt eljárás és argumentumai
            GetTaroltEljarasokByName getTaroltEljarasokByName = new GetTaroltEljarasokByName(program.DatabaseConnection);
            List<TaroltEljaras> taroltEljarasLista = getTaroltEljarasokByName.GetTaroltEljaras(taroltEljarasName);


            Console.WriteLine(">>SERIALIZE");

            foreach (var taroltEljaras in taroltEljarasLista)
            {
                serializer.serialize(taroltEljaras, typeof(TaroltEljaras), "d:\\Server\\Visual_studio\\output_Xmls\\StorProcs\\" + taroltEljaras.Megnevezes + ".xml");
            }

            //Tábla és oszlopai

            string name = "tblParBank";
            GetTablaByName getTablaByName = new GetTablaByName(program.DatabaseConnection);
            List<Tabla> tablaLista = getTablaByName.GetTablaListByName(name);


            Console.WriteLine(">>SERIALIZE");

            foreach (var tabla in tablaLista)
            {
                serializer.serialize(tabla, typeof(Tabla), "d:\\Server\\Visual_studio\\output_Xmls\\arntesztTablak\\" + tabla.Megnevezes + ".xml");
            }
        }


    }
}
