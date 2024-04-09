using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
            tbSurname.Text = contact.Surname;
            tbPesel.Text = contact.Pesel;
            tbAddress.Text = contact.domciu;
            tbNumber.Text = contact.number;
            tbEmail.Text = contact.Email;
            tbAdd.Text = contact.additional;
        }
        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            if(tbName.Text == "" || tbSurname.Text == "" || tbPesel.Text == "" || tbAddress.Text == "")
            {
                MessageBox.Show("Podaj informacje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _contact.Name = tbName.Text;
            _contact.Surname = tbSurname.Text;
            if (!checkPesel())
            {
                MessageBox.Show("Niepoprawny PESEL", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _contact.Pesel = tbPesel.Text;
            _contact.domciu = tbAddress.Text;
            _contact.number = tbNumber.Text;
            _contact.Email = tbEmail.Text;
            _contact.additional = tbAdd.Text;
            _contact.Sex = _contact.Pesel[9] % 2 == 0 ? 'K' : 'M';
            _contact.BirthDate = getDate();
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
                bImage.Content = $"Prześlij obraz ({filename})";
                var bytes = File.ReadAllBytes(filename);
                _contact.ImageBytes = bytes;
            }
        }

        private DateTime getDate()
        {
            var day = int.Parse(_contact.Pesel.Substring(4, 2));
            var month = int.Parse(_contact.Pesel.Substring(2, 2)) % 20;
            var century = (int)Math.Floor(double.Parse(_contact.Pesel.Substring(2, 2)) / 20);
            if (century == 4) century = -1;
            var year = int.Parse(_contact.Pesel.Substring(0, 2));
            DateTime data = new DateTime((century+19)*100+year, month, day);
            return data;
        }

        private bool checkPesel()
        {
            string tempPesel = tbPesel.Text;
            int[] wagi = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(tempPesel[i].ToString()) * wagi[i] % 10;
            }
            if (int.Parse(tempPesel[10].ToString()) != 10 - (sum % 10)) return false;
            if (int.Parse(tempPesel.Substring(2,2))%20>12) return false;
            return true;
        }
    }
}
