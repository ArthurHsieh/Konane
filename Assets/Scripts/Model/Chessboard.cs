using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Chessboard
    {
        public Chessboard(int boardSize)
        {
            Nodes = new Node[boardSize * boardSize];
            for (int i = 0; i < Nodes.Length; ++i)
            {
                Nodes[i] = new Node(i, boardSize);
            }
            BoardSize = boardSize;
        }

        public int BoardSize { get; }
        public Node[] Nodes { get; }
    }
}