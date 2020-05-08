using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View;

public class NodeView : MonoBehaviour, INodeView, IPointerDownHandler
{
    [SerializeField]
    Image m_BG;
    [SerializeField]
    Image m_Chess;
    [SerializeField]
    Image m_Hint;
    [SerializeField]
    Sprite[] m_BGSprites;
    [SerializeField]
    Color[] m_ChessColor;

    Model.Node mThisNodeData = null;
    Controller.IChessboardNodeListener listener = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        listener?.OnClickNode(mThisNodeData.Index);
    }

    public void SetListener(Controller.IChessboardNodeListener listener)
    {
        this.listener = listener;
    }

    public void SetNode(Model.Node data)
    {
        mThisNodeData = data;
        m_BG.sprite = mThisNodeData.IsWhite ? m_BGSprites[0] : m_BGSprites[1];
        m_Chess.color = mThisNodeData.IsWhite ? m_ChessColor[0] : m_ChessColor[1];
        m_BG.enabled = !data.IsVacancy;
    }

    public void ToggleHint(bool enable)
    {
        m_Hint.enabled = enable;
    }
}
