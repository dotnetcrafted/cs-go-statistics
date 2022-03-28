using System;
using System.Linq;
using TelegramBot.Extensions;

namespace TelegramBot
{
    public class Time
    {
        public int Hours { get; private set; }
        public int Minutes { get; private set; }

        public Time(int hours, int minutes)
        {
            if (hours < 0 || hours > 23)
            {
                throw new ArgumentException("Hours must be in range 1 - 24");
            }
            if (minutes < 0 || minutes > 59)
            {
                throw new ArgumentException("Minutes must be in range 0 - 59");
            }
            Hours = hours;
            Minutes = minutes;
        }

        protected bool Equals(Time other)
        {
            return Hours == other.Hours && Minutes == other.Minutes;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Time) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Hours * 397) ^ Minutes;
            }
        }

        public static bool operator ==(Time timeOne, Time timeTwo)
        {
            return timeOne?.Hours == timeTwo?.Hours && timeOne?.Minutes == timeTwo?.Minutes;
        }

        public static bool operator !=(Time timeOne, Time timeTwo)
        {
            return !(timeOne == timeTwo);
        }

        public static bool TryParse(string time, out Time result)
        {
            result = new Time(0, 0);
            
            if (time.IsEmpty())
            {
                return false;
            }

            var hoursAndMinutes = time.Split(':');

            if (!hoursAndMinutes.Any())
            {
                return false;
            }

            if (!int.TryParse(hoursAndMinutes[0], out var hours) || !int.TryParse(hoursAndMinutes[1], out var minutes))
            {
                return false;
            }

            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            {
                return false;
            }

            result = new Time(hours, minutes);
            return true;


        }
    }
}