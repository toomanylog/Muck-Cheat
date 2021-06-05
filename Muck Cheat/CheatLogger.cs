using UnityEngine;
using System.Text;
using System.IO;
using System;

namespace GameName_Cheat
{
    public class CheatLogger : MonoBehaviour {

        public static CheatLogger instance;
        public static bool enabled;

        public StringBuilder builder;

        public void Start() {
            if(enabled) {
		builder = new StringBuilder();
                Application.logMessageReceived += new Application.LogCallback(log);
                if(!File.Exists("latest.log")) {
                    File.CreateText("latest.log");
                }
            }
        }

        public void log(string logString, string stackTrace, LogType type) {
            string val = "[" + Enum.GetName(typeof(LogType), type) + "] " + logString;

            if(!String.IsNullOrEmpty(stackTrace)) {
                val = val + " | StackTrace: " + stackTrace;
            }

            builder.AppendLine(val);
        }

        void OnApplicationQuit() {
            if(!File.Exists("latest.log")) {
                File.CreateText("latest.log");
            }

            File.WriteAllText("latest.log", builder.ToString());
        }

    }
}
