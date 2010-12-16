using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsletterManager.Entities
{
    public class Subscribers
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool CanReceiveHtmlEmail { get; set; }
        public bool CanReceiveAttachment { get; set; }
    }
}
