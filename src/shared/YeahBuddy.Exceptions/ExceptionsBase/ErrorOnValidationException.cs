namespace YeahBuddy.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(IList<string> errorMessages) : YeahBuddyException
{
    public IList<string> ErrorMessages { get; set; } = errorMessages;
}