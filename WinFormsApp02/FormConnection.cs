using System;
using System.Windows.Forms;


namespace RCCombatCalc
{
    public partial class FormConnection : Form
    {
        #region VARS        
        string transType = string.Empty;
        CommunicationManager comm;
        LogStringClass curLogString;
        AutoCompleteStringCollection pilotList;
        BFDataClass dataBF;
        #endregion

        #region MAIN
        public FormConnection(LogStringClass logString, AutoCompleteStringCollection pilotList)
        {
            this.curLogString = logString;
            this.pilotList = pilotList;
            dataBF = new();
            comm = new CommunicationManager(dataBF);

            comm.CurrentTransmissionType = RCCombatCalc.CommunicationManager.TransmissionType.Text;

            InitializeComponent();
            textBox1.AutoCompleteCustomSource = pilotList;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        #endregion

        #region ON-LOAD INITIALIZATION
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
        #endregion

        #region OPEN PORT
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
        #endregion

        #region CLOSE PORT
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
        #endregion

        #region VIEW INFO, NO ACTION
        private void button1_Click(object sender, EventArgs e) // get info
        {
            comm.WriteData("info", CommunicationManager.RequestType.Info);
           
        }
        #endregion

        #region GET LOG AND PROCESS
        private void button2_Click(object sender, EventArgs e) // get log
        {
            comm.WriteData("log read", CommunicationManager.RequestType.Log);
            button3.Enabled = true;

        }
        #endregion

        #region SUBMIT RESULTS
        private void button3_Click(object sender, EventArgs e) // SUBMIT
        {
            curLogString.team = Int32.Parse(maskedTextBox1.Text);
            curLogString.isGroundTarget = radioButton1.Checked;
            curLogString.roundsFired = dataBF.ammoInit - dataBF.ammoLeft;
            curLogString.gunId = dataBF.iD;
            curLogString.health = dataBF.health;
            curLogString.hitsFrom = dataBF.hits;

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
        #endregion

        #region SEND LOG ERASE COMMAND
        private void button4_Click(object sender, EventArgs e) // CLEAR LOG
        {
            comm.WriteData("log erase", CommunicationManager.RequestType.Other);
        }
        #endregion
    }
}
