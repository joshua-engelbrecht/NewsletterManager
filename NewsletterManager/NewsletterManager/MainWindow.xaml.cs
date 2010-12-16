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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;


using NewsletterManager.Entities;
using NewsletterManager.Mapping;
namespace NewsletterManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SubscriberMap subscriberMapping = new SubscriberMap();
        string connectionString = ConfigurationManager.AppSettings["connectionString"];
        SqlConnection conn;
        private bool _canreceivehtmlemail = false;
        private bool _canreceiveattachment = false;

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();
        }

        private void addSubscriber_Click(object sender, RoutedEventArgs e)
        {
            var newSubscriber = new Entities.Subscribers { FirstName = addSubFirstName.Text, 
                LastName = addSubLastName.Text, 
                EmailAddress = addSubEmail.Text,
                CanReceiveHtmlEmail = (addSubCanReceiveHtmlEmail.IsChecked == true) ? true : false,
                CanReceiveAttachment = (addSubCanReceiveAttachment.IsChecked == true) ? true : false
            };
            var success = subscriberMapping.addSubscriber(newSubscriber, conn);
            var msg = string.Format("{0} {1} added unsuccessfully", newSubscriber.FirstName, newSubscriber.LastName);
            if (success > 0)
            {
                msg = string.Format("{0} {1} added successfully", newSubscriber.FirstName, newSubscriber.LastName);
            }
            MessageBox.Show(msg);
            
            addSubFirstName.Text = "";
            addSubLastName.Text = "";
            addSubEmail.Text = "";
            addSubCanReceiveHtmlEmail.IsChecked = false;
            addSubCanReceiveAttachment.IsChecked = false;
        }

        private void editSubscriber_Click(object sender, RoutedEventArgs e)
        {
            var editSubscriber = new Subscribers { FirstName = addSubFirstName.Text, 
                LastName = addSubLastName.Text, 
                EmailAddress = addSubEmail.Text,
                CanReceiveHtmlEmail = (addSubCanReceiveHtmlEmail.IsChecked == true) ? true : false,
                CanReceiveAttachment = (addSubCanReceiveAttachment.IsChecked == true) ? true : false
            };
            var success = subscriberMapping.editSubscriber(editSubscriber, conn);
            var msg = string.Format("{0} {1} added unsuccessfully", editSubscriber.FirstName, editSubscriber.LastName);
            if (success > 0)
            {
                msg = string.Format("{0} {1} added successfully", editSubscriber.FirstName, editSubscriber.LastName);
            }
            MessageBox.Show(msg);
            clearAddTextBoxes(null, null);
        }

        private void closeSql()
        {
            conn.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeSql();
        }

        private void findSubscriber_Click(object sender, RoutedEventArgs e)
        {
            var findSubscriber = new Subscribers
            {
                FirstName = findSubFirstName.Text,
                LastName = findSubLastName.Text,
                EmailAddress = findSubEmail.Text,
                CanReceiveHtmlEmail = _canreceivehtmlemail,
                CanReceiveAttachment = _canreceiveattachment
            };

            var listOfSubscribers = subscriberMapping.getSubscribers(findSubscriber, conn);
            //display subscribers
            var editWindow = new EditSubscribers(listOfSubscribers);
            var ret = editWindow.ShowDialog();
            if (ret.Value)
            {
                var editedSubscribers = editWindow.getEditedList;
                subscriberMapping.editSubscribers(editedSubscribers, conn);
            }

            clearFindTextBoxes(null, null);
        }

        private void clearFindTextBoxes(object sender, RoutedEventArgs e)
        {
            findSubFirstName.Text = "";
            findSubLastName.Text = "";
            findSubEmail.Text = "";
            CRHE_true.IsChecked = false;
            CRHE_false.IsChecked = false;
            CRA_true.IsChecked = false;
            CRA_false.IsChecked = false;
        }

        private void CRHEChecked(object sender, RoutedEventArgs e)
        {
            if (CRHE_true.IsChecked == true)
                _canreceivehtmlemail = true;
            if (CRHE_false.IsChecked == true)
                _canreceivehtmlemail = false;
        }

        private void CRAChecked(object sender, RoutedEventArgs e)
        {
            if (CRA_true.IsChecked == true)
                _canreceiveattachment = true;
            if (CRA_false.IsChecked == true)
                _canreceiveattachment = false;
        }

        private void clearAddTextBoxes(object sender, RoutedEventArgs e)
        {
            addSubFirstName.Text = "";
            addSubLastName.Text = "";
            addSubEmail.Text = "";
            addSubCanReceiveHtmlEmail.IsChecked = false;
            addSubCanReceiveAttachment.IsChecked = false;
        }
    }
}
