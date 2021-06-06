using System;
using System.Collections.Generic;

public static class ItemUtils
{
    public static int ItemIdFromItem(InventoryItem item)
    {
        foreach (KeyValuePair<int, InventoryItem> i in ItemManager.Instance.allItems)
        {
            if (i.Value == item)
            {
                return i.Key;
            }
        }
        return -1;
    }

    public static InventoryItem ItemFromName(string name)
    {
        InventoryItem it = null;
        foreach (KeyValuePair<int, InventoryItem> item in ItemManager.Instance.allItems)
        {
            if (item.Value.name.Equals(name, StringComparison.OrdinalIgnoreCase)) it = item.Value;
        }
        return it;
    }
}