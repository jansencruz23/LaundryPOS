﻿using LaundryPOS.Contracts;
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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Employee> GetEmployeeByUsername(string username)
        {
            return await _context.Employees
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Username.Equals(username));
        }
    }
}