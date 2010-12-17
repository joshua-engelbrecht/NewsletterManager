using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
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
        DistributionListMap distributionListMapping = new DistributionListMap();
        SubscriberToDistroMap subscriberToDistributionMapping = new SubscriberToDistroMap();
        string connectionString = ConfigurationManager.AppSettings["connectionString"];
        SqlConnection conn;
        private bool _canreceivehtmlemail = false;
        private bool _canreceiveattachment = false;
        private bool _distroListSent = false;

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();
            subscriberMapping.connection = distributionListMapping.connection = subscriberToDistributionMapping.connection = conn;
            fillDistroListSubscriberFrom();
        }



        private void addSubscriber_Click(object sender, RoutedEventArgs e)
        {
            var newSubscriber = new Entities.Subscribers { FirstName = addSubFirstName.Text, 
                LastName = addSubLastName.Text, 
                EmailAddress = addSubEmail.Text,
                CanReceiveHtmlEmail = (addSubCanReceiveHtmlEmail.IsChecked == true) ? true : false,
                CanReceiveAttachment = (addSubCanReceiveAttachment.IsChecked == true) ? true : false
            };
            var success = subscriberMapping.addSubscriber(newSubscriber);
            var msg = string.Format("{0} {1} added unsuccessfully", newSubscriber.FirstName, newSubscriber.LastName);
            if (success > 0)
            {
                msg = string.Format("{0} {1} added successfully", newSubscriber.FirstName, newSubscriber.LastName);
            }
            System.Windows.MessageBox.Show(msg);
            
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
            var success = subscriberMapping.editSubscriber(editSubscriber);
            var msg = string.Format("{0} {1} added unsuccessfully", editSubscriber.FirstName, editSubscriber.LastName);
            if (success > 0)
            {
                msg = string.Format("{0} {1} added successfully", editSubscriber.FirstName, editSubscriber.LastName);
            }
            System.Windows.MessageBox.Show(msg);
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

            var listOfSubscribers = subscriberMapping.getSubscribers(findSubscriber);
            //display subscribers
            var findWindow = new FindSubscribers(listOfSubscribers, subscriberMapping);
            findWindow.ShowDialog();
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

        private void toDistroList_Click(object sender, RoutedEventArgs e)
        {
            var selected = new List<Subscribers>();
            foreach (Subscribers subscriber in addDistroListToSelectFrom.SelectedItems)
            {
                selected.Add(subscriber);
            }

            foreach (Subscribers subscriber in selected)
            {
                addDistroListSelectedSubscribers.Items.Add(subscriber);
                addDistroListToSelectFrom.Items.Remove(subscriber);
            }
        }

        private void fromDistroList_Click(object sender, RoutedEventArgs e)
        {
            var selected = new List<Subscribers>();
            foreach (Subscribers subscriber in addDistroListSelectedSubscribers.SelectedItems)
            {
                selected.Add(subscriber);
            }
            foreach (Subscribers subscriber in selected)
            {
                addDistroListToSelectFrom.Items.Add(subscriber);
                addDistroListSelectedSubscribers.Items.Remove(subscriber);
            }
            
        }

        private void fillDistroListSubscriberFrom()
        {
            var listOfSubscribers = subscriberMapping.getSubscribers();
            foreach (Subscribers subscriber in listOfSubscribers)
            {
                addDistroListToSelectFrom.Items.Add(subscriber);
            }
        }

        private void addDistroList_Click(object sender, RoutedEventArgs e)
        {
            var newDistroList = new DistributionList
            {
                Id = Guid.NewGuid(),
                Name = addDistroListName.Text,
                SendNewsletterAsAttachment = (addDistroListSendAtAttachment.IsChecked == true) ? true : false
            };

            var success1 = distributionListMapping.addDistributionList(newDistroList);
            var success2 = 0;
            foreach (Subscribers addToDistroList in addDistroListSelectedSubscribers.Items)
            {
                success2 = subscriberToDistributionMapping.addNewMapping(addToDistroList.Id, newDistroList.Id);
            }

            var msg = string.Format("{0} added unsuccessfully", newDistroList.Name);
            if (success1 > 0 && success2 > 0)
            {
                msg = string.Format("{0} added successfully", newDistroList.Name);
            }


            System.Windows.MessageBox.Show(msg);
            clearAddDistroForm(null, null);

        }

        private void clearAddDistroForm(object sender, RoutedEventArgs e)
        {
            addDistroListName.Text = "";
            addDistroListSendAtAttachment.IsChecked = false;
            addDistroListSelectedSubscribers.Items.Clear();
            addDistroListToSelectFrom.Items.Clear();
            fillDistroListSubscriberFrom();
        }

        private void calendar1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = findDistroDateCal.SelectedDate.Value;
            findDistroDate.Text = selectedDate.ToShortDateString();
            findDistroDateExpander.IsExpanded = false;
        }

        private void clearFindDistroClick(object sender, RoutedEventArgs e)
        {
            findDistroName.Text = "";
            findDistroDate.Text = "";
        }

        private void findDistributionList_Click(object sender, RoutedEventArgs e)
        {
            var findDistribution = new DistributionList{
                Name = findDistroName.Text,
                LastSentDate = DateTime.Parse(findDistroDate.Text),
                SendNewsletterAsAttachment = (AttachNewsletter.IsChecked == true) ? true : false
            };

            List<DistributionList> distroLists = distributionListMapping.getDistributionLists(findDistribution);

            clearFindDistroClick(null, null);
        }

    }
}
