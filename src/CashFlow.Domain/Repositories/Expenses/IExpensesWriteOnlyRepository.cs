using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesWriteOnlyRepository
{
    public Task Add(Expense expense);

    /// <summary>
    /// This function returns True if the deletion was sucessful otherwise returns False
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<bool> Delete(long id);
}
