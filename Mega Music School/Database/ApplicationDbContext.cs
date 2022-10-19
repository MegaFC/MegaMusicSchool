using Mega_Music_School.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mega_Music_School.Database 
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserAndAdminProfile> UserAndAdminProfiles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LGA> LGAs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<StudentCourse> StudentCourse { get; set; }
    }
}
