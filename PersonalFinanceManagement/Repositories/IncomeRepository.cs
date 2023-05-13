using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Repositories
{
    public class IncomeRepository : IIncomeRepository 
    {
        private readonly MyDbContext _context;

        public IncomeRepository(MyDbContext context)
        {
            _context = context;
        }

        public IQueryable<Income> GetAllIncomes()
        {
            // return _context.Incomes.ToList();
            return _context.Incomes
                .Include(s => s.Category)
                .Include(s => s.User);
        }

        public Income GetIncomeById(Guid id)
        {
            return _context.Incomes.FirstOrDefault(i => i.Id == id);
        }

        public void AddIncome(Income income)
        {
            _context.Incomes.Add(income);
            _context.SaveChanges();
        }

        public void UpdateIncome(Income income)
        {
            _context.Incomes.Update(income);
            _context.SaveChanges();
        }

        public void DeleteIncome(Guid id)
        {
            var income = _context.Incomes.Find(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
                _context.SaveChanges();
            }
        }

        public async Task<double> GetTotalIncomeLast7DaysAsync()
        {
            // Get DateTime object for 7 days ago
            var sevenDaysAgo = DateTime.Today.AddDays(-7);

            // Calculate total income for last 7 days
            var totalIncome = await _context.Incomes
                .Where(i => i.Date >= sevenDaysAgo)
                .SumAsync(i => i.Amount);

            return totalIncome;
        }
    }
}