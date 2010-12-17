using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewsletterManager.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Windows;

namespace NewsletterManager.Mapping
{
    public class SubscriberMap
    {
        public SqlConnection connection { private get; set; }

        public int addSubscriber(Subscribers subscriber)
        {
            var CRHE = subscriber.CanReceiveHtmlEmail ? 1 : 0;
            var CRA = subscriber.CanReceiveAttachment ? 1 : 0;
            var rtn = 0;
            var sql = string.Format("INSERT INTO nl.Subscribers (FirstName, LastName, EmailAddress, CanReceiveHtmlEmail, CanReceiveAttachment)" +
                "Values ('{0}', '{1}', '{2}', {3}, {4})", subscriber.FirstName, subscriber.LastName, subscriber.EmailAddress, CRHE, CRA);
            using (var cmd = new SqlCommand(sql, connection))
            {
                rtn = cmd.ExecuteNonQuery();
            }
            return rtn;
        }

        public Subscribers getSubscriber(string id)
        {
            Subscribers foundSubscriber = null;
            var sql = string.Format("SELECT FirstName, LastName, EmailAddress, CanReceiveHtmlEmail, CanReceiveAttachment from nl.Subscribers " +
                "WHERE Id= '{0}' limit 1", id);
            using (var cmd = new SqlCommand(sql, connection))
            {
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    foundSubscriber.Id = (Guid)dataReader["Id"];
                    foundSubscriber.FirstName = (string)dataReader["FirstName"];
                    foundSubscriber.LastName = (string)dataReader["LastName"];
                    foundSubscriber.EmailAddress = (string)dataReader["EmailAddress"];
                    foundSubscriber.CanReceiveHtmlEmail = ((int)dataReader["CanReceiveHtmlEmail"] == 1) ? true : false;
                    foundSubscriber.CanReceiveAttachment = ((int)dataReader["CanReceiveAttachment"] == 1) ? true : false;
                }
            }
            return foundSubscriber;
        }

        public List<Subscribers> getSubscribers()
        {
            var ListOfSubscribers = new List<Subscribers>();
            var sql = string.Format("SELECT Id, FirstName, LastName, EmailAddress, CanReceiveHtmlEmail, CanReceiveAttachment from nl.Subscribers");
            using (var cmd = new SqlCommand(sql, connection))
            {
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ListOfSubscribers.Add(new Subscribers
                    {
                        Id = (Guid)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        EmailAddress = (string)dataReader["EmailAddress"],
                        CanReceiveHtmlEmail = ((int)dataReader["CanReceiveHtmlEmail"] == 1) ? true : false,
                        CanReceiveAttachment = ((int)dataReader["CanReceiveAttachment"] == 1) ? true : false,
                    });

                }
                dataReader.Close();
            }
            return ListOfSubscribers;
        }

        public List<Subscribers> getSubscribers(Subscribers searchCriteria)
        {
            var ListOfSubscribers = new List<Subscribers>();
            var whereString = "";

            if (!String.IsNullOrEmpty(searchCriteria.FirstName))
            {
                whereString += string.Format("FirstName like '%{0}%'", searchCriteria.FirstName);
            }

            if (!String.IsNullOrEmpty(searchCriteria.LastName))
            {
                whereString += string.Format(" {0} LastName like '%{1}%'", (String.IsNullOrEmpty(whereString)) ? "" : "and", searchCriteria.LastName);
            }

            if (!String.IsNullOrEmpty(searchCriteria.EmailAddress))
            {
                whereString += string.Format(" {0} LastName like '%{1}%'", (String.IsNullOrEmpty(whereString)) ? "" : "and", searchCriteria.EmailAddress);
            }

            if (String.IsNullOrEmpty(whereString))
                return getSubscribers();

            var sql = string.Format("SELECT Id, FirstName, LastName, EmailAddress, CanReceiveHtmlEmail, CanReceiveAttachment from nl.Subscribers " +
                "WHERE {0}", whereString);
            using (var cmd = new SqlCommand(sql, connection))
            {
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ListOfSubscribers.Add(new Subscribers{
                        Id = (Guid)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        EmailAddress = (string)dataReader["EmailAddress"],
                        CanReceiveHtmlEmail = ((int)dataReader["CanReceiveHtmlEmail"] == 1) ? true : false,
                        CanReceiveAttachment = ((int)dataReader["CanReceiveAttachment"] == 1) ? true : false,
                    });
                    
                }
                dataReader.Close();
            }
            return ListOfSubscribers;
        }

        public int deleteSubscriber(string id)
        {
            var rtn = 0;
            var sql = string.Format("DELETE FROM nl.Subscribers WHERE Id= '{0}'", id);
            using (var cmd = new SqlCommand(sql, connection))
            {
                rtn = cmd.ExecuteNonQuery();
            }
            return rtn;
        }

        public int editSubscriber(Subscribers subscriber)
        {
            var CRHE = subscriber.CanReceiveHtmlEmail ? 1 : 0;
            var CRA = subscriber.CanReceiveAttachment ? 1 : 0;
            var rtn = 0;
            var sql = string.Format("Update nl.Subscribers Set FirstName = '{0}', LastName = '{1}', EmailAddress = '{2}', CanReceiveHtmlEmail = {3}, CanReceiveAttachment = {4}" +
                " WHERE Id = '{5}'" , subscriber.FirstName, subscriber.LastName, subscriber.EmailAddress, CRHE, CRA, subscriber.Id);
            using (var cmd = new SqlCommand(sql, connection))
            {
                rtn = cmd.ExecuteNonQuery();
            }
            return rtn;
        }


        public void editSubscribers(List<Subscribers> editedSubscribers)
        {
            foreach (Subscribers subscriber in editedSubscribers)
            {
                editSubscriber(subscriber);
            }
        }

        public void deleteSubscribers(List<Subscribers> deletedSubscribers)
        {
            foreach (Subscribers subscriber in deletedSubscribers)
            {
                deleteSubscriber(subscriber.Id.ToString());
            }
        }
    }
}
