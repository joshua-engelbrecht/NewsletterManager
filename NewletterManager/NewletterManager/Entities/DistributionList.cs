using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsletterManager.Entities
{
    public class DistributionList
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Guid NewsletterId { get; set; }
        public bool HasBeenSent { get; set; }
        public bool SendNewsletterAsAttachment { get; set; }
    }
}
