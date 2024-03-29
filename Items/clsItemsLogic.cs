﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject_WpfApp.Items
{
    public class clsItemsLogic
    {
        clsItemsSQL clsItemsSQL;

        public clsItemsLogic()
        {
            clsItemsSQL = new clsItemsSQL(); 
        }

        public List<clsItem> getAllItems()
        {
            try
            {
                int iRef = 0;
                DataSet itemsTableDataSet = clsItemsSQL.selectAllItems(ref iRef);

                List<clsItem> listItems = new List<clsItem>();

                for (int i = 0; i < iRef; i++)
                {
                    clsItem tempItem = new clsItem((string)itemsTableDataSet.Tables[0].Rows[i][0],
                                                    (string)itemsTableDataSet.Tables[0].Rows[i][1],
                                                    (decimal)itemsTableDataSet.Tables[0].Rows[i][2]
                                                    );
                    listItems.Add(tempItem);
                }

                return listItems;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        public List<int> getInvoiceNumbersWithItemcode(string itemCode, ref int iRef)
        {
            try
            {
                iRef = 0;
                DataSet lineItemsDataSet = clsItemsSQL.selectInvoicesWithItemcode(itemCode, ref iRef);

                List<int> listInvoiceNumbers = new List<int>();

                for (int i = 0; i < iRef; i++)
                {
                    clsLineItems tempLineItem = new clsLineItems((int)lineItemsDataSet.Tables[0].Rows[i][0]);

                    int tempInvoiceNumber = tempLineItem.InvoiceNumber;

                    listInvoiceNumbers.Add(tempInvoiceNumber);
                }

                return listInvoiceNumbers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        public void updateItem(string itemDescription, decimal cost, string itemCode)
        {
            try
            {
                clsItemsSQL.updateItem(itemDescription, cost, itemCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        public void insertItem(string itemCode, string itemDescription, decimal cost)
        {
            try
            {
                clsItemsSQL.insertItem(itemCode, itemDescription, cost);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        public void deleteItem(string itemCode)
        {
            try
            {
                clsItemsSQL.deleteItem(itemCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
