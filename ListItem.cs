﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench
{
    public class ListItem
    {
        private string m_sText;
        private int m_nValue;

        public ListItem(string sText, int nValue)
        {
            m_sText = sText;
            m_nValue = nValue;
        }

        public string Text { get { return m_sText; } }

        public int Value { get { return m_nValue; } }

        public override string ToString()
        {
            return m_sText;
        }
    }

    public class CheckedListItem : ListItem
    {
        private Boolean m_bChecked;

        public Boolean Checked { get; set; }

        public CheckedListItem(string sText, int nValue, Boolean bChecked = true)
            : base(sText, nValue)
        {
            m_bChecked = bChecked;
        }
    }
}
