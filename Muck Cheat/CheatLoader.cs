using UnityEngine;
using System.Collections.Generic;

namespace Muck_Cheat
{
    public static class CheatLoader
    {
        public static GameObject cheatObject;
        public static bool initialized;

        public static void main()
        {
            if(!initialized)
            {
                //Create a new GameObject and attach Scripts
                cheatObject = new GameObject();
                cheatObject.AddComponent<Cheat>();

                //Define if logging is enabled
                CheatLogger.loggerEnabled = true;
                //Add Logging Script
                cheatObject.AddComponent<CheatLogger>();
                Object.DontDestroyOnLoad(cheatObject);
                
                initialized = true;
            }
        }

        public static void Eject()
        {
            if (!initialized) return;
            List<GameObject> objs = new List<GameObject>();
            ModMenu menu = ModMenu.Instance;
            foreach(MenuElement obj in menu.GetElements())
            {
                foreach(Transform child in obj.transform)
                {
                    objs.Add(child.gameObject);
                }
                objs.Add(obj.gameObject);
            }

            foreach(GameObject obj in objs)
            {
                Object.Destroy(obj);
            }
        }

    }
}
