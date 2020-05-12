namespace View
{
    public interface INodeView
    {
        void SetListener(Controller.IChessboardNodeListener listener);

        void SetNode(Model.Node data);

        void ToggleHint(bool enable);

        void SetInteractable(bool value);

        void ToggleSelect(bool value);
    }
}