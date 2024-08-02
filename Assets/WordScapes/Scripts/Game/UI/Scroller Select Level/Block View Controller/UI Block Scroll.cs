using EnhancedUI.EnhancedScroller;

public class UIBlockScroll : EnhancedScrollerCellView
{
    protected ScrollViewData _data;

    public virtual void SetData(ScrollViewData data)
    {
        _data = data;
    }
}
