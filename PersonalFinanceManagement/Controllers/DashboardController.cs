using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;

namespace PersonalFinanceManagement.Controllers;
[Authorize]
    public class DashboardController : Controller
    {

        private readonly ICategoryRepository _categoryRepo;
        private readonly IIncomeRepository _incomeRepo;
        private readonly ISpendingRepository _spendingRepo;
        private readonly UserManager<User> _userManager;

        public DashboardController(UserManager<User> userManager, ICategoryRepository categoryRepo, IIncomeRepository incomeRepo, ISpendingRepository spendingRepo)
        {
            _categoryRepo = categoryRepo;
            _incomeRepo = incomeRepo;
            _spendingRepo = spendingRepo;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            //Total Income
            double last7DaysIncome = await _incomeRepo.GetTotalIncomeLast7DaysAsync(user.Id);
            var cultureInfo = new CultureInfo("vi-VN");
            cultureInfo.NumberFormat.CurrencySymbol = "VND";
            ViewBag.TotalIncome = last7DaysIncome.ToString("C0", cultureInfo);
            //Total Expense
            double last7DaysSpending = await _spendingRepo.GetTotalSpendingLast7DaysAsync(user.Id);
            ViewBag.TotalSpending =last7DaysSpending.ToString("C0", cultureInfo);
            //Balance
            int Balance = Convert.ToInt32(last7DaysIncome - last7DaysSpending);
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(cultureInfo, "{0:C0}", Balance);

          
            ViewBag.DoughnutChartData = _spendingRepo.GetAllSpendings(user.Id)
                .ToList()
                .GroupBy(s => s.CategoryId)
                .Select(g => new
                {
                    CategoryName = g.First().Category.Name,
                    Amount = g.Sum(s => s.Amount),
                    FormattedAmount = g.Sum(s => s.Amount).ToString("C0", cultureInfo),
                })
                .OrderByDescending(g => g.Amount)
                .ToList();


           
            List<SplineChartData> IncomeSummary = _incomeRepo.GetAllIncomes(user.Id)
                .GroupBy(i => i.Date)
                .Select(g => new SplineChartData()
                {
                    day = g.First().Date.ToString("dd-MMM"),
                    income = Convert.ToInt32(Math.Round(g.Sum(i => i.Amount)))
                })
                .ToList();

            List<SplineChartData> SpendingSummary = _spendingRepo.GetAllSpendings(user.Id)
                .GroupBy(i => i.Date)
                .Select(g => new SplineChartData()
                {
                    day = g.First().Date.ToString("dd-MMM"),
                    spending =  Convert.ToInt32(Math.Round(g.Sum(i => i.Amount)))
                })
                .ToList();

            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join spending in SpendingSummary on day equals spending.day into spendingJoined
                                      from spending in spendingJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          spending = spending == null ? 0 : spending.spending,
                                      };
           
            var recentTransactions = _spendingRepo.GetAllSpendings(user.Id)
                .Include(s => s.Category)
                .Include(s => s.User)
                .Select(s => new { s.Id, s.Description, s.Amount, s.Date, s.CategoryId, CategoryName = s.Category.Name, CategoryType= s.Category.Type , s.UserId,  s.CreatedAt, s.UpdatedAt })
                .Concat(_incomeRepo.GetAllIncomes(user.Id).Include(i => i.Category)
                    .Include(i => i.User)
                    .Select(i => new { i.Id, i.Description, i.Amount, i.Date, i.CategoryId, CategoryName = i.Category.Name,CategoryType = i.Category.Type, i.UserId,  i.CreatedAt, i.UpdatedAt }))
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToList();
 


            ViewBag.RecentTransactions = recentTransactions;




            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int spending;

    }