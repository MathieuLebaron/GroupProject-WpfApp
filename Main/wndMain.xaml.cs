﻿using GroupProject_WpfApp.Items;
using GroupProject_WpfApp.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GroupProject_WpfApp.Main
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        #region windows
        wndItems itemWindow;
        wndSearch searchWindow;
        clsMainSQL mainInventory;
        clsMainLogic mainLogic;  
        clsDataAccess db;
        #endregion
        bool edit = false;
        bool newI = false;
        internal int invoiceID =0;//HEY SEND ME THE INVOICE NUMBER!!!!!
        internal clsItem itemID;//SEND ME THE ITEM ID TOO!!!!

        public wndMain()

        { //start window
            InitializeComponent();
            //show other windows
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
           
            searchWindow = new wndSearch(this); 
            itemWindow = new wndItems(this);
            mainInventory = new clsMainSQL();
            
            //put info into invoice and inventory list/drop down
            invoice_List.ItemsSource = mainInventory.getAllInvoices();
            ItemDropDown.ItemsSource = mainInventory.getAllItems();
        }

        

        /// <summary>
        /// Open Items page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                itemWindow = new wndItems(this);
                itemWindow.ShowDialog();
                this.Show();
                if (itemID != null && newI == true) ItemsList.Items.Add(itemID);
                if (itemID != null && edit == true) ItemsList.Items.Add(itemID);
                ItemDropDown.ItemsSource = mainInventory.getAllItems();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// open search page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                searchWindow = new wndSearch(this);
                searchWindow.ShowDialog();
                this.Show();
                //catch invoice id
                if (invoiceID != 0)
                {
                    int invoiceIndex = invoiceID - 5000;
                    invoice_List.SelectedIndex = invoiceIndex;
                }
                ItemDropDown.ItemsSource = mainInventory.getAllItems();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }


        }


        /// <summary>
        /// new invoice outline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            try{ 
            InvoiceDateBox.IsEnabled = true;
            ItemDropDown.IsEnabled = true;
            SelectButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
            newI = true;
            int id = mainInventory.getnewID();
            //show new invoice number
            invoiceNum.Content = id;
            //show current date
            InvoiceDateBox.Text = DateTime.Now.ToString();
            //show current Cost
            CostNum.Content = 0;
            //show Tax Cost
            taxNum.Content = 0;
            //show Total Cost
            TotalCostNum.Content = 0;
            //Show date
            InvoiceDateBox.Text = "";
            //items list
            ItemsList.ItemsSource = null;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }


        /// <summary>
        /// send selected inventory obj from drop down to items list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            try { 
            //Grab item number getONEItem()

            ItemsList.Items.Add(ItemDropDown.SelectedItem);

            //split items into variables
            string item = ItemDropDown.SelectedItem.ToString();
            string[] itemVar = item.Split(' ');
            string itemCost = "";
            for (int i = 0; i < itemVar.Length; i++)
            {
                itemCost = itemVar[i];
            }
            decimal Cost = Convert.ToDecimal(CostNum.Content) + Convert.ToDecimal(itemCost);

            //add item Description and cost it ItemsList
            CostNum.Content = Cost;
            taxNum.Content = decimal.Multiply(Cost, .1m);
            decimal TotalCost = Convert.ToDecimal(taxNum.Content) + Cost;
            TotalCostNum.Content = TotalCost;
        }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
    }
}

        /// <summary>
        /// save edited to correct invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
            //create new invoice obj
            clsMainLogic newInvoice = new clsMainLogic();
            List<clsItem> items = new List<clsItem>();

                //get new date
                DateTime invoiceDate = DateTime.Now;
                //save invoice id
                newInvoice.ID = Int32.Parse(invoiceNum.Content.ToString());
            //save itemsList to invoice obj
            //save Cost to invoice obj
            newInvoice.InvoiceTotal = decimal.Parse(TotalCostNum.Content.ToString());
               
            //add items to list
            foreach (clsItem s in ItemsList.Items)
            {
                items.Add(s);
            }

            //if new: newInvoice()
            if (edit == false)
            {
                    DateTime.TryParse(InvoiceDateBox.Text, out invoiceDate);
                    mainInventory.newInvoice(invoiceDate, newInvoice.InvoiceTotal, newInvoice.ID, items);//doesn't add all info yet. 
                newI = false;
            }
            //if edit: editInvoice()
            else
            {
                    DateTime.TryParse(InvoiceDateBox.Text, out invoiceDate);
                    mainInventory.editInvoice(invoiceDate, newInvoice.InvoiceTotal, newInvoice.ID, items);
                    edit = false;
            }
            //hide all invoice buttons
            invoice_List.ItemsSource = mainInventory.getAllInvoices();

            InvoiceDateBox.IsEnabled = false;
            ItemDropDown.IsEnabled = false;
            SelectButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            ItemDropDown.SelectedItem = null;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

            return;
        }

        /// <summary>
        /// delete selected items from ItemsList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int num = Int32.Parse(invoiceNum.Content.ToString());
            if(invoiceNum.Content == null) { return; }
            else  mainInventory.DeleteItemsFromInvoice(num);
            ItemsList.Items.Clear();
        }

        /// <summary>
        /// if different invoice is selected, invoice box is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoice_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ItemsList.Items.Clear();
                InvoiceDateBox.IsEnabled = false;
                ItemDropDown.IsEnabled = false;
                SelectButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;

                int idNum = invoice_List.SelectedIndex;
                idNum += 5000;
                if (idNum < 5000) idNum = 5000;
                clsMainLogic myInvoice = mainInventory.getOneInvoice(idNum);
                List<clsItem> items = mainInventory.getSomeItem(idNum);
                decimal cost = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    cost += items[i].Cost;
                }
                invoiceNum.Content = myInvoice.ID;
                InvoiceDateBox.Text = myInvoice.InvoiceDate.ToString();
                CostNum.Content = cost;
                foreach (clsItem item in items)
                {
                    ItemsList.Items.Add(item);
                }
                taxNum.Content = decimal.Multiply(cost, .1m); ; //to be updated once items are fixed.
                TotalCostNum.Content = myInvoice.InvoiceTotal.ToString();
            }

            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// edit function start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (invoice_List.SelectedValue != null)
                {
                    InvoiceDateBox.IsEnabled = true;
                    ItemDropDown.IsEnabled = true;
                    SelectButton.IsEnabled = true;
                    SaveButton.IsEnabled = true;
                    DeleteButton.IsEnabled = true;
                    edit = true;
                }
                else return;
            }

            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }


        /// <summary>
        /// catch all errors
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
