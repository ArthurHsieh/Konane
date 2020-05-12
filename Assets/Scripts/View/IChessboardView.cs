using System.Collections.Generic;

namespace View
{
    public interface IChessboardView
    {
        void ShowCheckerboard(Model.Chessboard checkerboardData, Controller.IChessboardNodeListener listener);

        void SetNodeInteractable(List<int> interactableNodes);

        void SetNodeHint(List<int> hintNodes);

        void SetNodeSelect(int index);

        void ClearSelect();
    }
}