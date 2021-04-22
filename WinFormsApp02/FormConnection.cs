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
       
        
        string transType = string.Empty;
        CommunicationManager comm;
        LogStringClass curLogString;
        AutoCompleteStringCollection pilotList;
        public FormConnection(LogStringClass logString, AutoCompleteStringCollection pilotList)
        {
            this.curLogString = logString;
            this.pilotList = pilotList;
            comm = new CommunicationManager(logString);

            comm.CurrentTransmissionType = RCCombatCalc.CommunicationManager.TransmissionType.Text;

            InitializeComponent();
            textBox1.AutoCompleteCustomSource = pilotList;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void FormConnection_Load(object sender, EventArgs e) // INIT PORT LIST & BUTTONS STATE
        {
            comm.SetPortNameValues(cboPort);
            cboPort.SelectedIndex = 0;
            cmdClose.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void cmdOpen_Click(object sender, EventArgs e) // OPEN PORT
        {
            comm.PortName = cboPort.Text;
            
            comm.Parity = "None";
            comm.StopBits = "One";
            comm.DataBits = "8";
            comm.BaudRate = "115000";
            comm.DisplayWindow = rtbDisplay;

            if (comm.OpenPort())
            {
                cmdOpen.Enabled = false;
                cmdClose.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button4.Enabled = true;
            }
        }

        

              

        private void cmdClose_Click(object sender, EventArgs e) // CLOSE PORT 
        {
            if (comm.ClosePort())
            {
                cmdOpen.Enabled = true;
                cmdClose.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e) // get info
        {
            comm.WriteData("info", CommunicationManager.RequestType.Info);
           
        }

        private void button2_Click(object sender, EventArgs e) // get log
        {
            comm.WriteData("log read", CommunicationManager.RequestType.Log);
            button3.Enabled = true;

        }

        private void button3_Click(object sender, EventArgs e) // SUBMIT
        {
            curLogString.team = Int32.Parse(maskedTextBox1.Text);
            curLogString.isGroundTarget = radioButton1.Checked;

            if (radioButton1.Checked) { curLogString.name = "Ground target"; }
            else
            { curLogString.name = textBox1.Text; }


            if (!pilotList.Contains(curLogString.name) && !radioButton1.Checked)
            {
                pilotList.Add(curLogString.name);
            }

            comm.ClosePort();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e) // CLEAR LOG
        {
            comm.WriteData("log erase", CommunicationManager.RequestType.Other);
        }
    }
}
