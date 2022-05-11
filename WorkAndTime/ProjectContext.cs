using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkAndTime
{
    public partial class ProjectContext : DbContext
    {
        public ProjectContext(DbConnection dbConnection) : base(dbConnection,false) { }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<TimeTrack> TimeTrack { get; set; }
    }
}
