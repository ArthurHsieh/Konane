using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

public class GameLogic : MonoBehaviour, IChessboardNodeListener
{
    [SerializeField]
    ChessboardView m_ChessboardView;

    public void OnClickNode(int index)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        var chessboardData = Model.Model.GetChessboard(8);
        m_ChessboardView.ShowCheckerboard(chessboardData, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
