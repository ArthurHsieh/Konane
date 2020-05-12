﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Model
    {
        static Chessboard mChessboard = null;

        public static Chessboard GetChessboard(int boardSize)
        {
            mChessboard = new Chessboard(boardSize);
            return mChessboard;
        }

        public static Node GetNode(int index)
        {
            if (mChessboard == null || index < 0 || index >= mChessboard.Nodes.Length)
                return null;

            return mChessboard.Nodes[index];
        }
    }
}