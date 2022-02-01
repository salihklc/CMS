using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketPeoplesModel
    {
        public int TicketIdx { get; set; }
        public List<Person> Watchers { get; set; }
        public Person Assignee { get; set; }
        public Person Reporter { get; set; }
      
    }

    public class Person
    {
        public int Idx { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }


    public class Vote
    {
        public int Idx { get; set; }
        public int Point { get; set; }
        public DateTime InsertDate { get; set; }
        public Person Person { get; set; }
    }
}
