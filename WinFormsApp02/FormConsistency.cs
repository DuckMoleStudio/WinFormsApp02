﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormConsistency : Form
    {
        List<int> consistencyList;
        List<int> gunIdIgnoreList;


        public FormConsistency(List<int> consistencyList, List<int> gunIdIgnoreList)
        {
            this.consistencyList = consistencyList;
            this.gunIdIgnoreList = gunIdIgnoreList;

            InitializeComponent();

            label2.Text = "";
            foreach (int c in consistencyList)
            {
                label2.Text += c + '\n';
            }
        }

        private void button1_Click(object sender, EventArgs e) // continue input
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // add to ignore list
        {
            foreach (int c in consistencyList) 
            { gunIdIgnoreList.Add(c); }

                this.Close();
        }
    }
}