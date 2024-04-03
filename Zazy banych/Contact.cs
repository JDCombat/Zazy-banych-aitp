using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Zazy_banych
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public byte[] ImageBytes { get; set; }
        public string Pesel { get; set;}
        public string Sex { get; set;}
        public DateTime BrithDate { get; set; }
        public string domciu { get; set; }
        public string number { get; set; }
        public string additional { get; set; }
    }
}
