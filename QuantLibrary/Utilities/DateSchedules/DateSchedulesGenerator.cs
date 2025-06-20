using QuantLibrary.Utilities;
using QuantLibrary.Utilities.Resets;

namespace QuantLibrary.Utilities.DateSchedules
{
    public class DateSchedulesGenerator
    {
        public static ResetSchedule GenerateResetSchedule(DateTime startDate, DateTime endDate, Frequency frequency, Dictionary<DateTime, double> knownRates)
        {
            List<SingleReset> ResetSchedule = [];

            DateTime currDate = startDate;

            while (currDate < endDate)
            {
                DateTime resetEndDate = AddTenor(frequency, currDate);
                if (resetEndDate > endDate)
                {
                    resetEndDate = endDate;
                }
                SingleReset singleReset = new SingleReset { ResetDate = currDate, EndDate = resetEndDate };

                if (knownRates.TryGetValue(currDate, out double rate))
                {
                    singleReset.KnownRate = rate;
                }
                ResetSchedule.Add(singleReset);

                currDate = resetEndDate;
            }



            return new ResetSchedule(ResetSchedule);
        }

        private static DateTime AddTenor(Frequency frequency, DateTime currentDate)
        {
            if (frequency == Frequency._3M)
            {
                return currentDate.AddMonths(3);
            }
            else if (frequency == Frequency._6M)
            {
                return currentDate.AddMonths(6);
            }
            else if (frequency == Frequency._1D)
            {
                return currentDate.AddDays(1);
            }
            else
            {
                throw new Exception("Frequency not supported");
            }
        }
    }
}
