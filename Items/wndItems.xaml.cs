﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupProject_WpfApp.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        clsItemsSQL clsItemsSQL;

        public wndItems()
        {
            InitializeComponent();
            clsItemsSQL = new clsItemsSQL();
            updateDataGrid();
        }

        private void updateLabels()
        {

        }

        private void updateDataGrid()
        {
            itemsTable_DataGrid.ItemsSource = clsItemsSQL.getItems();
        }

        private void itemsTable_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
