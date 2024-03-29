﻿using GroupProject_WpfApp.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject_WpfApp.Main
{
    internal class clsMainLogic
    {
        private int invoiceNum = 0;
        private DateTime invoiceDate = DateTime.Now;
        private decimal invoiceTotal = 0;
        internal List<clsItem> clsItems = new List<clsItem>();

        public List<clsItem> ClsItmes
        {
            get; set;
        }

        public int ID
        {
            get { return invoiceNum; }
            set { invoiceNum = value; }

        }
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }

        public decimal InvoiceTotal
        {
            get { return invoiceTotal; }
            set { invoiceTotal = value; }
        }


        public override string ToString()
        {
            return "ID: "+ ID.ToString() + " Date:" + invoiceDate.ToString() + " Total: $" + invoiceTotal.ToString();
        }
         
        public decimal Tax(decimal amount)
        {
            return Decimal.Multiply(amount, 0.1m);
        }
    }
}
