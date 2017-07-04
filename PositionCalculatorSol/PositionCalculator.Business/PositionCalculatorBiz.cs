using PositionCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionCalculator.Business
{
    public class PositionCalculatorBiz
    {
        public IEnumerable<PositionResultRow> GetNetPositions(List<PositionRow> positionRows)
        {
            var netPositions = positionRows
                .GroupBy(p => new
                {
                    p.Trader,
                    p.Symbol
                })
                .Select(p => new PositionResultRow
                {
                    Trader = p.Key.Trader,
                    Symbol = p.Key.Symbol,
                    Quantity = p.Sum(r => r.Quantity)
                });
            return netPositions;
        }

        public IEnumerable<PositionResultRow> GetBoxedPositions(List<PositionRow> positionRows)
        {
            var boxedPlusPositions = positionRows
                .Where(p => p.Quantity > 0)
                .GroupBy(p => new
                {
                    Trader = p.Trader,
                    Symbol = p.Symbol
                })
                .Select(p => new
                {
                    Trader = p.Key.Trader,
                    Symbol = p.Key.Symbol
                });

            var boxedMinusPositions = positionRows
                .Where(p => p.Quantity < 0)
                .GroupBy(p => new
                {
                    Trader = p.Trader,
                    Symbol = p.Symbol
                })
                .Select(p => new
                {
                    Trader = p.Key.Trader,
                    Symbol = p.Key.Symbol
                });

            var boxedPositions = positionRows
                .Where(p =>
                    boxedPlusPositions.Any(r => r.Trader == p.Trader && r.Symbol == p.Symbol) &&
                    boxedMinusPositions.Any(r => r.Trader == p.Trader && r.Symbol == p.Symbol)
                )
                .GroupBy(p => new { p.Trader, p.Symbol },
                    (g, r) => new
                    {
                        Trader = g.Trader,
                        Symbol = g.Symbol,
                        Quantity = Math.Min(r.Where(q => q.Quantity > 0).Sum(s => s.Quantity), r.Where(q => q.Quantity < 0).Sum(s => Math.Abs(s.Quantity))),
                        Count = r.Select(s => s.Broker).Distinct().Count()
                    }
                )
                .Where(p => p.Count > 1)
                .Select(p => new PositionResultRow { Trader = p.Trader, Symbol = p.Symbol, Quantity = p.Quantity });

            return boxedPositions;
        }
    }
}
