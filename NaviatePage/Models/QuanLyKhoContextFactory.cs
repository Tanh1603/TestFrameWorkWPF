using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.Models
{
    public class QuanLyKhoContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public QuanLyKhoContextFactory(Action<DbContextOptionsBuilder> action)
        {
            _configureDbContext = action;
        }

        public QuanLyKhoContext CreateDbContext()
        {
            DbContextOptionsBuilder<QuanLyKhoContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<QuanLyKhoContext>();
            _configureDbContext(dbContextOptionsBuilder);
            return new QuanLyKhoContext(dbContextOptionsBuilder.Options);
        }
    }
}