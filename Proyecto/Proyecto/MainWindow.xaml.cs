using Proyecto.Base_de_Datos;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //login
        //databse connection
        Connect_DB db = new Connect_DB();
        SqlConnection connection;

        HomeWindow mainPage = new HomeWindow();

        private void CheckLogin(string username, string password)
        {
            try
            {
                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    // Query to verify if the username and password match
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // Login successful, open the main page
                            mainPage.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña inválidos. Por favor, inténtelo de nuevo.");
                            Console.WriteLine(count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error: " + ex.Message);
            }
            finally
            {
                db.GetConnection().Close();
            }
        }

        private void ingresarBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = userName.Text;
            string password = userPass.Text;

            CheckLogin(username, password);
        }
    }
}
