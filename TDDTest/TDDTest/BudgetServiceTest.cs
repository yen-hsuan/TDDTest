using ConsoleApp1;
using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;

namespace TDDTest;

public class BudgetServiceTest
{
    private readonly BudgetService _budgetService;
    private readonly IBudgetRepo _budgetRepo;

    public BudgetServiceTest()
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
    public void query_amount_in_one_month_should_return_10()
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

    [Fact] public void query_amount_in_two_months_and_all_exist()
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
    public void query_amount_in_two_month_and_one_not_exist()
    {
        _budgetRepo.GetAll().Returns(new List<Budget> { new Budget
            {
                YearMonth = "202301",
                Amount = 310
            }
        });
        var actual = _budgetService.Query(new DateTime(2023, 1, 1), new DateTime(2023, 2, 15));
        actual.Should().Be(310); 
    }

    [Fact]
    public void query_amount_in_leap_year()
    {
        _budgetRepo.GetAll().Returns(new List<Budget> { new Budget
            {
                YearMonth = "201602",
                Amount = 290
            },
        });
        var actual = _budgetService.Query(new DateTime(2016, 2, 29), new DateTime(2016, 2, 29));
        actual.Should().Be(10); 
        
    }
    
    [Fact]
    public void query_amount_in_three_month_and_one_not_exist()
    {
        _budgetRepo.GetAll().Returns(new List<Budget> { new Budget
            {
                YearMonth = "202310",
                Amount = 310
            },
            new Budget
            {
                YearMonth = "202312",
                Amount = 620
            }
        });
        var actual = _budgetService.Query(new DateTime(2023, 10, 31), new DateTime(2023, 12, 2));
        actual.Should().Be(50); 
        
    }

    [Fact]
    public void query_cross_two_year()
    {
        _budgetRepo.GetAll().Returns(new List<Budget> { new Budget
            {
                YearMonth = "202312",
                Amount = 310
            },
            new Budget
            {
                YearMonth = "202401",
                Amount = 620
            }
        });
        var actual = _budgetService.Query(new DateTime(2023, 12, 31), new DateTime(2024, 1, 2));
        actual.Should().Be(50); 
        
    }
    
    [Fact]
    public void query_invalid_period_should_return_zero()
    {
        var actual = _budgetService.Query(new DateTime(2023, 1, 2), new DateTime(2023, 1, 1));
        actual.Should().Be(0);
    }
}