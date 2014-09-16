using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.RBT.InputFiles
{
    public partial class ucRBTChangeDetection : UserControl
    {
        public ucRBTChangeDetection()
        {
            InitializeComponent();
            lstSegregations.Items.Add(new Classes.BudgetSegregation("Tier 1 Channel Units", "tier1channelunits"));
            lstSegregations.Items.Add(new Classes.BudgetSegregation("Tier 2 Channel Units", "tier2channelunits"));
            lstSegregations.Items.Add(new Classes.BudgetSegregation("Channel / Non-channel", "channel"));
            lstSegregations.Items.Add(new Classes.BudgetSegregation("Bankfull Union", "bankfull_union"));
      
            for (int i = 0; i < lstSegregations.Items.Count; i++)
            {
                lstSegregations.SetItemChecked(i, true);
            }
        }

        private void ucRBTChangeDetection_Load(object sender, EventArgs e)
        {
            lstSegregations.CheckOnClick = true;

        }

        public CheckedListBox BudgetMasks
        {
            get { return lstSegregations; }
        }

        public double Threshold
        {
            get { return (double)valThreshold.Value; }
        }

    }
}
