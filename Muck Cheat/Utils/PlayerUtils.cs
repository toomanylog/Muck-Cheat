using System.Collections.Generic;

public class PlayerUtils
{

    public static PlayerManager GetLocalPlayer()
    {
        foreach(KeyValuePair<int, PlayerManager> entry in GameManager.players)
        {
            if (entry.Value.id == LocalClient.instance.myId) return entry.Value;
        }
        return null;
    }


    public static Client GetLocalPlayerClient()
    {
        foreach (KeyValuePair<int, Client> entry in Server.clients)
        {
            if (entry.Value.id == LocalClient.instance.myId)
                return entry.Value;
        }
        return null;
    }
}
