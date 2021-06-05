public class MenuLabel : MenuElement
{
    public bool selectable = true;

    public MenuLabel()
    {
        Select = OnSelect;
    }

    void OnSelect(bool fromAbove)
    {
        if(selectable)
        {
            if (fromAbove)
            {
                ModMenu.Instance.SelectNextButton();
            }
            else
                ModMenu.Instance.SelectPreviousButton();
        }
    }
}
