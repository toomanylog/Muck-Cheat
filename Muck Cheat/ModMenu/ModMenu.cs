using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModMenu : MonoBehaviour
{
    public static ModMenu Instance;
    private Canvas canvas;
    private GameObject mainObject;
    private GameObject eventSystem;
    private GameObject panelObj;

    #region ElementVariables
    private List<MenuElement> elements;
    private int totalElements;
    private int selectedElementIndex;
    private MenuElement selectedElement;
    private ButtonPositioning _pos;
    private MenuFolder currentFolder;

    public bool InFolder()
    {
        return currentFolder != null;
    }

    public void SetCurrentFolder(MenuFolder folder)
    {
        currentFolder = folder;
    }

    public ButtonPositioning pos { get { return _pos;} }

    private TextAnchor textAllign = TextAnchor.MiddleCenter;

    public List<MenuElement> GetElements()
    {
        return elements;
    }

    public MenuElement GetSelectedElement()
    {
        return selectedElement;
    }

    public bool isRendering()
    {
        return _pos != null;
    }
    #endregion

    #region MenuVariables
    private float posX = 100F,
                  posY = 100F;

    private bool drawGUI = false;

    public bool IsDrawing()
    {
        return drawGUI;
    }
    #endregion

    #region Element Creation

    public void SetTextAllign(TextAnchor allign)
    {
        textAllign = allign;
    }

    private T CreateElement<T>(string label) where T : MenuElement
    {
        if (!isRendering()) return null;

        Utils.CreateButton(out GameObject obj, out Text text, textAllign);

        obj.transform.SetParent(mainObject.transform);
        text.text = label;
        obj.AddComponent<T>();
        obj.name = label;

        T element = obj.GetComponent<T>();

        return element;
    }

    public MenuButton CreateButton(string label)
    {
        if (!isRendering()) return null;

        MenuButton mButton = CreateElement<MenuButton>(label);

        mButton.Init();

        return mButton;
    }

    public MenuSlider CreateSlider(string label)
    {
        if (!isRendering()) return null;

        MenuSlider mSlider = CreateElement<MenuSlider>(label);

        mSlider.Init();

        return mSlider;
    }

    public MenuSelection CreateSelection<T>(string label) where T : Enum
    {
        if (!isRendering()) return null;

        MenuSelection mSelection = CreateElement<MenuSelection>(label);

        mSelection.Init();
        mSelection.SetEnum<T>();

        return mSelection;
    }

    public MenuStringSelection CreateStringSelection(string label)
    {
        if (!isRendering()) return null;

        MenuStringSelection mStringSelection = CreateElement<MenuStringSelection>(label);

        mStringSelection.Init();

        return mStringSelection;
    }

    public MenuLabel CreateLabel(string label)
    {
        if (!isRendering()) return null;

        MenuLabel mSelection = CreateElement<MenuLabel>(label);

        mSelection.Init();

        return mSelection;
    }

    public MenuFolder CreateFolder(string label)
    {
        if (!isRendering()) return null;

        MenuFolder mFolder = CreateElement<MenuFolder>(label);

        mFolder.Init();

        return mFolder;
    }

    #endregion

    #region Element registry
    /// <summary>
    /// Registers a Element of type <see cref="MenuElement"/>.
    /// </summary>
    /// <param name="element"></param>
    public void RegisterElement(MenuElement element)
    {
        if(isRendering())
        {
            elements.Add(element);
            totalElements = elements.Count;
            PositionButtons();
        }
    }

    public void UnregisterElement(MenuElement element)
    {
        if(isRendering())
        {
            elements.Remove(element);
            totalElements = elements.Count;
            PositionButtons();
        }
    }
    #endregion

    #region Initialization
    /// <summary>
    /// Starts the rendering of the Elements.
    /// </summary>
    /// <param name="elementHeight">Height of every <see cref="MenuElement"/></param>
    /// <param name="elementSpace">Space between each <see cref="MenuElement"/></param>
    public void StartRendering(float x, float y, float height,
        float margin, float elementHeight, float elementWidth, float elementSpace, int buttonFontSize)
    {
        //Initialize Positioning Tool
        _pos = new ButtonPositioning(posX = x, posY = y, margin, elementHeight, elementWidth, elementSpace);
        Utils.fontSize = buttonFontSize;

        //Create Panel
        Image img = (Image)Utils.CreateUIElement<Image>(out panelObj);
        panelObj.name = "Background";
        panelObj.transform.transform.SetParent(mainObject.transform);

        img.color = new Color32(128, 128, 128, 128);
    }

    public void StartRendering(float x, float y, float height,
        float margin, float elementHeight, float elementWidth, float elementSpace, int buttonFontSize, Utils.TitleInfo tInfo)
    {
        StartRendering(x, y, height, margin, elementHeight, elementWidth, elementSpace, buttonFontSize);
        _pos = new ButtonPositioning(tInfo, posX = x, posY = y, margin, elementHeight, elementWidth, elementSpace);

        Text text = (Text)Utils.CreateUIElement<Text>(out GameObject textObj);
        text.text = tInfo.text;
        textObj.name = "Title";
        textObj.transform.SetParent(mainObject.transform);

        RectTransform tTrans = textObj.GetComponent<RectTransform>();
    }
    public static ModMenu CreateModMenuObject()
    {
        GameObject modMenu = new GameObject();
        modMenu.AddComponent<ModMenu>();
        DontDestroyOnLoad(modMenu);
        return modMenu.GetComponent<ModMenu>();
    }

    void Start()
    {
        //Initialize Variables
        Instance = this;
        elements = new List<MenuElement>();
        Debug.Log("[ModMenu] ModMenu by JNNJ is initializing!");

        //Create Canvas GameObject
        mainObject = new GameObject();
        mainObject.AddComponent<Canvas>();
        mainObject.layer = 5;
        mainObject.name = "ModMenu Canvas";

        //Create Canvas
        canvas = mainObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.4F;

        CreateEventSystem();

        DontDestroyOnLoad(mainObject);
        selectedElement = null;
        selectedElementIndex = 0;
        totalElements = elements.Count;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    #endregion

    #region EventSystem

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        CreateEventSystem();
    }

    void CreateEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            eventSystem = new GameObject();
            eventSystem.name = "Event System";
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(eventSystem);
        }
    }

    #endregion

    #region Button Positioning
    public void PositionButtons()
    {
        if(isRendering())
        {
            if (selectedElementIndex == 0)
            {
                selectedElement = elements[0];
                selectedElement.GetButton().Select();
            }
            _pos.SetElements(totalElements);

            int _selectedElementIndex = !InFolder() ? selectedElementIndex : currentFolder.selectedElementIndex;
            List<MenuElement> _elements = !InFolder() ? elements : currentFolder.GetElements();
            MenuElement _selectedElement = !InFolder() ? selectedElement : currentFolder.selectedElement;

            List<MenuElement> reversed = new List<MenuElement>(_elements);

            reversed.Reverse();

            foreach (MenuElement element in reversed)
            {
                Rect rect = new Rect(_pos.NextControlRect(false));

                Button button = element.GetButton();

                RectTransform buttonTrans = button.GetComponent<RectTransform>();
                RectTransform textTrans = element.GetTextInstance().GetComponent<RectTransform>();

                if(element.hasSecondText())
                    SetRectTransformValues(pos.GetSecondTextPos(), element.GetSecondTextInstance().GetComponent<RectTransform>());

                SetRectTransformValues(rect, buttonTrans);
                SetRectTransformValues(new Rect(0, 0, rect.width - 10, rect.height), textTrans);

                if(element.HasIcon(MenuIcon.Left)) 
                    SetRectTransformValues(_pos.GetIconPos(MenuIcon.Left), element.leftIcon.GetComponent<RectTransform>());

                if (element.HasIcon(MenuIcon.Right)) 
                    SetRectTransformValues(_pos.GetIconPos(MenuIcon.Right), element.rightIcon.GetComponent<RectTransform>());

                if (element.HasIcon(MenuIcon.Right2)) 
                    SetRectTransformValues(_pos.GetIconPos(MenuIcon.Right2), element.rightIcon2.GetComponent<RectTransform>());
            }

            RectTransform pTrans = panelObj.GetComponent<RectTransform>();
            SetRectTransformValues(_pos.GetBackgroundRect(_elements.Count, _elements[_elements.Count / 2].GetComponent<RectTransform>().localPosition.y), pTrans);
        }
    }

    private void SetRectTransformValues(Rect values, RectTransform rTrans)
    {
        rTrans.sizeDelta = new Vector2(values.width, values.height);

        rTrans.localScale = Vector3.one;
        rTrans.localRotation = new Quaternion(0, 0, 0, 0);

        rTrans.localPosition = new Vector3(values.x, values.y, 0);
    }

    #endregion

    #region Mode and Position switching
    public void ChangeMode(bool draw)
    {
        if (draw)
        {
            selectedElement.GetButton().Select();
            mainObject.SetActive(true);
        }
        else
            mainObject.SetActive(false);
        drawGUI = draw;
    }

    void ChangePos(Vector2 newPos)
    {
        _pos.ChangePos(newPos.x, newPos.y);
    }

    #endregion

    #region Controls
    void Update()
    {
        MenuElement _selectedElement = !InFolder() ? selectedElement : currentFolder.selectedElement;
        if (selectedElement != null && !selectedElement.IsSelected()) selectedElement.GetButton().Select();
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            ChangeMode(!drawGUI);
        }

        if(IsDrawing() && isRendering())
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                SelectPreviousButton();
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                SelectNextButton();
            }

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                _selectedElement.LeftArrow();
            }

            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                _selectedElement.RightArrow();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                _selectedElement.Pressed();
            }
        }
    }

    public void SelectPreviousButton()
    {
        int _selectedElementIndex = !InFolder() ? selectedElementIndex : currentFolder.selectedElementIndex;
        List<MenuElement> _elements = !InFolder() ? elements : currentFolder.GetElements();
        MenuElement _selectedElement = !InFolder() ? selectedElement : currentFolder.selectedElement;

        _selectedElementIndex--;
        if (selectedElementIndex == -1)
        {
            _selectedElementIndex = _elements.Count - 1;
            _selectedElement = _elements[_elements.Count - 1];
        }
        else
            _selectedElement = _elements[_selectedElementIndex];

        _selectedElement.GetButton().Select();
        _selectedElement.Select(false);

        if(InFolder())
        {
            currentFolder.selectedElementIndex = _selectedElementIndex;
            currentFolder.selectedElement = _selectedElement;
        } else
        {
            selectedElementIndex = _selectedElementIndex;
            selectedElement = _selectedElement;
        }
    }

    public void SelectNextButton()
    {
        int _selectedElementIndex = !InFolder() ? selectedElementIndex : currentFolder.selectedElementIndex;
        List<MenuElement> _elements = !InFolder() ? elements : currentFolder.GetElements();
        MenuElement _selectedElement = !InFolder() ? selectedElement : currentFolder.selectedElement;

        _selectedElementIndex++;
        if (_selectedElementIndex == _elements.Count)
        {
            _selectedElementIndex = 0;
            _selectedElement = _elements[0];
        } else
            _selectedElement = _elements[_selectedElementIndex];

        _selectedElement.GetButton().Select();
        _selectedElement.Select(true);

        if (InFolder())
        {
            currentFolder.selectedElementIndex = _selectedElementIndex;
            currentFolder.selectedElement = _selectedElement;
        }
        else
        {
            selectedElementIndex = _selectedElementIndex;
            selectedElement = _selectedElement;
        }
    }
    #endregion
}
