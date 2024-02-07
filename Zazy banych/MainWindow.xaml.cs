using System;
using System.Collections.Generic;
using System.Data;
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
using MySqlConnector;

namespace Zazy_banych
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void bConnect_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection;
            if (bConnect.Content.ToString() != "Disconnect")
            {
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = tbName.Text,
                    Database = tbDatabase.Text,
                    UserID = tbUID.Text,
                    Password = pbPassword.Password,
                    SslMode = MySqlSslMode.None,
                };
                bConnect.Content = "Disconnect";
                lConnect.Content = "Connected";
                connection = new MySqlConnection(builder.ConnectionString);
                try
                {
                    using (connection)
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand("SELECT * from `2p_dane`", connection))
                        {

                            DataTable dt = new DataTable();
                            MySqlDataAdapter da = new MySqlDataAdapter(command);
                            da.Fill(dt);
                            dGrid.ItemsSource = dt.DefaultView;
                            connection.Close();
                        }
                    }
                }
                catch(MySqlException ex)
                {
                    MessageBox.Show(ex.ToString(), "błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                bConnect.Content = "Connect";
                lConnect.Content = "No connection";
            }
        }
    }
}
