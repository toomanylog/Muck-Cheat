using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuSelection : MenuElement
{
    private string selected;
    private Type enumType;

    private List<string> enumValues;
    public SelectionChangeCallback SelectionChange = delegate { };

    public void SetEnum<T>() where T : Enum
    {
        if(enumType == null)
        {
            enumType = typeof(T);
            enumValues = new List<string>(Enum.GetNames(enumType));

            SetSelected(enumValues[0]);
            ChangeSecondText(selected);
        }
    }

    private void SetSelected(string selected)
    {
        this.selected = selected;
        int i = enumValues.IndexOf(selected);

        if (i == 0)
        {
            leftIcon.gameObject.SetActive(false);
        }

        if (i >= enumValues.Count)
        {
            rightIcon.gameObject.SetActive(false);
        }
    }

    public void SetSelected(object value)
    {
        SetSelected(Enum.GetName(enumType, value));
    }

    public T GetSelected<T>() where T : Enum
    {
        if (typeof(T) != enumType) return default;
        return (T)Enum.Parse(enumType, selected);
    }

    public override MenuButtonType GetElementType()
    {
        return MenuButtonType.Selection;
    }

    public MenuSelection()
    {
        LeftArrow = OnLeft;
        RightArrow = OnRight;
        InitCallback = OnInit;
    }

    void OnInit(Button button)
    {
        AddIcons(new MenuIcon[2] { MenuIcon.Left, MenuIcon.Right });
        AddSecondText("null");

        leftIcon.sprite = Utils.Base64ToSprite(Base64Icons.LessThan);
        rightIcon.sprite = Utils.Base64ToSprite(Base64Icons.GreaterThan);
    }

    #region Controls
    void OnLeft()
    {
        string oldValue = selected;
        int i = enumValues.IndexOf(selected);
        i--;

        if(i == 0)
        {
            leftIcon.gameObject.SetActive(false);
        }

        if(i < 0)
        {
            i = 0;
            return;
        }

        rightIcon.gameObject.SetActive(true);

        selected = enumValues[i];

        ChangeSecondText(selected);

        SelectionChange(oldValue, selected);
    }

    void OnRight()
    {
        string oldValue = selected;
        int i = enumValues.IndexOf(selected);
        i++;


        if (i >= enumValues.Count)
        {
            rightIcon.gameObject.SetActive(false);
            i = enumValues.Count - 1;
            return;
        }

        leftIcon.gameObject.SetActive(true);

        selected = enumValues[i];

        ChangeSecondText(selected);

        SelectionChange(oldValue, selected);
    }

    #endregion

    public delegate void SelectionChangeCallback(string oldValue, string newValue);

}