using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CharlieProject.ViewModel
{
    public class SubItem
    {
        //Defines sub elements, that we are essentially not using in the end, but was setup as an option in case it was needed. /JBR
        public SubItem(string name, UserControl screen = null)
        {
            Name = name;
            Screen = screen;
        }

        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
