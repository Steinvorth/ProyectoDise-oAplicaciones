using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Proyecto.Base_de_Datos;


namespace Proyecto.Windows
{
    /// <summary>
    /// Interaction logic for MantConductor.xaml
    /// </summary>
    public partial class MantConductor : Window
    {
        //conexion a base de datos
        Connect_DB db = new Connect_DB();
        SqlConnection connection;


        public MantConductor()
        {
            InitializeComponent();

            connection = db.GetConnection();
            SetConductores();
        }

        private void SetConductores()
        {
            // Fetch data from the database
            DataTable conductoresDataTable = FetchConductoresFromDatabase();

            // Bind the data to the DataGrid
            DG.ItemsSource = conductoresDataTable.DefaultView;
        }

        private DataTable FetchConductoresFromDatabase()
        {
            DataTable conductoresDataTable = new DataTable();

            // Assume you have appropriate relationships between tables and modify the query accordingly
            string query = "SELECT c.id, t.nombre, t.apellido, t.cedula, v.tipoVehiculo, v.placa " +
                           "FROM ConductorVehiculo c " +
                           "INNER JOIN Trabajadores t ON c.cedulaTrabajador = t.cedula " +
                           "INNER JOIN Vehiculos v ON c.vehiculo_id = v.vehiculo_id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    conductoresDataTable.Load(reader);
                }

                connection.Close();
            }

            return conductoresDataTable;
        }
    }
}
    

