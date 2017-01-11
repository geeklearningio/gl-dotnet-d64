namespace GeekLearning.D64.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

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
        public void DateFilterByPrefix(string isoDate, params string[] otherDates)
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

        [Theory]
        [InlineData("1985-05-09T06:00:00Z", new string[] { "1977-04-22T06:00:00Z", "1977-04-22T06:00:00.5Z",
            "1985-05-09T06:00:00Z", "1985-05-09T06:01:00Z", "1985-05-09T06:00:01Z"})]
        public void DateFilterByBoundary(string isoDate, params string[] otherDates)
        {
            var date = DateTimeOffset.Parse(isoDate);

            var idGen = new TimebasedId(false);

            var others = otherDates
                .Select(otherDate => DateTimeOffset.Parse(otherDate))
                .Select(otherDate => new KeyValuePair<string, DateTimeOffset>(idGen.NewId(otherDate), otherDate))
                .ToArray();

            var prefix = idGen.DateBoundary(date);

            var laterDatesByKey = others.Where(x => StringComparer.Ordinal.Compare(prefix, x.Key) <= 0);
            var laterDatesByValue = others.Where(x => x.Value >= date);

            Assert.Equal(laterDatesByValue, laterDatesByKey);
        }

        [Theory]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T07:00:00Z", new string[] {
            "1985-05-08T06:00:00Z", "1985-05-09T05:59:00Z", "1985-05-09T05:59:59Z",
            "1985-05-09T06:00:00Z","1985-05-09T06:59:59Z", "1985-05-09T06:00:00Z",
            "1985-05-09T06:01:00Z", "1985-05-09T06:00:01Z", "1985-05-09T07:00:01Z"
        })]
        public void DateFilterRangeByBoundary(string isoFromDate, string isoToDate, params string[] otherDates)
        {
            var fromDate = DateTimeOffset.Parse(isoFromDate);
            var toDate = DateTimeOffset.Parse(isoToDate);

            var idGen = new TimebasedId(false);

            var others = otherDates
                .Select(otherDate => DateTimeOffset.Parse(otherDate))
                .Select(otherDate => new KeyValuePair<string, DateTimeOffset>(idGen.NewId(otherDate), otherDate))
                .ToArray();

            var fromBoundary = idGen.DateBoundary(fromDate);
            var toBoundary = idGen.DateBoundary(toDate);

            var filteredByKey = others.Where(x => StringComparer.Ordinal.Compare(fromBoundary, x.Key) <= 0 && StringComparer.Ordinal.Compare(toBoundary, x.Key) > 0);
            var filteredByValue = others.Where(x => x.Value >= fromDate && x.Value <= toDate);

            Assert.Equal(filteredByValue, filteredByKey);
        }

        [Theory]
        [InlineData("1985-05-09T06:00:00Z", "1985-05-09T07:00:00Z", new string[] {
            "1985-05-08T06:00:00Z", "1985-05-09T05:59:00Z", "1985-05-09T05:59:59Z",
            "1985-05-09T06:00:00Z","1985-05-09T06:59:59Z", "1985-05-09T06:00:00Z",
            "1985-05-09T06:01:00Z", "1985-05-09T06:00:01Z", "1985-05-09T07:00:01Z"
        })]
        public void DateFilterRangeByPrefix(string isoFromDate, string isoToDate, params string[] otherDates)
        {
            var fromDate = DateTimeOffset.Parse(isoFromDate);
            var toDate = DateTimeOffset.Parse(isoToDate);

            var idGen = new TimebasedId(false);

            var others = otherDates
                .Select(otherDate => DateTimeOffset.Parse(otherDate))
                .Select(otherDate => new KeyValuePair<string, DateTimeOffset>(idGen.NewId(otherDate), otherDate))
                .ToArray();

            var fromBoundary = idGen.DatePrefix(fromDate);
            var toBoundary = idGen.DatePrefix(toDate);

            var filteredByKey = others.Where(x => StringComparer.Ordinal.Compare(fromBoundary, x.Key) <= 0 && StringComparer.Ordinal.Compare(toBoundary, x.Key) > 0);
            var filteredByValue = others.Where(x => x.Value >= fromDate && x.Value <= toDate);

            Assert.Equal(filteredByValue, filteredByKey);
        }
    }
}
