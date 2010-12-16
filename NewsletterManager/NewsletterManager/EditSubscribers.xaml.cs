using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NewsletterManager.Entities;

namespace NewsletterManager
{
    /// <summary>
    /// Interaction logic for EditSubscribers.xaml
    /// </summary>
    public partial class EditSubscribers : Window
    {
        public List<Subscribers> editedList = new List<Subscribers>();

        public EditSubscribers(List<Subscribers> ListToEdit)
        {
            InitializeComponent();
            DisplaySubscribers(ListToEdit);
        }

        private void DisplaySubscribers(List<Subscribers> ListToEdit)
        {
            foreach (Subscribers subscriber in ListToEdit)
            {
                editSubs.Items.Add(subscriber);
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void finishEdit_Click(object sender, RoutedEventArgs e)
        {
            foreach (Subscribers editedSub in editSubs.Items)
            {
                editedList.Add(editedSub);
            }
            DialogResult = true;
            Close();
        }

        public List<Subscribers> getEditedList
        {
            get { return editedList; }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
