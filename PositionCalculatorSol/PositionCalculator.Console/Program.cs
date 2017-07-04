using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PositionCalculator.Utilities;
using PositionCalculator.Business;

namespace PositionCalculator.ConsoleApp
{
    class Program
    {
        private const string _inputPath = @"D:\TestPrjs\PositionCalculator\PositionCalculatorSol\PositionCalculator.Console\Input\";
        private const string _outputPath = @"D:\TestPrjs\PositionCalculator\PositionCalculatorSol\PositionCalculator.Console\Output\";

        static void Main(string[] args)
        {
            var positionCsvIO = new PositionCsvIO(_inputPath, _outputPath);
            var positionRows = positionCsvIO.Read("test_data.csv");
            var calculator = new PositionCalculatorBiz();

            var netPositions = calculator.GetNetPositions(positionRows);
            positionCsvIO.Write("net_positions_expected.csv", netPositions);

            var boxedPositions = calculator.GetBoxedPositions(positionRows);
            positionCsvIO.Write("boxed_positions_expected.csv", boxedPositions);

            Console.Read();
        }
    }
}
