using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MenuElement : MonoBehaviour
{
    private Button button;
    public Image leftIcon,
                 rightIcon,
                 rightIcon2;

    private MenuIcon[] icons = new MenuIcon[0];

    private bool initialized;

    public InitializeCallback InitCallback = delegate { };
    public LeftArrowCallback LeftArrow = delegate { };
    public RightArrowCallback RightArrow = delegate { };
    public PressCallback Pressed = delegate { };
    public SelectCallback Select = delegate { };

    #region Getters and Setters

    public bool HasIcon(MenuIcon icon)
    {
        foreach (MenuIcon i in icons) if (i == icon) return true;
        return false;
    }

    public bool IsSelected()
    {
        return EventSystem.current.currentSelectedGameObject == gameObject;
    }

    public Button GetButton()
    {
        return button;
    }

    public virtual MenuButtonType GetElementType()
    {
        return MenuButtonType.Unkown;
    }

    #endregion

    #region Text

    public void ChangeText(string newText)
    {
        GetTextInstance().text = newText;
    }

    public bool hasSecondText()
    {
        return button.transform.Find("Text 2") != null;
    }

    public void ChangeSecondText(string newText)
    {
        GetSecondTextInstance().text = newText;
    }

    public Text GetSecondTextInstance()
    {
        Transform obj = button.transform.Find("Text 2");
        if (obj == null)
        {
            AddSecondText("null");
            return GetSecondTextInstance();
        }
        else
            return obj.GetComponent<Text>();
    }

    public void AddSecondText(string text)
    {
        Text txt = (Text)Utils.CreateUIElement<Text>(out GameObject sT);
        txt.fontSize = Utils.fontSize;
        txt.font = Font.CreateDynamicFontFromOSFont("Arial", Utils.fontSize);
        txt.text = text;
        txt.alignment = TextAnchor.MiddleCenter;
        sT.name = "Text 2";
        sT.transform.SetParent(transform);
    }

    /// <summary>
    /// Gets the Text instance of the Button
    /// </summary>
    /// <returns>Text instance of the Element.</returns>
    public Text GetTextInstance()
    {
        return button.transform.Find("Text").gameObject.GetComponent<Text>();
    }
    #endregion

    #region Icons
    public void AddIcons(MenuIcon[] icons)
    {
        this.icons = icons;

        foreach (MenuIcon icon in icons)
        {
            switch (icon)
            {
                case MenuIcon.Left:
                    if (leftIcon != null) break;
                    leftIcon = (Image)Utils.CreateUIElement<Image>(out GameObject lObj);
                    lObj.transform.SetParent(gameObject.transform);
                    lObj.name = "Left Icon";
                    break;
                case MenuIcon.Right:
                    if (rightIcon != null) break;
                    rightIcon = (Image)Utils.CreateUIElement<Image>(out GameObject rOBj);
                    rOBj.transform.SetParent(gameObject.transform);
                    rOBj.name = "Right Icon";
                    break;
                case MenuIcon.Right2:
                    if (rightIcon2 != null) break;
                    rightIcon2 = (Image)Utils.CreateUIElement<Image>(out GameObject r2Obj);
                    r2Obj.transform.SetParent(gameObject.transform);
                    r2Obj.name = "Right Icon 2";
                    break;
            }
        }
    }
    #endregion


    public void Init()
    {
        if(!initialized)
        {
            button = GetComponent<Button>();
            initialized = true;
            InitCallback(button);
        }
    }

    #region Callbacks
    public delegate void InitializeCallback(Button button);
    public delegate void SelectCallback(bool fromAbove);

    #region Control Callbacks
    public delegate void LeftArrowCallback();
    public delegate void RightArrowCallback();
    public delegate void PressCallback();
    #endregion
    #endregion
}
