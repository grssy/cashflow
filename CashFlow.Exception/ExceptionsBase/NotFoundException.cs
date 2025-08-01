using System.Net;

namespace CashFlow.Exception.ExceptionsBase;
public class NotFoundException : CashFlowException
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;


    public NotFoundException(string errorMessage) : base(errorMessage)
    {
        
    }

    public override List<string> GetErrors()
    {
        return new List<string>() { Message };
    }
}
