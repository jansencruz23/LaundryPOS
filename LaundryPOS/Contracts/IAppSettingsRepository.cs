﻿using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Contracts
{
    public interface IAppSettingsRepository : IBaseRepository<AppSettings>
    {
        Task<AppSettings> GetFirstAppSettings();
    }
}