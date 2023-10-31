using LaundryPOS.Contracts;
using LaundryPOS.Data;
using LaundryPOS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Repositories
{
    public class AppSettingsRepository : BaseRepository<AppSettings>, IAppSettingsRepository
    {
        public AppSettingsRepository(ApplicationDbContext context) 
            : base(context)
        {

        }

        public async Task<AppSettings> GetFirstAppSettings()
        {
            return await _context.AppSettings.FirstAsync();
        }
    }
}