//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ElpoDockerApi
{

    public partial class TestDockerModel : DbContext
    {

        public TestDockerModel() :
            base()
        {
            OnCreated();
        }

        public TestDockerModel(DbContextOptions<TestDockerModel> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {




                IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
                IConfigurationRoot configuration = configBuilder.Build();

                string cnnStr = "";
                cnnStr = configuration.GetValue<string>("ConnectionStrings:Server");



                optionsBuilder.UseSqlServer(cnnStr);
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<Person> People
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.PersonMapping(modelBuilder);
            this.CustomizePersonMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region Person Mapping

        private void PersonMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable(@"Person", @"dbo");
            modelBuilder.Entity<Person>().Property<int>(x => x.Id).HasColumnName(@"Id").HasColumnType(@"int").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Person>().Property<string>(x => x.Name).HasColumnName(@"Name").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
            modelBuilder.Entity<Person>().HasKey(@"Id");
        }

        partial void CustomizePersonMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
