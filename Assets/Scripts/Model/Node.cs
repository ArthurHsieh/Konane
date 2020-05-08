using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Node
    {
        public Node(int index, int boardSize)
        {
            Index = index;
            X = index % boardSize;
            Y = index / boardSize;
        }

        public int Index { get; }
        public int X { get; }
        public int Y { get; }
        public bool IsWhite => (X + Y) % 2 != 0;
        public bool IsVacancy;
    }
}