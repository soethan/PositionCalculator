using NUnit.Framework;
using PositionCalculator.Business;
using PositionCalculator.Models;
using PositionCalculator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PositionCalculator.Tests
{
    [TestFixture]
    public class PositionCalculatorTest
    {
        private PositionCalculatorBiz _calculator;

        public PositionCalculatorTest()
        {
            _calculator = new PositionCalculatorBiz();
        }

        [Test]
        public void Calculate_Net_Positions_Works()
        {
            var positionRows = new List<PositionRow> { 
                new PositionRow{ Trader = "Joe", Broker = "ML", Symbol = "IBM.N", Quantity = 100, Price = 50},
                new PositionRow{ Trader = "Joe", Broker = "DB", Symbol = "IBM.N", Quantity = -50, Price = 50},
                new PositionRow{ Trader = "Joe", Broker = "CS", Symbol = "IBM.N", Quantity = 30, Price = 30},
                new PositionRow{ Trader = "Mike", Broker = "CS", Symbol = "AAPL.N", Quantity = 100, Price = 20},
                new PositionRow{ Trader = "Mike", Broker = "BC", Symbol = "AAPL.N", Quantity = 200, Price = 20},
                new PositionRow{ Trader = "Debby", Broker = "BC", Symbol = "NVDA.N", Quantity = 500, Price = 20}
            };

            var expectedResult = new List<PositionResultRow> { 
                new PositionResultRow{ Trader = "Joe", Symbol = "IBM.N", Quantity = 80},
                new PositionResultRow{ Trader = "Mike", Symbol = "AAPL.N", Quantity = 300},
                new PositionResultRow{ Trader = "Debby", Symbol = "NVDA.N", Quantity = 500}
            };

            var actualResult = _calculator.GetNetPositions(positionRows);
            Assert.AreEqual(expectedResult.Count, actualResult.Count());
            Assert.IsTrue(IsResultArrayValuesEqual(expectedResult, actualResult.ToList()));
        }

        [Test]
        public void Calculate_Boxed_Positions_Exist_When_A_Trader_Has_Long_And_Short_Positions_For_The_Same_Symbol_At_Different_Brokers()
        {
            var positionRows = new List<PositionRow> { 
                new PositionRow{ Trader = "Joe", Broker = "ML", Symbol = "IBM.N", Quantity = 100, Price = 50},
                new PositionRow{ Trader = "Joe", Broker = "DB", Symbol = "IBM.N", Quantity = -50, Price = 50},
                new PositionRow{ Trader = "Joe", Broker = "CS", Symbol = "IBM.N", Quantity = 30, Price = 30}
            };

            var expectedResult = new List<PositionResultRow> { 
                new PositionResultRow{ Trader = "Joe", Symbol = "IBM.N", Quantity = 50}
            };

            var actualResult = _calculator.GetBoxedPositions(positionRows);
            Assert.AreEqual(expectedResult.Count, actualResult.Count());
            Assert.IsTrue(IsResultArrayValuesEqual(expectedResult, actualResult.ToList()));
        }

        [Test]
        public void Calculate_Boxed_Positions_Does_Not_Exist_When_No_Trader_Has_Both_Long_And_Short_Positions_For_The_Same_Symbol_At_Different_Brokers()
        {
            var positionRows = new List<PositionRow> { 
                new PositionRow{ Trader = "Joe", Broker = "ML", Symbol = "IBM.N", Quantity = 100, Price = 50},
                new PositionRow{ Trader = "Joe", Broker = "DB", Symbol = "IBM.N", Quantity = 50, Price = 50},
                new PositionRow{ Trader = "Joe", Broker = "CS", Symbol = "IBM.N", Quantity = 30, Price = 30},
                new PositionRow{ Trader = "Mike", Broker = "DB", Symbol = "IBM.N", Quantity = -50, Price = 50},
                new PositionRow{ Trader = "Mike", Broker = "DB", Symbol = "IBM.N", Quantity = 20, Price = 50}
            };

            var actualResult = _calculator.GetBoxedPositions(positionRows);
            Assert.AreEqual(0, actualResult.Count());
        }
        
        private bool IsResultArrayValuesEqual(List<PositionResultRow> expectedResult, List<PositionResultRow> actualResult)
        {
            int count = 0;

            foreach (var item in actualResult)
            {
                if (expectedResult.Select(r => r.Trader == item.Trader && r.Symbol == item.Symbol && r.Quantity == item.Quantity).FirstOrDefault() != null)
                {
                    count++;
                }
            }

            return expectedResult.Count == count;
        }
    }
}
