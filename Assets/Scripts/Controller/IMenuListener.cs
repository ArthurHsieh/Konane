using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public interface IMenuListener
    {
        void OnClickStart();

        void OnClickGameOver();
    }
}