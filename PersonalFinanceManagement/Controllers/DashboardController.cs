using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Repositories;

namespace PersonalFinanceManagement.Controllers;
[Authorize]
    public class DashboardController : Controller
    {

        private readonly ICategoryRepository _categoryRepo;
        private readonly IIncomeRepository _incomeRepo;
        private readonly ISpendingRepository _spendingRepo;

        public DashboardController(ICategoryRepository categoryRepo, IIncomeRepository incomeRepo, ISpendingRepository spendingRepo)
        {
            _categoryRepo = categoryRepo;
            _incomeRepo = incomeRepo;
            _spendingRepo = spendingRepo;
        }

        public async Task<ActionResult> Index()
        {
            //Total Income
            double last7DaysIncome = await _incomeRepo.GetTotalIncomeLast7DaysAsync();
            var cultureInfo = new CultureInfo("vi-VN");
            cultureInfo.NumberFormat.CurrencySymbol = "VND";
            ViewBag.TotalIncome = last7DaysIncome.ToString("C0", cultureInfo);
            //Total Expense
            double last7DaysSpending = await _spendingRepo.GetTotalSpendingLast7DaysAsync();
            ViewBag.TotalSpending =last7DaysSpending.ToString("C0", cultureInfo);
            //Balance
            int Balance = Convert.ToInt32(last7DaysIncome - last7DaysSpending);
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(cultureInfo, "{0:C0}", Balance);

            //Doughnut Chart - Expense By Category
            // ViewBag.DoughnutChartData = SelectedTransactions
            //     .Where(i => i.Category.Type == "Expense")
            //     .GroupBy(j => j.Category.CategoryId)
            //     .Select(k => new
            //     {
            //         categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
            //         amount = k.Sum(j => j.Amount),
            //         formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
            //     })
            //     .OrderByDescending(l => l.amount)
            //     .ToList();
            //
            ViewBag.DoughnutChartData = _spendingRepo.GetAllSpendings()
                .GroupBy(s => s.CategoryId)
                .Select(g => new
                {
                    CategoryName = g.First().Category.Name,
                    Amount = g.Sum(s => s.Amount),
                    FormattedAmount = g.Sum(s => s.Amount).ToString("C0", cultureInfo),
                })
                .OrderByDescending(g => g.Amount)
                .ToList();

            //Spline Chart - Income vs Expense

            //Income
            // List<SplineChartData> IncomeSummary = SelectedTransactions
            //     .Where(i => i.Category.Type == "Income")
            //     .GroupBy(j => j.Date)
            //     .Select(k => new SplineChartData()
            //     {
            //         day = k.First().Date.ToString("dd-MMM"),
            //         income = k.Sum(l => l.Amount)
            //     })
            //     .ToList();
            
            //Expense
            // List<SplineChartData> ExpenseSummary = SelectedTransactions
            //     .Where(i => i.Category.Type == "Expense")
            //     .GroupBy(j => j.Date)
            //     .Select(k => new SplineChartData()
            //     {
            //         day = k.First().Date.ToString("dd-MMM"),
            //         expense = k.Sum(l => l.Amount)
            //     })
            //     .ToList();
            List<SplineChartData> IncomeSummary = _incomeRepo.GetAllIncomes()
                .GroupBy(i => i.Date)
                .Select(g => new SplineChartData()
                {
                    day = g.First().Date.ToString("dd-MMM"),
                    income = Convert.ToInt32(Math.Round(g.Sum(i => i.Amount)))
                })
                .ToList();

            List<SplineChartData> SpendingSummary = _spendingRepo.GetAllSpendings()
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
            //Recent Transactions
            // ViewBag.RecentTransactions = await _context.Transactions
            //     .Include(i => i.Category)
            //     .OrderByDescending(j => j.Date)
            //     .Take(5)
            //     .ToListAsync();
            // var recentTransactions =_spendingRepo.GetAllSpendings()
            //     .Concat<Income>(_incomeRepo.GetAllIncomes())
            //     .OrderByDescending(t => t.Date)
            //     .Take(5)
            //     .ToList();

            // var recentTransactions = _spendingRepo.GetAllSpendings()
            //     .Concat(_incomeRepo.GetAllIncomes())
            //     .OrderByDescending(t => t.Date)
            //     .Take(5)
            //     .ToList();
            var recentTransactions = _spendingRepo.GetAllSpendings()
                .Select(s => new { s.Id, s.Description, s.Amount, s.Date, s.CategoryId, s.Category, s.UserId, s.User, s.CreatedAt, s.UpdatedAt })
                .Concat(_incomeRepo.GetAllIncomes()
                    .Select(i => new { i.Id, i.Description, i.Amount, i.Date, i.CategoryId, i.Category, i.UserId, i.User, i.CreatedAt, i.UpdatedAt }))
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