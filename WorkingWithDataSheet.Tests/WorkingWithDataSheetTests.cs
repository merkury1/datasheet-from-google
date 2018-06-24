using ConsoleApp1;
using System.Collections.Generic;
using Xunit;

namespace WorkingWithDataSheet.Tests
{
    public class WorkingWithDataSheetTests
    {
        [Fact]
        public void CanGetArithmeticMean()
        {
            IList<object> dataList = new List<object>();
            dataList.Add(1);
            dataList.Add(2);
            dataList.Add(3);
            dataList.Add(4);
            dataList.Add(5);

            Program a = new Program();
            decimal arithmeticMean = Program.GetArithmeticMean(dataList);

            Assert.Equal(3, arithmeticMean);
        }
    }
}
