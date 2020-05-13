using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Chessboard
    {
        public Chessboard(int boardSize, bool isVacancy)
        {
            Nodes = new Node[boardSize * boardSize];
            for (int i = 0; i < Nodes.Length; ++i)
            {
                Nodes[i] = new Node(i, boardSize, isVacancy);
            }
            for (int i = 0; i < Nodes.Length; ++i)
            {
                if (Nodes[i].X != 0)
                    Nodes[i].LeftNode = Nodes[i - 1];
                if (Nodes[i].X != boardSize - 1)
                    Nodes[i].RightNode = Nodes[i + 1];
                if (Nodes[i].Y != 0)
                    Nodes[i].UpNode = Nodes[i - boardSize];
                if (Nodes[i].Y != boardSize - 1)
                    Nodes[i].DownNode = Nodes[i + boardSize];
            }
            BoardSize = boardSize;
        }

        public int BoardSize { get; }
        public Node[] Nodes { get; }
    }
}