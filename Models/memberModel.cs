using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidVaccinationSystem.Models
{
    public class memberModel
    {
        public List<memberBE> memberList = new List<memberBE>();
        public memberModel()
        {
            memberList = new List<memberBE>
            {
                new memberBE{ firstName="Gilli",  lastName="Rabinowitz"},
                new memberBE{ firstName="Itamar",  lastName="Rabinowitz"},
                new memberBE{firstName="Isaac",  lastName="Zinner"},
                new memberBE{ firstName="Osnat",  lastName="Rabinowitz"}
            };
        }
    }
}