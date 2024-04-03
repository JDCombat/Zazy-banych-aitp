using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Zazy_banych
{
    /// <summary>
    /// Logika interakcji dla klasy OknoWindow.xaml
    /// </summary>
    public partial class OknoWindow : Window
    {
        public Contact _contact;
        public OknoWindow()
        {
            InitializeComponent();
        }
        public OknoWindow(Contact contact):this()
        {
            _contact = contact;
            tbName.Text = contact.Name;
            tbAge.Text = contact.Age.ToString();
            tbSurname.Text = contact.Surname;
        }
        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            if(tbName.Text == "" && tbSurname.Text == "" && tbAge.Text == "")
            {
                MessageBox.Show("Podaj informacje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _contact.Name = tbName.Text;
            _contact.Surname = tbSurname.Text;
            _contact.Age = int.Parse(tbAge.Text);
            DialogResult = true;
            return;
        }

        private void bImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            bool? result = op.ShowDialog();
            if (result == true)
            {
                var filename = op.FileName;
                var bytes = File.ReadAllBytes(filename);
                _contact.ImageBytes = bytes;
            }
        }
    }
}
