using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RCCombatCalc
{
    public partial class FormConnection : Form
    {
        CommunicationManager comm = new CommunicationManager();
        string transType = string.Empty;
        public FormConnection()
        {
            InitializeComponent();
        }
    }
}
