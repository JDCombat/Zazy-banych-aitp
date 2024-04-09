using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace Zazy_banych
{
    public class Contact
    {
        public int Lp { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] ImageBytes { get; set; }
        public string Pesel { get; set;}
        public char Sex { get; set;}
        public DateTime BirthDate { get; set; }
        public string FormatedDate { get; set; }
        public string domciu { get; set; }
        public string number { get; set; }
        public string Email { get; set; }
        public string additional { get; set; }
    }
}
