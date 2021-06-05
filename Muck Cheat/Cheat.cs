using UnityEngine;
public class Cheat : MonoBehaviour
{
    private ModMenu menu;

    public void Init()
    {
        if (menu == null) menu = ModMenu.Instance;
        menu.StartRendering(670, 300, 500, 1, 100, 500, 5, 20);
        menu.SetTextAllign(TextAnchor.MiddleLeft);

        menu.RegisterElement(menu.CreateButton("Button"));

    }

    void Start()
    {
        menu = ModMenu.Instance;
        Init();
    }
}