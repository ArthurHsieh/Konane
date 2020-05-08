using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public interface IChessboardView
    {
        void ShowCheckerboard(Model.Chessboard checkerboardData, Controller.IChessboardNodeListener listener);
    }
}