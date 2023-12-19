using Proyecto.Base_de_Datos;
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
using System.Windows.Threading;


namespace Proyecto.Windows
{
    /// <summary>
    /// Interaction logic for MantVehiculos.xaml
    /// </summary>
    public partial class MantVehiculos : Window
    {
        Connect_DB db = new Connect_DB();
        private SqlConnection connection;
        private DispatcherTimer timer;

        private int vehiculoId;

        public MantVehiculos()
        {
            InitializeComponent();
            connection = new Connect_DB().GetConnection();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += Timer_Tick;

            CargarVehiculos();
        }

        private void CargarVehiculos()
        {
            DataTable vehiclesTable = ExecuteQuery("SELECT * FROM Vehiculos WHERE vehiculo_id NOT IN (SELECT vehiculo_id FROM MantenimientoVehiculos WHERE estado = 'En Mantenimiento')");
            selectCbox.ItemsSource = vehiclesTable.DefaultView;
            selectCbox.DisplayMemberPath = "placa";
            selectCbox.SelectedValuePath = "vehiculo_id";
        }

        private void enviarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectCbox.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un vehículo antes de enviar a mantenimiento.");
                return;
            }

            vehiculoId = (int)selectCbox.SelectedValue;
            string descripcion = descripcionTbox.Text;

            // Use parameterized query to prevent SQL injection
            string insertQuery = "INSERT INTO MantenimientoVehiculos (vehiculo_id, fechaInicio, estado, descripcion) VALUES (@vehiculo_id, GETDATE(), 'En Mantenimiento', @descripcion)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@vehiculo_id", SqlDbType.Int).Value = vehiculoId;
                command.Parameters.AddWithValue("@descripcion", SqlDbType.NVarChar).Value = descripcion;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar a mantenimiento: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            ActualizarVehiculosEnMantenimiento();
            ActualizarVehiculosListos();
            CargarVehiculos();  //actalizar los vehiculos disponibles en el ComboBox

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE MantenimientoVehiculos SET estado = 'Completado' WHERE vehiculo_id = @vehiculo_id AND estado = 'En Mantenimiento'";
            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@vehiculo_id", SqlDbType.Int).Value = vehiculoId;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al completar mantenimiento: " + ex.Message);
                    Console.WriteLine("Error al completar mantenimiento: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            ActualizarVehiculosEnMantenimiento();
            ActualizarVehiculosListos();
            CargarVehiculos();  //actalizar los vehiculos disponibles en el ComboBox

            timer.Stop();
        }

        private void ActualizarVehiculosEnMantenimiento()
        {
            DataTable vehiclesInMaintenanceTable = ExecuteQuery("SELECT * FROM MantenimientoVehiculos WHERE estado = 'En Mantenimiento'");
            vehiculosMantenimiento.ItemsSource = vehiclesInMaintenanceTable.DefaultView;
        }

        private void ActualizarVehiculosListos()
        {
            DataTable readyVehiclesTable = ExecuteQuery("SELECT * FROM MantenimientoVehiculos WHERE estado = 'Completado'");
            vehiculosListos.ItemsSource = readyVehiclesTable.DefaultView;
        }

        private DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return dataTable;
        }
    }
}
