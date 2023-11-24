using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Base_de_Datos
{
    public class Connect_DB
    {
        private string connectionString;

        public Connect_DB()
        {
            // Initialize with default connection string
            InitializeConnectionString();
        }

        private void InitializeConnectionString()
        {
            // Default connection details
            string server = "100.104.98.58";
            string database = "ProyectoDisApp";
            string user = "emi";
            string password = "Emi200307";

            // Form the connection string using the provided parameters
            connectionString = $"Server={server};Database={database};User Id={user};Password={password}";

            // Call the TestConnection method to update the connection string if needed
            TestConnection();
        }

        private void TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Test the connection with login 1: Emi
                    connection.Open();
                    connection.Close();
                }

                // If successful, no need to test further
                return;
            }
            catch
            {
                // If login 1 fails, try login 2: Juan
                string server = "100.104.98.58";
                string database = "Proyecto";
                string user = "juanes";
                string password = "12345678";

                connectionString = $"Server={server};Database={database};User Id={user};Password={password}";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        connection.Close();
                    }
                }
                catch
                {
                    // Prompt an error message that database is not accessible
                    throw new InvalidOperationException("No se puede conectar a la base de datos. Verifique si esta encendido el servidor, y si tiene las credenciales correctas.");
                }
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
