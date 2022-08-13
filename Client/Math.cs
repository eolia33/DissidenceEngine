using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Client
{
    internal static class Math
    { 
        public static bool IsInZone(Point[] poly, Point pnt)
        {
        int i, j;
        int nvert = poly.Length;
        bool c = false;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            if (((poly[i].Y > pnt.Y) != (poly[j].Y > pnt.Y)) &&
             (pnt.X < (poly[j].X - poly[i].X) * (pnt.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X))
                c = !c;
        }
        return c;
        }

        public static bool IsASuccess(Random random, double chance)
        {
            if (random.NextDouble() < chance)
                return true;

            return false;
        }
    }
}
