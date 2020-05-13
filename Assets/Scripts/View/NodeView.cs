using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View;

public class NodeView : MonoBehaviour, INodeView
{
    [SerializeField]
    Image m_BG;
    [SerializeField]
    Image m_Chess;
    [SerializeField]
    Image m_Hint;
    [SerializeField]
    Image m_Select;
    [SerializeField]
    Sprite[] m_BGSprites;
    [SerializeField]
    Color[] m_ChessColor;
    [SerializeField]
    Button m_Button;

    Model.Node mThisNodeData = null;
    Controller.IChessboardNodeListener listener = null;

    void Start()
    {
        m_Button.onClick.AddListener(() => {
            listener?.OnClickNode(mThisNodeData.Index);
        });
    }

    public void SetInteractable(bool value)
    {
        m_Button.interactable = value;
    }

    public void SetListener(Controller.IChessboardNodeListener listener)
    {
        this.listener = listener;
    }

    public void SetNode(Model.Node data)
    {
        mThisNodeData = data;
        m_BG.sprite = mThisNodeData.IsWhite ? m_BGSprites[1] : m_BGSprites[0];
        m_Chess.color = mThisNodeData.IsWhite ? m_ChessColor[1] : m_ChessColor[0];
        m_Chess.enabled = !data.IsVacancy;
    }

    public void ToggleHint(bool enable)
    {
        m_Hint.enabled = enable;
    }

    public void ToggleSelect(bool value)
    {
        m_Select.enabled = value;
    }
}
