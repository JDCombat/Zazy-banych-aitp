using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
        MySqlConnection connection;

        private void bConnect_Click(object sender, RoutedEventArgs e)
        {

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = tbName.Text,
                    Database = tbDatabase.Text,
                    UserID = tbUID.Text,
                    Password = pbPassword.Password,
                    SslMode = MySqlSslMode.None,
                };
                connection = new MySqlConnection(builder.ConnectionString);
            if (bConnect.Content.ToString() != "Disconnect")
            {
                try
                {
                    connection.Open();
                }
                catch(MySqlException ex)
                {
                    MessageBox.Show(ex.ToString(), "błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bConnect.Content = "Disconnect";
                lConnect.Content = "Connected";
            }
            else
            {
                bConnect.Content = "Connect";
                lConnect.Content = "No connection";
                connection.Close();
            }
        }
        private void bData_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            connection.Open();
            dGrid.Items.Clear();
            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * from `2pdane`", connection);
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contact contact = new Contact();
                        contact.Id = int.Parse(reader["id"].ToString());
                        contact.Name = reader["Imie"].ToString();
                        contact.Surname = reader["Nazwisko"].ToString();
                        contact.Age = int.Parse(reader["wiek"].ToString());
                        contact.ImageBytes = (Byte[])reader["zdjecie"];
                        dGrid.Items.Add(contact);
                    }
                }
                command.Dispose();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            connection.Open();
            Contact contact = new Contact();
            var okno = new OknoWindow(contact);
            okno.ShowDialog();
            if (okno.DialogResult == true)
            {
                MySqlCommand command = new MySqlCommand($"INSERT INTO 2pdane(imie, nazwisko, wiek, zdjecie) VALUES('{contact.Name}', '{contact.Surname}', {contact.Age}, ?data)", connection);
                MySqlParameter fileParameter = new MySqlParameter("?data", MySqlDbType.MediumBlob, contact.ImageBytes.Length);
                fileParameter.Value = contact.ImageBytes;
                command.Parameters.Add(fileParameter);
                command.ExecuteReader();
                command.Dispose();
                connection.Close() ;
                bData_Click(null, null);
            }

        }

        private void bRemove_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            connection.Open();
            var selected = (Contact)dGrid.SelectedItems[0];
            var command = new MySqlCommand($"DELETE FROM 2pdane where id = {selected.Id}",connection);
            command.ExecuteReader();
            command.Dispose();
            connection.Close();
            bData_Click(null, null);
        }

        private void bEdit_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            connection.Open();
            var selected = dGrid.SelectedItems[0] as Contact;
            var okno = new OknoWindow(selected);
            okno.ShowDialog();
            var command = new MySqlCommand($"UPDATE 2pdane SET Imie = '{selected.Name}', Nazwisko = '{selected.Surname}', Wiek = {selected.Age}, zdjecie = ?data WHERE id = {selected.Id}",connection);
            MySqlParameter fileParameter = new MySqlParameter("?data", MySqlDbType.MediumBlob, selected.ImageBytes.Length);
            fileParameter.Value = selected.ImageBytes;
            command.Parameters.Add(fileParameter);
            command.ExecuteReader();
            command.Dispose();
            connection.Close();
            bData_Click(null, null);
        }

        private void dGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dGrid.SelectedItem == null)
            {
                return;
            }
            else
            {
                try
                {
                    BitmapImage image = new BitmapImage();
                    using (var mem = new MemoryStream(((Contact)dGrid.SelectedItems[0]).ImageBytes))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    iImage.Source = image;
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),"Błąd przy obrazieeeee" ,MessageBoxButton.OK);
                }

            }

        }
    }
}
