using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsletterManager.Entities
{
    public class Newsletter
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
