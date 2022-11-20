using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reflection_l2
{
    public partial class DopName : Form
    {
        public DopName(string name, string text)
        {
            InitializeComponent(name);
            textBox1.Text = text;
        }
    }
}
