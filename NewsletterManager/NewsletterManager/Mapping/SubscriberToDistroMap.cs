using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewsletterManager.Entities;

namespace NewsletterManager.Mapping
{
    class SubscriberToDistroMap
    {
        public SqlConnection connection { private get; set; }


        public int addNewMapping(Guid SubscriberId, Guid DistroListId)
        {
            var rtn = 0;
            var sql = string.Format("INSERT INTO nl.SubscriberToDistro (SubscriberId, DistributionId)" +
            "Values ('{0}', '{1}')", SubscriberId, DistroListId);
            using (var cmd = new SqlCommand(sql, connection))
            {
                rtn = cmd.ExecuteNonQuery();
            }

            return rtn;

        }
    }
}
