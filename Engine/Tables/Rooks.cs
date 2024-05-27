using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tables
{
    internal class Rooks
    {

        public static readonly decimal[] WhiteRookTable = new decimal[64]
        {
                0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m,
                0.5m, 1m, 1m, 1m, 1m, 1m, 1m, 0.5m,
                0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
                0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
                0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
                0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
                0.5m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 0.5m,
                1m, 1m, 1m, 1m, 1m, 1m, 1m, 1m
        };

        public static readonly decimal[] BlackRookTable = new decimal[64]
{
            1m, 1m, 1m, 1m, 1m, 1m, 1m, 1m,
            0.5m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 0.5m,
            0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
            0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
            0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
            0.25m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.25m,
            0.5m, 1m, 1m, 1m, 1m, 1m, 1m, 0.5m,
            0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m
};
        public static decimal GetWhiteSquareWeight(int index)
        {
            return WhiteRookTable[index];
        }


        public static decimal GetBlackSquareWeight(int index)
        {
            return BlackRookTable[index];
        }
    }

}
