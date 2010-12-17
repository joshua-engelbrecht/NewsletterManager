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

namespace NewsletterManager
{
    /// <summary>
    /// Interaction logic for EditSubscriberView.xaml
    /// </summary>
    public partial class EditSubscriberView : Window
    {
        private Entities.Subscribers editMe;
        private NewsletterManager.Mapping.SubscriberMap subscriberMapping;

        public EditSubscriberView(Entities.Subscribers editMe, NewsletterManager.Mapping.SubscriberMap subscriberMap)
        {
            this.editMe = editMe;
            InitializeComponent();
            subscriberMapping = subscriberMap;
            DisplaySubscriber();
        }

        private void DisplaySubscriber()
        {
            editSubFirstName.Text = editMe.FirstName;
            editSubLastName.Text = editMe.LastName;
            editSubEmail.Text = editMe.EmailAddress;
            editSubCanReceiveHtmlEmail.IsChecked = editMe.CanReceiveHtmlEmail;
            editSubCanReceiveAttachment.IsChecked = editMe.CanReceiveAttachment;
        }

        private void editSubscriber_Click(object sender, RoutedEventArgs e)
        {
            editMe.FirstName = editSubFirstName.Text;
            editMe.LastName = editSubLastName.Text;
            editMe.EmailAddress = editSubEmail.Text;
            editMe.CanReceiveHtmlEmail = (editSubCanReceiveHtmlEmail.IsChecked == true) ? true : false;
            editMe.CanReceiveAttachment = (editSubCanReceiveAttachment.IsChecked == true) ? true : false;

            subscriberMapping.editSubscriber(editMe);
            DialogResult = true;
            Close();
        }

    }
}
