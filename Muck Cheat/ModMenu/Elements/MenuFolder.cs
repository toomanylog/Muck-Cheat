using System.Collections.Generic;
public class MenuFolder : MenuElement
{
    private List<MenuElement> elements;
    public int totalElements { get { return _totalElements; } }
    private int _totalElements;
    public int selectedElementIndex;
    public MenuElement selectedElement;
    private ModMenu menu;

    public List<MenuElement> GetElements()
    {
        return elements;
    }

    #region Element registry
    public void RegisterElement(MenuElement element)
    {
        if (menu.isRendering())
        {
            elements.Add(element);

            element.gameObject.SetActive(false);

            _totalElements = elements.Count;
            menu.PositionButtons();
        }
    }

    public void UnregisterElement(MenuElement element)
    {
        if (menu.isRendering())
        {
            elements.Remove(element);
            _totalElements = elements.Count;
            menu.PositionButtons();
        }
    }
    #endregion

    public MenuFolder()
    {
        menu = ModMenu.Instance;
        Pressed = OnPress;
        elements = new List<MenuElement>();

        MenuButton button = menu.CreateButton("Back");
        button.Pressed = Back;
        button.AddIcons(new MenuIcon[1] { MenuIcon.Left });
        button.leftIcon.sprite = Utils.Base64ToSprite(Base64Icons.LessThan);

        RegisterElement(button);
    }

    void Back()
    {
        menu.SetCurrentFolder(null);
    }

    void OnPress()
    {
        ModMenu.Instance.SetCurrentFolder(this);
    }

}