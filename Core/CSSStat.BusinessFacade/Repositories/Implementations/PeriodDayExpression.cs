using CsStat.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace BusinessFacade.Repositories.Implementations
{
    public static class PeriodsDayCondition
    {
        public static Expression<Func<Log, bool>> Get(Constants.PeriodDay? periodDay)
        {
            Expression<Func<Log, bool>> condition = null;

            if (periodDay == Constants.PeriodDay.Afternoon)
            {
                condition = x => x.DateTime.Date.AddHours(7) <= x.DateTime && x.DateTime < x.DateTime.Date.AddHours(13);
            }
            if (periodDay == Constants.PeriodDay.Evening)
            {
                condition = x => x.DateTime.Date.AddHours(13) <= x.DateTime;
            }

            return condition;
        }
    }
}
