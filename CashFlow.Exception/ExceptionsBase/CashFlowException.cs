namespace CashFlow.Exception.ExceptionsBase;
public abstract class CashFlowException : SystemException
{
    protected CashFlowException(string errorMessage) : base(errorMessage)
    {
        
    }

    public abstract int StatusCode { get; }

    public abstract List<string> GetErrors();
}
