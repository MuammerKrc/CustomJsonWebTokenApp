using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.IUnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.UnitOfWorks
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
