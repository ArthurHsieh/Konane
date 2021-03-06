﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class ChessboardView : MonoBehaviour, IChessboardView
{
    [SerializeField]
    RectTransform m_Root;
    [SerializeField]
    GridLayoutGroup m_GridLayoutGroup;
    [SerializeField]
    NodeView m_ChessboardNodeView;

    List<NodeView> mNodes = new List<NodeView>();
    NodeView mSelectNode = null;
    int mBoardSize = 0;

    public void ShowCheckerboard(Model.Chessboard checkerboardData, Controller.IChessboardNodeListener listener)
    {
        if(mBoardSize != checkerboardData.BoardSize)
        {
            mBoardSize = checkerboardData.BoardSize;
            m_GridLayoutGroup.cellSize = m_Root.rect.size / checkerboardData.BoardSize;
            m_GridLayoutGroup.constraintCount = checkerboardData.BoardSize;
        }

        var nodesData = checkerboardData.Nodes;

        if (mNodes.Count < nodesData.Length)
            LoadNodes(nodesData.Length - mNodes.Count);

        for(int i = 0; i < mNodes.Count; ++i)
        {
            if(i < nodesData.Length)
            {
                if(!mNodes[i].gameObject.activeSelf)
                    mNodes[i].gameObject.SetActive(true);
                mNodes[i].SetNode(nodesData[i]);
                mNodes[i].SetListener(listener);
            }
            else
            {
                if (mNodes[i].gameObject.activeSelf)
                    mNodes[i].gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        m_ChessboardNodeView.gameObject.SetActive(false);
    }

    void LoadNodes(int count)
    {
        if (count == 0)
            return;

        for(int i = 0; i < count; ++i)
        {
            var node = Instantiate(m_ChessboardNodeView, m_Root.transform);
            node.name = m_ChessboardNodeView.name + mNodes.Count;
            mNodes.Add(node);
        }
    }

    public void SetNodeInteractable(List<int> interactableNodes)
    {
        for(int i = 0; i < mNodes.Count; ++i)
        {
            mNodes[i].SetInteractable(interactableNodes != null ? interactableNodes.Contains(i) : false);
        }
    }

    public void SetNodeHint(List<int> hintNodes)
    {
        for (int i = 0; i < mNodes.Count; ++i)
        {
            mNodes[i].ToggleHint(hintNodes != null ? hintNodes.Contains(i) : false);
        }
    }

    public void SetNodeSelect(int index)
    {
        ClearSelect();
        mSelectNode = mNodes[index];
        mSelectNode?.ToggleSelect(true);
    }

    public void ClearSelect()
    {
        mSelectNode?.ToggleSelect(false);
        mSelectNode = null;
    }
}
