using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpenseUpdateOnlyRepository
{
    public void Update(Expense expense);
    public Task<Expense?> GetById(long id);
}
