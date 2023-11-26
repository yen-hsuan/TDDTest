namespace ConsoleApp1;

public class BudgetService(IBudgetRepo budgetRepo)
{
    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end) return 0;
        var budgets = budgetRepo.GetAll();

        var monthList = new List<string>
        {
            start.ToString("yyyyMM"),
            end.ToString("yyyyMM"),
        };
        var budgetList = Enumerable.Where<Budget>(budgets, x => monthList.Contains(x.YearMonth)).ToList();
        if (budgetList.Count == 0)
        {
            return 0;
        }

        var amountByDay = budgetList
            .ToDictionary(x => x.YearMonth,
                x => x.Amount / DateTime.DaysInMonth(int.Parse(x.YearMonth[..4]), int.Parse(x.YearMonth[4..])));


        var amount = 0;
        var currentDay = start;
        while (currentDay <= end)
        {
            var key = currentDay.ToString("yyyyMM");
            currentDay = currentDay.AddDays(1);
            if (!amountByDay.ContainsKey(key))
            {
                continue;
            }

            amount += amountByDay[key];
        }
        

        return amount;
    }
}