# Konane

開啟方式
===

1. 打開Asset/Scenes/Play.unity
2. 運行場景

遊戲規則
===

黑子先手，初步必須先從指示的棋子挑出一顆除去；白子再從相鄰的棋子中挑出一顆除去。

之後則以跳棋的方式移動，只能橫豎移動，不可斜向移動，跳過對手棋子將其除去。

若棋子有可能連跳，則玩家可選擇連跳，但不可以在連跳途中變換行進方向。

當一方無法走棋時遊戲結束，該方為負。

詳細可參考 [規則](https://brainking.com/cn/GameRules?tp=94)

專案內容概述
===
使用了MVC的架構，將大部分邏輯集中於GameLogic，減少資料與顯示方面的程式複雜度。
