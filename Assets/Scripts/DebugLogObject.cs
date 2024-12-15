using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class DebugLogObject : MonoBehaviour
    {
        private static List<string> Log = new List<string>(){"Debug log enabled:"};

        private void Start()
        {
            
        }

        private void Update()
        {
            string res_log = "";
            foreach (var i in Log)
            {
                res_log = res_log + "\n" + i;
            }
            gameObject.GetComponent<TMP_Text>().text = res_log;
        }

        public static void log(string text)
        {
            if (Log.Count > 15) Log.RemoveAt(0);
            Log.Add(text);
        }
    }
}