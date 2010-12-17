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

        public List<DistributionList> findDistributionLists(DistributionList findDistribution)
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
    }
}
