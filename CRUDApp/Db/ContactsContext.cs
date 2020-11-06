using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Db
{
    public class ContactsContext : DbContext
    {
        public ContactsContext(DbContextOptions<ContactsContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Contact>().ToTable("Contact");
        }
    }
}
