using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Model
    {
        public static Chessboard GetChessboard(int boardSize)
        {
            return new Chessboard(boardSize);
        }
    }
}