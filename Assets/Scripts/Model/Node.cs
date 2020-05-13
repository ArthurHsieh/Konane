using System;

namespace Model
{
    [Flags]
    public enum NeighborFlags
    {
        None = 0,
        Right = 1,
        Left = 2,
        Up = 4,
        Down = 8,
    }

    public class Node
    {
        public Node(int index, int boardSize)
        {
            Index = index;
            X = index % boardSize;
            Y = index / boardSize;
            IsVacancy = false;
        }

        public int Index { get; }
        public int X { get; }
        public int Y { get; }
        public bool IsWhite => (X + Y) % 2 != 0;
        public bool IsVacancy;
        public NeighborFlags NeighborFlags
        {
            get
            {
                NeighborFlags value = NeighborFlags.None;
                if (RightNode != null && !RightNode.IsVacancy)
                    value |= NeighborFlags.Right;
                if (LeftNode != null && !LeftNode.IsVacancy)
                    value |= NeighborFlags.Left;
                if (UpNode != null && !UpNode.IsVacancy)
                    value |= NeighborFlags.Up;
                if (DownNode != null && !DownNode.IsVacancy)
                    value |= NeighborFlags.Down;
                return value;
            }
        }
        public Node RightNode = null;
        public Node LeftNode = null;
        public Node UpNode = null;
        public Node DownNode = null;
    }
}