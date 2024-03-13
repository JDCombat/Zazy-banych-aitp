using System;
using System.Collections.Generic;
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

    }
}
