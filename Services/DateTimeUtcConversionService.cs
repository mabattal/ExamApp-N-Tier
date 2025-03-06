namespace ExamApp.Services
{
    public class DateTimeUtcConversionService : IDateTimeUtcConversionService
    {
        private readonly TimeZoneInfo _turkeyTimeZone;

        public DateTimeUtcConversionService()
        {
            _turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
        }

        public DateTime ConvertToUtc(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, _turkeyTimeZone);
        }

        public DateTime ConvertToTurkeyTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, _turkeyTimeZone);
        }
    }
}