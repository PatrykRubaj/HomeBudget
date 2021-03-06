﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HomeBudget.Controllers;
using HomeBudget.Models;

namespace HomeBudget.Views
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        public DateTime TransactionDate { get; set; }
        private TransactionController transactionController;
        private bool isManualEditCommit = false;
        public ObservableCollection<Entry> Entries { get; set; }

        public TransactionView()
        {
            InitializeComponent();
            transactionController = new TransactionController();
            TransactionDate = DateTime.Now;
            Entries = new ObservableCollection<Entry>();
            LoadEntries();
            DataContext = this;
        }

        private void shopComboBox_DropDownOpened(object sender, EventArgs e)
        {
            shopComboBox.DataContext = transactionController.GetAllShops();
            shopComboBox.Items.Refresh();
        }

        private void saveTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            Shop selectedShop = shopComboBox.SelectedItem as Shop;
            transactionController.Add(TransactionDate.ToString(), amountTextBox.Text, selectedShop, descriptionTextBox.Text);

            ClearForm();
            RefreshTransactionsTable();
        }

        private void ClearForm()
        {
            this.amountTextBox.Clear();
            this.descriptionTextBox.Clear();
        }

        private void RefreshTransactionsTable()
        {
            LoadEntries();
            this.entryDataGrid.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTransactionsTable();
            shopComboBox.DataContext = transactionController.GetAllShops();
            shopComboBox.SelectedItem = 0;
        }

        private void TransactionDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var transaction = ((Button)sender).DataContext as Entry;
            var transWindow = new Windows.TransactionDetailsWindow(transaction);
            transWindow.ShowDialog();
        }
        private void DeleteTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var transaction = ((Button)sender).DataContext as Entry;
            transactionController.Delete(transaction);
            RefreshTransactionsTable();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshTransactionsTable();
        }

        private void entryDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!isManualEditCommit)
            {
                isManualEditCommit = true;
                DataGrid grid = (DataGrid)sender;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                isManualEditCommit = false;
            }

            transactionController.SaveChanges();
        }

        private void LoadEntries()
        {
            var tmpEntries = transactionController.GetAll();
            Entries.Clear();

            foreach(var entry in tmpEntries)
            {
                Entries.Add(entry);
            }
        }
    }
}
