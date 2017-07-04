using NUnit.Framework;
using PositionCalculator.Models;
using PositionCalculator.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PositionCalculator.Tests
{
    public class PositionCsvIOTests
    {
        public PositionCsvIOTests()
        {

        }

        [Test]
        public void PositionCsvIO_Throws_FileNotFoundException_When_File_Does_Not_Exist()
        {
            var positionCsvIO = new PositionCsvIO(@"C:\Input\Path", @"C:\Output\Path");

            Assert.Throws<FileNotFoundException>(() =>
            {
                var positionRows = positionCsvIO.Read("foo.csv");
            });

        }

        [Test]
        public void PositionCsvIO_Can_Read_Input_File_Successfully()
        {
            string _inputPath = @"D:\TestPrjs\PositionCalculator\PositionCalculatorSol\PositionCalculator.Console\Input\";
            string _outputPath = @"D:\Output\Path\";
            var positionCsvIO = new PositionCsvIO(_inputPath, _outputPath);
            var positionRows = positionCsvIO.Read("test_data.csv");

            Assert.True(positionRows.Count == 21);
        }

        [Test]
        public void PositionCsvIO_Throws_DirectoryNotFoundException_When_Output_Directory_Does_Not_Exist()
        {
            var positionCsvIO = new PositionCsvIO(@"C:\Input\Path", @"C:\Output\Path");

            Assert.Throws<DirectoryNotFoundException>(() =>
            {
                positionCsvIO.Write("foo.csv", new List<PositionResultRow> { 
                    new PositionResultRow{ Trader = "Joe", Symbol = "IBM.N", Quantity = 50}
                });
            });
        }

        [Test]
        public void PositionCsvIO_Can_Write_Output_File_Successfully()
        {
            string _inputPath = @"D:\Input\Path\";
            string _outputPath = @"D:\TestPrjs\PositionCalculator\PositionCalculatorSol\PositionCalculator.Console\Output\";
            var positionCsvIO = new PositionCsvIO(_inputPath, _outputPath);
            var outputResult = new List<PositionResultRow> { 
                new PositionResultRow{ Trader = "Joe", Symbol = "IBM.N", Quantity = 50}
            };
            var fileName = "output.csv";
            positionCsvIO.Write(fileName, outputResult);

            Assert.True(File.Exists(string.Format("{0}{1}", _outputPath, fileName)));
        }
    }
}
