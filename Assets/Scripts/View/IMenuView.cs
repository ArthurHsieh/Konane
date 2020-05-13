using Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public interface IMenuView
    {
        void ShowMenu(IMenuListener listener);

        void CloseMenu();

        void ShowGameOver(Round winner);

        void CloseGameOver();
    }
}