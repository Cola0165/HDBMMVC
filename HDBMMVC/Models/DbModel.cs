using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QXHMVC.Models
{
    public class DbModel : DbContext
    {
        public DbModel() : base("name=HDBMMVCConnection")
        {
        }
        public System.Data.Entity.DbSet<QXHMVC.Models.Village> Villages { get; set; }

    }
}