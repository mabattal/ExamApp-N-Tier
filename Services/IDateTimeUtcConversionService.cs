namespace ExamApp.Services
{
    public interface IDateTimeUtcConversionService
    {
        DateTime ConvertToUtc(DateTime dateTime);
        DateTime ConvertToTurkeyTime(DateTime dateTime);
    }
}
