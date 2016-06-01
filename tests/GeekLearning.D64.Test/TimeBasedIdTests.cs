using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeekLearning.D64.Test
{
    public class TimeBasedIdTests
    {
        [Theory]
        [InlineData("1977-04-22T06:00:00Z", "1977-04-22T06:00:00.5Z")]
        [InlineData("1985-05-09T06:00:00Z", "2016-04-22T06:00:00Z")]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T07:00:00Z")]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T06:01:00Z")]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T06:00:01Z")]
        public void TimeOrdering(string isoDateA, string isoDateB)
        {
            var dateA = DateTimeOffset.Parse(isoDateA);
            var dateB = DateTimeOffset.Parse(isoDateB);

            var idGen = new TimebasedId(false);

            Assert.True(StringComparer.Ordinal.Compare(idGen.NewId(dateA), idGen.NewId(dateB)) < 0);
        }

        [Theory]
        [InlineData("1977-04-22T06:00:00Z", "1977-04-22T06:00:00.5Z")]
        [InlineData("1985-05-09T06:00:00Z", "2016-04-22T06:00:00Z")]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T07:00:00Z")]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T06:01:00Z")]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T06:00:01Z")]
        public void TimeOrderingDescending(string isoDateA, string isoDateB)
        {
            var dateA = DateTimeOffset.Parse(isoDateA);
            var dateB = DateTimeOffset.Parse(isoDateB);

            var idGen = new TimebasedId(true);

            Assert.True(StringComparer.Ordinal.Compare(idGen.NewId(dateA), idGen.NewId(dateB)) > 0);
        }


        [Theory]
        [InlineData("1985-05-09T06:00:00Z", new string[] { "1977-04-22T06:00:00Z", "1977-04-22T06:00:00.5Z",
            "1985-05-09T06:00:00Z", "1985-05-09T06:01:00Z", "1985-05-09T06:00:01Z"})]
        public void DateFilter(string isoDate, params string[] otherDates)
        {
            var date = DateTimeOffset.Parse(isoDate);

            var idGen = new TimebasedId(false);

            var others = otherDates
                .Select(otherDate => DateTimeOffset.Parse(otherDate))
                .Select(otherDate => new KeyValuePair<string, DateTimeOffset>(idGen.NewId(otherDate), otherDate))
                .ToArray();

            var prefix = idGen.DatePrefix(date);

            var laterDatesByKey = others.Where(x => StringComparer.Ordinal.Compare(prefix, x.Key) <= 0);
            var laterDatesByValue = others.Where(x => x.Value >= date);

            Assert.Equal(laterDatesByValue, laterDatesByKey);
        }
    }
}
