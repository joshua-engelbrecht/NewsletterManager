using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsletterManager.Entities
{
    class SubscriberToDistro
    {
        public Guid SubscriberId { get; set; }
        public Guid DistributionId { get; set; }
    }
}
