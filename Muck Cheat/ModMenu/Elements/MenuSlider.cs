using UnityEngine.UI;

public class MenuSlider : MenuElement
{
    public float minValue = 0,
                 maxValue = 1,
                 step = 0.1F,
                 value = 0;

    public ValueChangeCallback Change = delegate { };

    public MenuSlider()
    {
        LeftArrow = OnLeft;
        RightArrow = OnRight;
    }

    public override MenuButtonType GetElementType()
    {
        return MenuButtonType.Slider;
    }

    #region Controls

    void OnLeft()
    {
        float oldValue = value;
        value -= step;

        if (value > minValue)
        {
            value = minValue;
        }
        else
            Change(oldValue, value);
    }

    void OnRight()
    {
        float oldValue = value;
        value += step;

        if (value > maxValue)
        {
            value = maxValue;
        }
        else
            Change(oldValue, value);
    }

    #endregion

    public delegate void ValueChangeCallback(float oldValue, float newValue);
}
