using System;
using Microsoft.EntityFrameworkCore;

namespace AdminNewsModule.Models{

    public class NewsContext : DbContext{
        public NewsContext(DbContextOptions<NewsContext> options) : base(options){

        }

        public DbSet<Author> Author{get;set;}
        public DbSet<Category> Category{get;set;}
        public DbSet<News> News{get;set;}
    }
}