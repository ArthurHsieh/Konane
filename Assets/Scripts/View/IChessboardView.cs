using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public interface IChessboardView
    {
        void ShowCheckerboard(Model.Chessboard checkerboardData, Controller.IChessboardNodeListener listener);

        void SetNodeInteractable(List<int> interactableNodes);

        void SetNodeHint(List<int> hintNodes);
    }
}