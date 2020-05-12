using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

namespace Controller
{
    public enum Round
    {
        Black,
        White,
    }

    public enum RoundState
    {
        Opening,
        Battle,
        End,
    }

    public class GameLogic : MonoBehaviour, IChessboardNodeListener
    {
        [SerializeField]
        ChessboardView m_ChessboardView;

        Round mCurrentRound = Round.Black;
        RoundState mRoundState = RoundState.Opening;
        Model.Chessboard mChessboardData = null;
        List<int> mCurrentInteractableNodes = new List<int>();
        List<int> mCurrentHintNodes = new List<int>();        

        public void OnClickNode(int index)
        {
            switch(mRoundState)
            {
                case RoundState.Opening:
                    mChessboardData.Nodes[index].IsVacancy = true;
                    m_ChessboardView.ShowCheckerboard(mChessboardData, this);
                    break;
            }
            Debug.LogFormat("OnClickNode index: {0}", index);
        }

        // Start is called before the first frame update
        void Start()
        {
            mChessboardData = Model.Model.GetChessboard(8);
            m_ChessboardView.ShowCheckerboard(mChessboardData, this);

            CheckRoundAndState();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CheckRoundAndState()
        {
            mCurrentInteractableNodes.Clear();
            mCurrentHintNodes.Clear();
            switch (mRoundState)
            {
                case RoundState.Opening:
                    for (int i = 0; i < mChessboardData.Nodes.Length; ++i)
                    {
                        switch (mCurrentRound)
                        {
                            case Round.White:
                                if (mChessboardData.Nodes[i].IsWhite)
                                    mCurrentInteractableNodes.Add(i);
                                break;
                            case Round.Black:
                                if (!mChessboardData.Nodes[i].IsWhite &&
                                    mChessboardData.Nodes[i].X == mChessboardData.Nodes[i].Y &&
                                    (mChessboardData.Nodes[i].X == 0 || mChessboardData.Nodes[i].X == 3 || mChessboardData.Nodes[i].X == 4 || mChessboardData.Nodes[i].X == 7)
                                    )
                                {
                                    mCurrentInteractableNodes.Add(i);
                                    mCurrentHintNodes.Add(i);
                                }
                                break;
                        }
                    }
                    m_ChessboardView.SetNodeHint(mCurrentHintNodes);
                    m_ChessboardView.SetNodeInteractable(mCurrentInteractableNodes);
                    break;
                case RoundState.Battle:
                    for(int i = 0; i < mChessboardData.Nodes.Length; ++i)
                    {
                        switch(mCurrentRound)
                        {
                            case Round.White:
                                if (mChessboardData.Nodes[i].IsWhite)
                                    mCurrentInteractableNodes.Add(i);
                                break;
                            case Round.Black:
                                if (!mChessboardData.Nodes[i].IsWhite)
                                    mCurrentInteractableNodes.Add(i);
                                break;
                        }
                    }
                    m_ChessboardView.SetNodeInteractable(mCurrentInteractableNodes);
                    break;
                case RoundState.End:
                    m_ChessboardView.SetNodeHint(null);
                    m_ChessboardView.SetNodeInteractable(null);
                    break;
            }
        }
    }
}