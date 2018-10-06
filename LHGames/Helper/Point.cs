using System;

namespace LHGames.Helper
{
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static double Distance(Point p1, Point p2) => Math.Sqrt(DistanceSquared(p1, p2));
        public static double DistanceSquared(Point p1, Point p2) => (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        public override string ToString() => $"{{{X}, {Y}}}";
        public static Point operator -(Point pt1, Point pt2) => new Point(pt1.X - pt2.X, pt1.Y - pt2.Y);
        public static Point operator +(Point pt1, Point pt2) => new Point(pt1.X + pt2.X, pt1.Y + pt2.Y);
        public static bool operator ==(Point pt1, Point pt2) => Equals(pt1, pt2);
        public static bool operator !=(Point pt1, Point pt2) => !Equals(pt1, pt2);
        public override bool Equals(object obj) => obj is Point pt2 && X == pt2.X && Y == pt2.Y;

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
