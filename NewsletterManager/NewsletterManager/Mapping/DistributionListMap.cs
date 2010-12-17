using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewsletterManager.Entities;

namespace NewsletterManager.Mapping
{
    public class DistributionListMap
    {
        public SqlConnection connection { private get; set; }

        public int addDistributionList(DistributionList newDistroList)
        {
            var rtn = 0;
            var sql = string.Format("INSERT INTO nl.DistributionList (Id, Name)" +
                "Values ('{0}', '{1}')", newDistroList.Id, newDistroList.Name);
            using (var cmd = new SqlCommand(sql, connection))
            {
                rtn = cmd.ExecuteNonQuery();
            }
            return rtn;
        }

        public List<DistributionList> getDistributionLists()
        {
            var ListOfDistributionLists = new List<DistributionList>();
            var sql = string.Format("SELECT Id, Name, LastSentDate, SendNewsletterAsAttachment from nl.DistributionList");
            using (var cmd = new SqlCommand(sql, connection))
            {
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ListOfDistributionLists.Add(new DistributionList
                    {
                        Id = (Guid)dataReader["Id"],
                        Name = (string)dataReader["Name"],
                        LastSentDate= (DateTime)dataReader["LastSentDate"],
                        SendNewsletterAsAttachment = ((int)dataReader["SendNewsletterAsAttachment"] == 0) ? false : true
                    });

                }
                dataReader.Close();
            }
            return ListOfDistributionLists;
        }

        public List<DistributionList> getDistributionLists(DistributionList searchCriteria)
        {
            var ListOfDistributionLists = new List<DistributionList>();
            var whereString = "";

            if (!String.IsNullOrEmpty(searchCriteria.Name))
            {
                whereString += string.Format("Name like '%{0}%'", searchCriteria.Name);
            }

            if (!String.IsNullOrEmpty(searchCriteria.LastSentDate.ToShortDateString())
            {
                whereString += string.Format(" {0} LastSentDate < '{1}'", (String.IsNullOrEmpty(whereString)) ? "" : "and", searchCriteria.LastSentDate.ToShortDateString());
            }

            if (searchCriteria.SendNewsletterAsAttachment)
            {
                whereString += string.Format(" {0} SendNewsletterAsAttachment = '{1}'", (String.IsNullOrEmpty(whereString)) ? "" : "and", (searchCriteria.SendNewsletterAsAttachment == true) ? 1 : 0);
            }

            if (String.IsNullOrEmpty(whereString))
                return getDistributionLists();

            var sql = string.Format("SELECT Id, Name, LastSentDate, SendNewsletterAsAttachment from nl.DistributionList WHERE {0}", whereString);
            using (var cmd = new SqlCommand(sql, connection))
            {
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ListOfDistributionLists.Add(new DistributionList
                    {
                        Id = (Guid)dataReader["Id"],
                        Name = (string)dataReader["Name"],
                        LastSentDate= (DateTime)dataReader["LastSentDate"],
                        SendNewsletterAsAttachment = ((int)dataReader["SendNewsletterAsAttachment"] == 0) ? false : true
                    });

                }
                dataReader.Close();
            }
            return ListOfDistributionLists;
    }
}
