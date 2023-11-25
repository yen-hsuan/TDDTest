using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;

namespace TDDTest;

public class UnitTest1
{
    private readonly BudgetService _budgetService;
    private readonly IBudgetRepo _budgetRepo;

    public UnitTest1()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Fact]
    public void query_amount_in_one_month_should_return_more_than_zero()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>
        {
            new Budget() { Amount = 310, YearMonth = "202301" }
        });
        var actual = _budgetService.Query(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        actual.Should().Be(310);
    }

    [Fact]
    public void query_amount_in_one_month_should_return_zero()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>
        {
            new Budget() { Amount = 0, YearMonth = "202301" }
        });
        var actual = _budgetService.Query(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        actual.Should().Be(0);
    }

    [Fact]
    public void query_amount_in_one_day_should_return_10()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>
        {
            new Budget() { Amount = 310, YearMonth = "202301" }
        });
        var actual = _budgetService.Query(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));
        actual.Should().Be(10);
    }

    [Fact]
    public void query_amount_when_budget_not_exist_then_return_0()
    {
        _budgetRepo.GetAll().Returns(new List<Budget> { });
        var actual = _budgetService.Query(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        actual.Should().Be(0);
    }

    [Fact]
    public void get_period_between_two_months()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>
        {
            new Budget
            {
                YearMonth = "202310",
                Amount = 310
            },
            new Budget
            {
                YearMonth = "202311",
                Amount = 600
            }
        });
        var actual = _budgetService.Query(new DateTime(2023, 10, 31), new DateTime(2023, 11, 2));
        actual.Should().Be(50);
    }

    [Fact]
    public void query_invalid_period_should_return_zero()
    {
        var actual = _budgetService.Query(new DateTime(2023, 1, 2), new DateTime(2023, 1, 1));
        actual.Should().Be(0);
    }
}

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end) return 0;
        var budgets = _budgetRepo.GetAll();

        var diffDays = (end - start).Days + 1;
        List<string> monthList = new List<string>
        {
            start.ToString("yyyyMM"),
            end.ToString("yyyyMM"),
        };
        var budgetList = budgets.Where(x => monthList.Contains(x.YearMonth)).ToList();
        if (budgetList.Count == 0)
        {
            return 0;
        }

        var amountByDay = budgetList
            .ToDictionary(x => x.YearMonth,
                x => x.Amount / DateTime.DaysInMonth(int.Parse(x.YearMonth[0..4]), int.Parse(x.YearMonth[4..])));


        var amount = 0;
        var currentDay = start;
        while (currentDay <= end)
        {
            amount += amountByDay[currentDay.ToString("yyyyMM")];

            currentDay = currentDay.AddDays(1);
        }
        

        return amount;
    }
}

public interface IBudgetRepo
{
    List<Budget> GetAll();
}

public class Budget
{
    public string YearMonth { get; set; }
    public int Amount { get; set; }
}