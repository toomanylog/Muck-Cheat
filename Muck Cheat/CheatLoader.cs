using UnityEngine;

namespace GameName_Cheat
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
                CheatLogger.enabled = true;
                //Add Logging Script
                cheatObject.AddComponent<CheatLogger>();
                Object.DontDestroyOnLoad(cheatObject);
                
                initialized = true;
            }
        }

    }
}
