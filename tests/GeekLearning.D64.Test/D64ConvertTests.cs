using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeekLearning.D64.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class D64ConvertTests
    {
        [Theory]
        [InlineData(42, 70)]
        [InlineData(60, 150)]
        [InlineData(60, 61)]
        [InlineData(72492028, 72492029)]
        [InlineData(72492028, 152492029)]
        public void LexicographicallySortable(int a, int b)
        {
            var aBytes = BitConverter.GetBytes(a);
            var bBytes = BitConverter.GetBytes(b);
            if (BitConverter.IsLittleEndian)
            {
                aBytes = aBytes.Reverse().ToArray();
                bBytes = bBytes.Reverse().ToArray();
            }
            Assert.True(StringComparer.Ordinal.Compare(D64Convert.Encode(aBytes), D64Convert.Encode(bBytes)) < 0);
        }
    }
}
