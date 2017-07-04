using PositionCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PositionCalculator.Utilities
{
    public interface IPositionCsvIO
    {
        List<PositionRow> Read(string fileName);
        void Write(string fileName, IEnumerable<PositionResultRow> positionRows);
    }
}
