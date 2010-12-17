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
using NewsletterManager.Mapping;
namespace NewsletterManager
{
    /// <summary>
    /// Interaction logic for FindSubscribers.xaml
    /// </summary>
    public partial class FindSubscribers : Window
    {
        private NewsletterManager.Mapping.SubscriberMap subscriberMapping;
        private List<Subscribers> editedList = new List<Subscribers>();
        private List<Subscribers> deletedList = new List<Subscribers>();

        public FindSubscribers(List<Subscribers> ListToEdit, NewsletterManager.Mapping.SubscriberMap subscriberMap)
        {
            InitializeComponent();
            Instructions.Content = "Instructions:" + Environment.NewLine +
               "Double Click on a Subscriber to Edit" + Environment.NewLine +
               "Click 'Cancel' to Discard Any Changes." + Environment.NewLine +
               "Click 'Finished Edititing' When Done.";
            editSubs.MouseDoubleClick += ListViewItem_MouseDoubleClick;
            subscriberMapping = subscriberMap;
            DisplaySubscribers(ListToEdit);
        }

        private void DisplaySubscribers(List<Subscribers> ListToEdit)
        {
            editSubs.Items.Clear();
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
            DialogResult = true;
            Close();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        private void deleteSelected(object sender, RoutedEventArgs e)
        {
            var selectedCount = editSubs.SelectedItems.Count;

            var msg = "Are you sure you want to delete " + selectedCount + ((editSubs.SelectedItems.Count != 1) ? " contacts" : " contact");
            var confirm = MessageBox.Show(msg, "Delete Confirmation", MessageBoxButton.YesNo);
            if (confirm.Equals(MessageBoxResult.Yes))
            {
                foreach (Subscribers deletededSub in editSubs.SelectedItems)
                {
                    deletedList.Add(deletededSub);
                }
                subscriberMapping.deleteSubscribers(deletedList);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var editSubscriber = (Subscribers)editSubs.SelectedItem;
            var selectedIndex = editSubs.SelectedIndex;
            var editSubscriberView = new EditSubscriberView(editSubscriber, subscriberMapping);
            var ret = editSubscriberView.ShowDialog();
            if (ret.Value)
            {
                var ListToEdit = new List<Subscribers>();
                foreach (Subscribers item in editSubs.Items)
                {
                    ListToEdit.Add(item);
                }
                DisplaySubscribers(ListToEdit);
            }
            editSubs.SelectedItems.Clear();
        }

    }
}
