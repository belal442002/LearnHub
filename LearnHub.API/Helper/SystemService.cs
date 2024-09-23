namespace LearnHub.API.Helper
{
    public static class SystemService
    {
        public static int GetSemesterByMonth()
        {
            var month = DateTime.Now.Month;

            if(month >= 9 && month <= 12)
            {
                return (int)SemesterNumber.First;
            }
            else if(month >= 1 && month <= 5)
            {
                return (int)SemesterNumber.Second;
            }
            else
            {
                return (int)SemesterNumber.Summer;
            }
        }
        public static double GetDurationInMinutes(DateTime EndDate, DateTime StartDate)
        {
            TimeSpan duration = EndDate - StartDate;
            return duration.TotalMinutes;
        }

    }
}
