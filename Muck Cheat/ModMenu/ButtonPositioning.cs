﻿using System;
using UnityEngine;

public class ButtonPositioning
{
    private float
            x, y,
            margin,
            controlHeight,
            controlWidth,
            controlDist,
            nextControlY;
    private Utils.TitleInfo title = null;

    public ButtonPositioning(Utils.TitleInfo title, float x, float y, float margin, float controlHeight, float controlWidth, float controlDist)
    : this(x, y, margin, controlHeight, controlWidth, controlDist)
    {
        this.title = title;
    }

    public ButtonPositioning(float x, float y, float margin, float controlHeight, float controlWidth, float controlDist)
    {
        this.x = x;
        this.y = y;
        this.controlWidth = controlWidth;
        this.margin = margin;
        this.controlHeight = controlHeight;
        this.controlDist = controlDist;
        nextControlY = y + 20;
    }

    public Rect GetSecondTextPos()
    {
        return new Rect(Mathf.Round((controlWidth - margin * 2) / 7.9F), 0, 100, controlHeight);
    }

    public Rect GetIconPos(MenuIcon icon)
    {
        switch(icon)
        {
            //LeftIcon
            case MenuIcon.Left:
                return new Rect(-Mathf.Round((controlWidth - margin * 2) / 12.5F), 0, 100, controlHeight);
            //RightIcon
            case MenuIcon.Right:
                return new Rect((controlWidth - margin * 2) / 3F, 0, 100, controlHeight);
            //RightIcon2
            case MenuIcon.Right2:
                return new Rect((controlWidth - margin * 2) / 2.5F, 0, controlHeight, controlHeight);
            default:
                return new Rect(0, 0, 0, 0);
        }
    }

    public void SetElements(int elements)
    {
        //TODO: FIX
        float newY = (elements * (controlHeight + controlDist) - controlDist);
        if(y != newY)
        {
            y -= newY / (elements + 1);
            Reset();
        }
    }

    public void ChangePos(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Rect GetBackgroundRect(int elements, float middleButtonY)
    {
        Rect rect = new Rect(x, middleButtonY, controlWidth + margin * 2 + 10, elements * (controlHeight + controlDist + 5) - controlDist);
        if (title != null && !String.IsNullOrEmpty(title.text))
        {
            int fontHeight = title.font.lineHeight / 2 + 1;
            rect.height += fontHeight;
            rect.y += fontHeight;
        }
        return rect;
    }

    public void Reset()
    {
        nextControlY = y + 20;
    }

    public Rect GetTitlePosition()
    {
        //return new Rect(x, middleButtonY)
        return new Rect();
    }

    public Rect NextControlRect(bool reverseY)
    {
        //Rect r = new Rect(x, nextControlY, width - margin * 2, controlHeight);
        Rect r = new Rect(x + margin, nextControlY, controlWidth - margin * 2, controlHeight);
        float next = controlHeight + controlDist;
        if (reverseY)
        {
            nextControlY -= next;
        }
        else
            nextControlY += next;
        return r;
    }
}