using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cheat : MonoBehaviour
{
    private ModMenu menu;

    #region ButtonCheck
    private bool godmode;
    private bool noHunger;
    #endregion

    #region Coroutines

    private Coroutine godModeCoroutine;
    private Coroutine noHungerCoroutine;

    #endregion

    #region Elements
    private MenuStringSelection spawner;
    #endregion

    #region Initialization
    public void Init()
    {
        if (menu == null) menu = ModMenu.Instance;
        menu.StartRendering(670, 300, 500, 1, 100, 500, 5, 20);
        menu.SetTextAllign(TextAnchor.MiddleLeft);

        MenuButton noHunger = menu.CreateButton("No Hunger");
        noHunger.Pressed = NoHunger;

        MenuButton godmode = menu.CreateButton("GodMode");
        godmode.Pressed = GodMode;

        spawner = menu.CreateStringSelection("Spawner");
        StartCoroutine(SetSpawnerStrings());
        spawner.Pressed = SpawnerPressed;

        menu.RegisterElement(spawner);
        menu.RegisterElement(godmode);
        menu.RegisterElement(noHunger);

        menu.ChangeMode(true);
        StartCoroutine(NoHungerCoroutine());
    }

    IEnumerator SetSpawnerStrings()
    {
        yield return new WaitForSeconds(5F);
        List<string> strings = new List<string>();

        foreach (KeyValuePair<int, InventoryItem> item in ItemManager.Instance.allItems)
        {
            strings.Add(item.Value.name);
            Debug.Log(item.Value.name);
        }

        spawner.SetStrings(strings.ToArray());
        Debug.Log(spawner.GetStrings().ToArray());
    }

    public void Start()
    {
        menu = ModMenu.CreateModMenuObject();
        Invoke(nameof(Init), 1);
    }

    #endregion

    #region ButtonListeners

    #region GodMode
    void GodMode()
    {
        godmode = !godmode;
        if(godmode)
        {
            godModeCoroutine = StartCoroutine(GodModeCoroutine());
        } else
            StopCoroutine(godModeCoroutine);
    }

    IEnumerator GodModeCoroutine()
    {
        while(true)
        {
            PlayerStatus stat = PlayerStatus.Instance;
            stat.hp = stat.maxHp;
            stat.stamina = stat.maxStamina;
            ClientSend.PlayerHp(stat.maxHp, stat.maxHp);
            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    void SpawnerPressed()
    {
        InventoryItem item = ItemUtils.ItemFromName(spawner.GetSelected());
        ItemManager.Instance.DropItem(LocalClient.instance.myId, ItemUtils.ItemIdFromItem(item), 1, ItemManager.Instance.GetNextId());
        ServerSend.DropItem(LocalClient.instance.myId, ItemUtils.ItemIdFromItem(item), 1, ItemManager.Instance.GetNextId());
    }

    #region No Hunger

    void NoHunger()
    {
        noHunger = !noHunger;
        if (noHunger)
        {
            noHungerCoroutine = StartCoroutine(NoHungerCoroutine());
        }
        else
            StopCoroutine(noHungerCoroutine);
    }

    IEnumerator NoHungerCoroutine()
    {
        while(true)
        {
            PlayerStatus stat = PlayerStatus.Instance;
            stat.hunger = stat.maxHunger;
            yield return new WaitForSeconds(1);
        }
    }

    #endregion

    #endregion

}