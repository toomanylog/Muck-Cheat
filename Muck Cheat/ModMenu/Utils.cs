using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Utils
{
    public class TitleInfo
    {
        public Font font;
        public string text;

        public TitleInfo(Font font, string text)
        {
            this.font = font;
            this.text = text;
        }
    }
    public static int fontSize;

    public static Sprite Base64ToSprite(string base64)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public static T GetFieldValue<T>(object obj, string fieldName)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }
        FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field == null)
        {
            throw new ArgumentException("fieldName", "No such field was found.");
        }
        if (!typeof(T).IsAssignableFrom(field.FieldType))
        {
            throw new InvalidOperationException("Field type and requested type are not compatible.");
        }
        return (T)((object)field.GetValue(obj));
    }

    public static void SetFieldValue<T>(object obj, string fieldName, object value)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }
        FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field == null)
        {
            throw new ArgumentException("fieldName", "No such field was found.");
        }
        if (!typeof(T).IsAssignableFrom(field.FieldType))
        {
            throw new InvalidOperationException("Field type and requested type are not compatible.");
        }
        field.SetValue(obj, (T)((object)value));
    }

    public static Component CreateUIElement<T>(out GameObject obj) where T : Component
    {
        obj = new GameObject();

        obj.AddComponent<CanvasRenderer>();
        obj.AddComponent<RectTransform>();

        RectTransform rTrans = obj.GetComponent<RectTransform>();
        rTrans.localPosition = Vector3.zero;
        rTrans.anchoredPosition = Vector3.zero;

        return obj.AddComponent<T>();
    }

    public static Button CreateButton(out GameObject obj, out Text text, TextAnchor textAllign)
    {
        Button button = (Button)CreateUIElement<Button>(out obj);

        button.targetGraphic = obj.AddComponent<Image>();

        ColorBlock block = button.colors;

        Color clr = Color.gray;

        block.selectedColor = clr;
        block.highlightedColor = clr;
        block.pressedColor = clr;
        button.colors = block;

        Navigation nav = button.navigation;
        nav.mode = Navigation.Mode.None;
        button.navigation = nav;

        text = (Text)CreateUIElement<Text>(out GameObject txt);
        txt.name = "Text";
        txt.transform.SetParent(obj.transform);

        text.text = "Button";
        text.color = Color.black;
        text.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
        text.fontSize = fontSize;
        text.alignment = textAllign;

        return button;
    }
}