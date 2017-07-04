using PositionCalculator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PositionCalculator.Utilities
{
    public class PositionCsvIO: IPositionCsvIO
    {
        private readonly string _inputPath;
        private readonly string _outputPath;

        public PositionCsvIO(string inputPath, string outputPath)
        {
            _inputPath = inputPath;
            _outputPath = outputPath;
        }

        public List<PositionRow> Read(string fileName)
        {
            var positions = new List<PositionRow>();
            bool isFirstRow = true;
            var fullPath = string.Format("{0}{1}", _inputPath, fileName);

            if(!File.Exists(fullPath))
            {
                throw new FileNotFoundException();
            }

            using (var reader = new StreamReader(File.OpenRead(fullPath)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    var dataRow = line.Split(',');

                    positions.Add(
                        new PositionRow
                        {
                            Trader = dataRow[0],
                            Broker = dataRow[1],
                            Symbol = dataRow[2],
                            Quantity = long.Parse(dataRow[3]),
                            Price = decimal.Parse(dataRow[4])
                        }
                    );
                }
            }

            return positions;
        }

        public void Write(string fileName, IEnumerable<PositionResultRow> positionRows)
        {
            if (!Directory.Exists(_outputPath))
            {
                throw new DirectoryNotFoundException();
            }

            IEnumerable<string> headerRow = new List<string> { "TRADER,SYMBOL,QUANTITY" };
            IEnumerable<string> bodyRows = positionRows.Select(p => string.Format("{0},{1},{2}", p.Trader, p.Symbol, p.Quantity));
            var strRows = headerRow.Concat(bodyRows);
            var fullPath = string.Format("{0}{1}", _outputPath, fileName);
            
            using (var writer = new StreamWriter(fullPath))
            {
                foreach (var row in strRows)
                {
                    writer.WriteLine(row);
                }
                
            }
        }
    }
}
