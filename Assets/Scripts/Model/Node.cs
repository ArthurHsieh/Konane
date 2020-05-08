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

            if (X != boardSize - 1)
                RightIndex = Index + 1;
            else
                RightIndex = -1;

            if(X != 0)
                LeftIndex = Index - 1;
            else
                LeftIndex = -1;

            if (Y != 0)
                UpIndex = Index - boardSize;
            else
                UpIndex = -1;

            if (Y != boardSize - 1)
                DownIndex = Index + boardSize;
            else
                DownIndex = -1;
        }

        public int Index { get; }
        public int X { get; }
        public int Y { get; }
        public bool IsWhite => (X + Y) % 2 != 0;
        public bool IsVacancy;
        public int RightIndex { get; }
        public int LeftIndex { get; }
        public int UpIndex { get; }
        public int DownIndex { get; }
    }
}