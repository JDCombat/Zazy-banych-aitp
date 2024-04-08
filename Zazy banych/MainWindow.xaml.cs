using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
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
                    ConvertZeroDateTime = true,
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
                int lp = 1;
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contact contact = new Contact();
                        contact.Lp = lp;
                        contact.Id = int.Parse(reader["id"].ToString());
                        contact.Name = reader["Imie"].ToString();
                        contact.Surname = reader["Nazwisko"].ToString();
                        contact.Pesel = reader["Pesel"].ToString();
                        contact.Sex = reader["Płeć"].ToString().ToCharArray()[0];
                        contact.number = reader["Telefon"].ToString();
                        contact.domciu = reader["Adres"].ToString();
                        contact.Email = reader["E-mail"].ToString();
                        contact.additional = reader["Inf.dodatkowe"].ToString();
                        contact.BirthDate = DateTime.Parse(reader["Data urodzenia"].ToString());
                        contact.ImageBytes = (Byte[])reader["zdjecie"];
                        lp++;
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
                MySqlCommand command = new MySqlCommand($"INSERT INTO 2pdane(imie, nazwisko, zdjecie, Pesel, `Data urodzenia`, Adres, Telefon, `E-mail`, `Inf.dodatkowe`, Płeć) VALUES('{contact.Name}', '{contact.Surname}', ?data, '{contact.Pesel}', @date, '{contact.domciu}', '{contact.number}', '{contact.Email}', '{contact.additional}', {contact.Sex})", connection);
                MySqlParameter fileParameter = new MySqlParameter("?data", MySqlDbType.MediumBlob, contact.ImageBytes.Length);
                fileParameter.Value = contact.ImageBytes;
                command.Parameters.AddWithValue("@date", contact.BirthDate);
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
            var command = new MySqlCommand($"UPDATE 2pdane SET Imie = '{selected.Name}', Nazwisko = '{selected.Surname}', zdjecie = ?data, Pesel = '{selected.Pesel}', `Data urodzenia` = @date, Adres = '{selected.domciu}', Telefon = '{selected.number}', `E-mail` = '{selected.Email}', `Inf.dodatkowe` = '{selected.additional}', Płeć = '{selected.Sex}' WHERE id = {selected.Id}",connection);
            MySqlParameter fileParameter = new MySqlParameter("?data", MySqlDbType.MediumBlob, selected.ImageBytes.Length);
            fileParameter.Value = selected.ImageBytes;
            command.Parameters.AddWithValue("@date", selected.BirthDate);
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
                    MessageBox.Show(ex.Message.ToString(),"Błąd przy obrazie" ,MessageBoxButton.OK);
                }

            }

        }
    }
}
