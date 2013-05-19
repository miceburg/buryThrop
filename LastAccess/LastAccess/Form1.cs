using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LastAccess
{
    public partial class Form1 : Form
    {
        public static Form1 mF = null;

        public Form1()
        {
            InitializeComponent();
            mF = this;
            LastAccess.ChkAccessDates();
        }
    }
}
