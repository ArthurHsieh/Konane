using System.Collections.Generic;
using UnityEngine;
using Model;

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

        const int BOARD_SIZE = 8;
        Round mCurrentRound = Round.Black;
        RoundState mRoundState = RoundState.Opening;
        Model.Chessboard mChessboardData = null;
        List<int> mCurrentVacancyBlackNodes = new List<int>();
        List<int> mCurrentVacancyWhiteNodes = new List<int>();
        List<int> mActableNodes = new List<int>();
        int mSelectNodeIndex = -1;

        // Start is called before the first frame update
        void Start()
        {
            mChessboardData = Model.Model.GetChessboard(BOARD_SIZE);
            m_ChessboardView.ShowCheckerboard(mChessboardData, this);

            CheckRoundAndState();
        }

        public void OnClickNode(int index)
        {
            //Debug.LogFormat("OnClickNode index: {0}", index);
            switch (mRoundState)
            {
                case RoundState.Opening:
                    mChessboardData.Nodes[index].IsVacancy = true;
                    m_ChessboardView.ShowCheckerboard(mChessboardData, this);
                    if (mChessboardData.Nodes[index].IsWhite)
                        mCurrentVacancyWhiteNodes.Add(index);
                    else
                        mCurrentVacancyBlackNodes.Add(index);
                    if (mCurrentRound == Round.Black)
                    {
                        mCurrentRound = Round.White;
                    }
                    else
                    {
                        mCurrentRound = Round.Black;
                        mRoundState = RoundState.Battle;
                    }

                    CheckRoundAndState();
                    break;
                case RoundState.Battle:
                    CheckSelectAndHint(index);
                    break;
            }
        }

        public void OnSelectNode(int index)
        {
           // Debug.LogFormat("OnSelectNode index: {0}", index);
        }

        public void OnDeselectNode(int index)
        {
            //Debug.LogFormat("OnDeselectNode index: {0}", index);
        }

        void CheckSelectAndHint(int index)
        {
            if(mActableNodes.Contains(index))
            {
                EatChess(mSelectNodeIndex, index);
                m_ChessboardView.ClearSelect();

                if (mCurrentRound == Round.Black)
                    mCurrentRound = Round.White;
                else
                    mCurrentRound = Round.Black;
                CheckRoundAndState();
            }
            else
            {
                mSelectNodeIndex = index;
                m_ChessboardView.SetNodeSelect(mSelectNodeIndex);
                mActableNodes = GetHintNodes(mSelectNodeIndex);
                m_ChessboardView.SetNodeHint(mActableNodes);
            }
        }

        void EatChess(int from, int to)
        {
            var delta = to - from;
            List<int> chessBeEaten = new List<int>();
            if (delta % (BOARD_SIZE*2) == 0)
            {
                if (delta > 0)
                {
                    for (int i = from; i < to; i += BOARD_SIZE)
                        chessBeEaten.Add(i);
                }
                else
                {
                    for (int i = from; i > to; i -= BOARD_SIZE)
                        chessBeEaten.Add(i);
                }
            }
            else
            {
                if (delta > 0)
                {
                    for (int i = from; i < to; i++)
                        chessBeEaten.Add(i);
                }
                else
                {
                    for (int i = from; i > to; i--)
                        chessBeEaten.Add(i);
                }
            }

            //Debug.LogFormat("chessBeEaten: {0}", string.Join(", ", chessBeEaten));

            for (int i = 0; i < chessBeEaten.Count; ++i)
                mChessboardData.Nodes[chessBeEaten[i]].IsVacancy = true;
            mChessboardData.Nodes[to].IsVacancy = false;

            m_ChessboardView.ShowCheckerboard(mChessboardData, this);
        }

        List<int> GetHintNodes(int select)
        {
            var node = mChessboardData.Nodes[select];
            var flag = node.NeighborFlags;
            List<int> hintNodes = new List<int>();
            if (flag.HasFlag(NeighborFlags.Right))
                hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Right));
            if (flag.HasFlag(NeighborFlags.Left))
                hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Left));
            if (flag.HasFlag(NeighborFlags.Up))
                hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Up));
            if (flag.HasFlag(NeighborFlags.Down))
                hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Down));

            return hintNodes;
        }

        List<int> GetAvailablePath(Node node, NeighborFlags neighbor)
        {
            List<int> pathNodes = new List<int>();
            Node thePath = null;
            switch (neighbor)
            {
                case NeighborFlags.Right:
                    thePath = node.RightNode.RightNode;
                    break;
                case NeighborFlags.Left:
                    thePath = node.LeftNode.LeftNode;
                    break;
                case NeighborFlags.Up:
                    thePath = node.UpNode.UpNode;
                    break;
                case NeighborFlags.Down:
                    thePath = node.DownNode.DownNode;
                    break;
            }
            if (thePath != null && thePath.IsVacancy)
            {
                pathNodes.Add(thePath.Index);
                if (thePath.NeighborFlags.HasFlag(neighbor))
                    pathNodes.AddRange(GetAvailablePath(thePath, neighbor));
            }
            return pathNodes;
        }

        void CheckRoundAndState()
        {
            List<int> selectableNodes = new List<int>();
            switch (mRoundState)
            {
                case RoundState.Opening:
                    switch (mCurrentRound)
                    {
                        case Round.Black:
                            selectableNodes.Add(0);
                            selectableNodes.Add(27);
                            selectableNodes.Add(36);
                            selectableNodes.Add(63);
                            break;
                        case Round.White:
                            var currentVacancyBlackNode = mChessboardData.Nodes[mCurrentVacancyBlackNodes[0]];
                            var flags = currentVacancyBlackNode.NeighborFlags;
                            if (flags.HasFlag(NeighborFlags.Right))
                                selectableNodes.Add(currentVacancyBlackNode.RightNode.Index);
                            if (flags.HasFlag(NeighborFlags.Left))
                                selectableNodes.Add(currentVacancyBlackNode.LeftNode.Index);
                            if (flags.HasFlag(NeighborFlags.Up))
                                selectableNodes.Add(currentVacancyBlackNode.UpNode.Index);
                            if (flags.HasFlag(NeighborFlags.Down))
                                selectableNodes.Add(currentVacancyBlackNode.DownNode.Index);
                            break;
                    }
                    m_ChessboardView.SetNodeHint(selectableNodes);
                    m_ChessboardView.SetNodeInteractable(selectableNodes);
                    break;
                case RoundState.Battle:
                    for (int i = 0; i < mChessboardData.Nodes.Length; ++i)
                    {
                        switch(mCurrentRound)
                        {
                            case Round.White:
                                if (mChessboardData.Nodes[i].IsWhite)
                                    selectableNodes.Add(i);
                                break;
                            case Round.Black:
                                if (!mChessboardData.Nodes[i].IsWhite)
                                    selectableNodes.Add(i);
                                break;
                        }
                    }
                    m_ChessboardView.SetNodeHint(null);
                    m_ChessboardView.SetNodeInteractable(selectableNodes);
                    break;
                case RoundState.End:
                    break;
            }
        }

        void CheckBattleState()
        {
            if (mRoundState != RoundState.Battle)
                return;

            List<int> thisRoundVacancyNode = mCurrentRound == Round.Black ? mCurrentVacancyBlackNodes : mCurrentVacancyWhiteNodes;

            for(int i = 0; i < thisRoundVacancyNode.Count; ++i)
            {
                var node = Model.Model.GetNode(thisRoundVacancyNode[i]);
                if(node.NeighborFlags != NeighborFlags.None)
                {

                }
            }
        }

    }
}