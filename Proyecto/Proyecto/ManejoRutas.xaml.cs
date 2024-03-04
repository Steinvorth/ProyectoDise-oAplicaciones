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
using System.Windows.Shapes;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for ManejoRutas.xaml
    /// </summary>
    public partial class ManejoRutas : Window
    {
        //login
        //databse connection
        Connect_DB db = new Connect_DB();
        SqlConnection connection;

        HomeWindow mainPage = new HomeWindow();

        public ManejoRutas()
        {
            InitializeComponent();
        }
        private void SeleccionarRuta_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button botonRuta && int.TryParse(botonRuta.Tag.ToString(), out int numeroRuta))
            {
                // Aquí podrías obtener la información de la ruta según el número de ruta seleccionado (1-6)
                Ruta rutaSeleccionada = ObtenerRutaPorNumero(numeroRuta);
                Usuario usuarioActual = ObtenerUsuarioActual(); // Obtener el usuario actual (implementar según tu lógica de autenticación)
                MessageBox.Show($"Has seleccionado la ruta: {rutaSeleccionada.NombreRuta}");
            }
        }

        // Método para obtener la ruta correspondiente al número de ruta seleccionado
        private Ruta ObtenerRutaPorNumero(int numeroRuta)
        {
            // Lógica para obtener la ruta correspondiente según el número
            // Puedes obtener la ruta desde una lista, base de datos, etc.
            // Este método debe devolver la ruta correspondiente al número seleccionado
            // Por ejemplo, podrías tener una lista de rutas predefinidas y obtener la ruta por su índice
            // Este es solo un ejemplo, debes implementar la lógica de obtención de rutas según tu situación específica
            List<Ruta> rutasPredefinidas = ObtenerRutasPredefinidas();
            return rutasPredefinidas[numeroRuta - 1]; // Suponiendo que el número de ruta es 1-6 y las rutas se almacenan en una lista
        }

        // Evento para cuando se desee modificar rutas (para administradores)
        private void ModificarRutas_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para abrir una ventana de administrador o un formulario para modificar las rutas
            // Esto podría abrir una nueva ventana o cambiar la visibilidad de controles según el rol del usuario
        }

        // Método para obtener el usuario actual (simulado para demostración)
        private Usuario ObtenerUsuarioActual()
        {
            // Lógica para obtener el usuario actual (simulada para demostración)
            // Aquí debes implementar tu lógica real para obtener el usuario autenticado
            // Retorna un usuario ficticio para demostración
            return new Usuario { ID = 1, Nombre = "UsuarioPrueba", Rol = "Usuario" };
        }

        // Método para obtener las rutas predefinidas (simulado para demostración)
        private List<Ruta> ObtenerRutasPredefinidas()
        {
            // Lógica para obtener las rutas predefinidas (simulada para demostración)
            // Retorna una lista de rutas ficticias para demostración
            return new List<Ruta> {
                new Ruta { ID = 1, NombreRuta = "Ruta 1" },
                new Ruta { ID = 2, NombreRuta = "Ruta 2" },
                new Ruta { ID = 3, NombreRuta = "Ruta 3" },
                new Ruta { ID = 4, NombreRuta = "Ruta 4" },
                new Ruta { ID = 5, NombreRuta = "Ruta 5" },
                new Ruta { ID = 6, NombreRuta = "Ruta 6" }
            };
        }
    }
}
