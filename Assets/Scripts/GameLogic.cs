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

    public class GameLogic : MonoBehaviour, IChessboardNodeListener, IMenuListener
    {
        [SerializeField]
        ChessboardView m_ChessboardView;
        [SerializeField]
        MenuView m_MenuView;

        const int BOARD_SIZE = 8;
        Round mCurrentRound = Round.Black;
        RoundState mRoundState = RoundState.Opening;
        Chessboard mChessboardData = null;
        List<int> mCurrentVacancyBlackNodes = new List<int>();
        List<int> mCurrentVacancyWhiteNodes = new List<int>();
        List<int> mActableNodes = new List<int>();
        int mSelectNodeIndex = -1;

        // Start is called before the first frame update
        void Start()
        {
            PrepareChessboard();
            m_MenuView.CloseCurrentRound();
            m_MenuView.CloseGameOver();
            m_MenuView.ShowMenu(this);
        }

        public void OnClickNode(int index)
        {
            //Debug.LogFormat("OnClickNode index: {0}", index);
            switch (mRoundState)
            {
                case RoundState.Opening:
                    OnClickWhenOpening(index);
                    break;
                case RoundState.Battle:
                    OnClickWhenBattle(index);
                    break;
            }
        }

        void OnClickWhenOpening(int index)
        {
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
        }

        void OnClickWhenBattle(int index)
        {
            if(mActableNodes.Contains(index))
            {
                EatChess(mSelectNodeIndex, index);
                m_ChessboardView.ClearSelect();
                m_ChessboardView.SetNodeHint(null);

                if (mCurrentRound == Round.Black)
                    mCurrentRound = Round.White;
                else
                    mCurrentRound = Round.Black;

                if (IsGameEnd())
                {
                    GameOver(mCurrentRound);
                }else
                {
                    CheckRoundAndState();
                }
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
            {
                mChessboardData.Nodes[chessBeEaten[i]].IsVacancy = true;
                if (mChessboardData.Nodes[chessBeEaten[i]].IsWhite)
                    mCurrentVacancyWhiteNodes.Add(chessBeEaten[i]);
                else
                    mCurrentVacancyBlackNodes.Add(chessBeEaten[i]);
            }
            mChessboardData.Nodes[to].IsVacancy = false;
            if (mChessboardData.Nodes[to].IsWhite)
                mCurrentVacancyWhiteNodes.Remove(to);
            else
                mCurrentVacancyBlackNodes.Remove(to);

            m_ChessboardView.ShowCheckerboard(mChessboardData, this);
        }

        List<int> GetHintNodes(int select)
        {
            var node = mChessboardData.Nodes[select];
            List<int> hintNodes = new List<int>();

            if(!node.IsVacancy)
            {
                var flag = node.NeighborFlags;
                if (flag.HasFlag(NeighborFlags.Right))
                    hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Right));
                if (flag.HasFlag(NeighborFlags.Left))
                    hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Left));
                if (flag.HasFlag(NeighborFlags.Up))
                    hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Up));
                if (flag.HasFlag(NeighborFlags.Down))
                    hintNodes.AddRange(GetAvailablePath(node, NeighborFlags.Down));
            }

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
            }
            m_MenuView.ShowCurrentRound(mCurrentRound);
        }

        bool IsGameEnd()
        {
            switch(mRoundState)
            {
                case RoundState.Battle:
                    List<int> thisRoundVacancyNode = mCurrentRound == Round.Black ? mCurrentVacancyBlackNodes : mCurrentVacancyWhiteNodes;
                    bool gameEnd = true;
                    for (int i = 0; i < thisRoundVacancyNode.Count; ++i)
                    {
                        var node = mChessboardData.Nodes[thisRoundVacancyNode[i]];
                        if (node.NeighborFlags != NeighborFlags.None)
                        {
                            if (node.NeighborFlags.HasFlag(NeighborFlags.Down) && node.DownNode.NeighborFlags.HasFlag(NeighborFlags.Down))
                            {
                                gameEnd = false;
                                break;
                            }
                            if (node.NeighborFlags.HasFlag(NeighborFlags.Up) && node.UpNode.NeighborFlags.HasFlag(NeighborFlags.Up))
                            {
                                gameEnd = false;
                                break;
                            }
                            if (node.NeighborFlags.HasFlag(NeighborFlags.Right) && node.RightNode.NeighborFlags.HasFlag(NeighborFlags.Right))
                            {
                                gameEnd = false;
                                break;
                            }
                            if (node.NeighborFlags.HasFlag(NeighborFlags.Left) && node.LeftNode.NeighborFlags.HasFlag(NeighborFlags.Left))
                            {
                                gameEnd = false;
                                break;
                            }
                        }
                    }
                    return gameEnd;
                case RoundState.Opening:
                    return false;
            }
            return true;
        }

        void PrepareChessboard()
        {
            mChessboardData = Model.Model.GetChessboard(BOARD_SIZE, true);
            m_ChessboardView.ShowCheckerboard(mChessboardData, this);
        }

        public void GameStart()
        {
            mChessboardData = Model.Model.GetChessboard(BOARD_SIZE);
            m_ChessboardView.ShowCheckerboard(mChessboardData, this);
            mCurrentRound = Round.Black;
            mRoundState = RoundState.Opening;

            mCurrentVacancyBlackNodes.Clear();
            mCurrentVacancyWhiteNodes.Clear();
            mActableNodes.Clear();
            mSelectNodeIndex = -1;

            CheckRoundAndState();
        }

        void GameOver(Round loser)
        {
            m_MenuView.CloseCurrentRound();
            m_MenuView.ShowGameOver(loser == Round.Black? Round.White: Round.Black);
        }

        public void OnClickStart()
        {
            m_MenuView.CloseMenu();
            GameStart();
        }

        public void OnClickGameOver()
        {
            m_MenuView.CloseGameOver();
            m_MenuView.ShowMenu(this);
        }
    }
}