using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Experimental.James
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }

        public static double ToDegrees(this double val)
        {
            return val * (180 / Math.PI);
        }

        /// <summary>
        /// extension for doubles, converts a degree to a cardinal point enumeration
        /// </summary>
        /// <param name="degree"></param>
        /// <returns>a cardinal point enumeration representing a compass direction</returns>
        public static Coordinate.CardinalPoints ToCardinalMark(this double degree)
        {
            var CardinalRanges = new List<CardinalRanges>
                                {
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.N, LowRange = 0, HighRange = 22.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.NE, LowRange = 22.5, HighRange = 67.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.E, LowRange = 67.5, HighRange = 112.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.SE, LowRange = 112.5, HighRange = 157.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.S, LowRange = 157.5, HighRange = 202.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.SW, LowRange = 202.5, HighRange = 247.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.W, LowRange = 247.5, HighRange = 292.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.NW, LowRange = 292.5, HighRange = 337.5},
                                    new CardinalRanges {CardinalPoint = Coordinate.CardinalPoints.N, LowRange = 337.5, HighRange = 360.1},
                                };
            if (!(degree >= 0 && degree <= 360))
            {
                throw new ArgumentOutOfRangeException(String.Format("degree: {0}", degree), "Degree value must be between 0 and 360.");
            }
            return CardinalRanges.Find(value => (degree >= value.LowRange && degree < value.HighRange)).CardinalPoint;
        }

        private struct CardinalRanges
        {
            public Coordinate.CardinalPoints CardinalPoint;
            public double LowRange; //low range value associated with the cardinal point
            public double HighRange; //high range value associated with the cardinal point
        }
    }
}
