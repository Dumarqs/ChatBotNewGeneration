﻿using Domain.Chats;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Data.SqlServer.Contexts
{
    public class ChatBotContext : DbContext
    {
        public ChatBotContext(DbContextOptions<ChatBotContext> options) : base(options)
        {
        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
