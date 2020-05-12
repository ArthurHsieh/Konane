using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public interface IChessboardNodeListener
    {
        void OnClickNode(int index);
        void OnSelectNode(int index);
        void OnDeselectNode(int index);
    }
}