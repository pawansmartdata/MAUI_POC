﻿using App.Core.Entity;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)  : base(options) { }


        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }  
    }
}
