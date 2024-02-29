using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executor
{
    public class DbInitialiser
    {
        private readonly ApplicationContext _context;

        public DbInitialiser(ApplicationContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (!_context.Database.CanConnect())
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }
            
        }
    }
}
