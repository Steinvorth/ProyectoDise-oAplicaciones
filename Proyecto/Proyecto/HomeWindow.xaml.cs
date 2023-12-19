using Microsoft.Win32;
using Proyecto.Base_de_Datos;
using Proyecto.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        //Variable para almacenar el Username del trabajador
        string username;

        //DB
        Connect_DB db = new Connect_DB();
        SqlConnection connection;

        public HomeWindow()
        {
            InitializeComponent();     
            connection = db.GetConnection();
            
        }

        public void GetUsername(string usr)
        {
            this.username = usr;
            SetUserDetails();            
            //MessageBox.Show("El usuario es: " + username);
        }

        //metodo que consigue los datos del usuario en base de datos y los pone en labels.
        public void SetUserDetails()
        {
            try
            {
                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    // Query to retrieve user details and profile picture based on the username
                    string query = "SELECT T.nombre, T.apellido, T.tipoEmpleado, U.fotoUsuario " +
                                   "FROM Trabajadores T " +
                                   "JOIN Usuarios U ON T.username = U.username " +
                                   "WHERE T.username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve values from the database
                                string nombre = reader["nombre"].ToString();
                                string apellido = reader["apellido"].ToString();
                                string tipoEmpleado = reader["tipoEmpleado"].ToString();
                                byte[] imageBytes = reader["fotoUsuario"] as byte[];

                                // Concatenate with existing content and set to labels
                                userNom.Content = $"Nombre: {userNom.Content} {nombre}";
                                userLastName.Content = $"Apellido: {userLastName.Content} {apellido}";
                                userRole.Content = $"Rol: {userRole.Content} {tipoEmpleado}";

                                // Set the profile picture
                                SetProfilePicture(imageBytes);
                            }
                            else
                            {
                                MessageBox.Show("Usuario no encontrado en la base de datos.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los detalles del usuario: " + ex.Message);
            }
        }

        //metodo que establece la imagen del perfil
        private void SetProfilePicture(byte[] imageBytes)
        {
            if (imageBytes != null && imageBytes.Length > 0)
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();

                        // Set the profilePic Image control source
                        profilePic.Source = bitmapImage;
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the exception details
                    Console.WriteLine("Error al establecer la imagen del perfil: " + ex.Message);
                }
            }
        }

        private void rutas_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        //boton mantenimiento de conductores, Este permite que el usuario pueda acceder a la ventana
        //estam seccion permite que el usuario pueda modificar conductores
        private void conductores_btn_Click(object sender, RoutedEventArgs e)
        {
            MantConductor conductorWindow = new MantConductor();
            conductorWindow.Show();
        }

        private void vehiculos_btn_Click(object sender, RoutedEventArgs e)
        {
            MantVehiculos mantVehiculos = new MantVehiculos();
            mantVehiculos.Show();
        }

        private void salir_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Carga_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
