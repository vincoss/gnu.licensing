using samplesl.Svr.Data;
using samplesl.Svr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace samplesl.Svr.Services
{
    public class EfDataStoreSvr : IDataStoreSvr
    {
        private readonly EfDbContext _context;

        public EfDataStoreSvr(EfDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            _context = context;
        }
    }
}
