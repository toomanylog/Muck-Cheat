using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuStringSelection : MenuElement
{
    private string selected;

    private List<string> strings;
    public SelectionChangeCallback SelectionChange = delegate { };

    public List<string> GetStrings()
    {
        return strings;
    }

    public void SetStrings(string[] strings)
    {
        this.strings = new List<string>(strings);

        SetSelected(this.strings[0]);
        ChangeSecondText(selected);
    }

    public void SetSelected(string selected)
    {
        this.selected = selected;
        int i = strings.IndexOf(selected);

        if (i == 0)
        {
            leftIcon.gameObject.SetActive(false);
        }

        if (i >= strings.Count)
        {
            rightIcon.gameObject.SetActive(false);
        }
    }

    public string GetSelected() 
    {
        return selected;
    }

    public override MenuButtonType GetElementType()
    {
        return MenuButtonType.Selection;
    }

    public MenuStringSelection()
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
        int i = strings.IndexOf(selected);
        i--;

        if (i == 0)
        {
            leftIcon.gameObject.SetActive(false);
        }

        if (i < 0)
        {
            i = 0;
            return;
        }

        rightIcon.gameObject.SetActive(true);

        selected = strings[i];

        ChangeSecondText(selected);

        SelectionChange(oldValue, selected);
    }

    void OnRight()
    {
        string oldValue = selected;
        int i = strings.IndexOf(selected);
        i++;


        if (i == strings.Count)
        {
            rightIcon.gameObject.SetActive(false);
            i = strings.Count - 1;
            selected = strings[0];
            return;
        }

        leftIcon.gameObject.SetActive(true);

        selected = strings[i];

        ChangeSecondText(selected);

        SelectionChange(oldValue, selected);
    }

    #endregion

    public delegate void SelectionChangeCallback(string oldValue, string newValue);

}