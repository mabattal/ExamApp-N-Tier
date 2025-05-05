namespace ExamApp.Services.ExceptionHandlers
{
    public class CriticalException(string message) : Exception(message);
}
