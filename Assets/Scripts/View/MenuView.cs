using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MenuView : MonoBehaviour, IMenuView
{
    [SerializeField]
    RectTransform m_MenuRoot;
    [SerializeField]
    RectTransform m_GameOverRoot;
    [SerializeField]
    Button m_StartButton;
    [SerializeField]
    Button m_GameOverButton;
    [SerializeField]
    Text m_GameOverText;

    IMenuListener mListener = null;

    public void ShowMenu(IMenuListener listener)
    {
        mListener = listener;
        m_MenuRoot.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        m_MenuRoot.gameObject.SetActive(false);
    }

    public void ShowGameOver(Round winner)
    {
        m_GameOverText.text = string.Format("{0} Win!", winner);
        m_GameOverRoot.gameObject.SetActive(true);
    }

    public void CloseGameOver()
    {
        m_GameOverRoot.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_StartButton.onClick.AddListener(() => {
            mListener?.OnClickStart();
        });
        m_GameOverButton.onClick.AddListener(() => {
            mListener?.OnClickGameOver();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
