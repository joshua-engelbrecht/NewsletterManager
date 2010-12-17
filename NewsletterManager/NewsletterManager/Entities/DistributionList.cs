using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsletterManager.Entities
{
    public class DistributionList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastSentDate { get; set; }
        public bool SendNewsletterAsAttachment { get; set; }
    }
}
