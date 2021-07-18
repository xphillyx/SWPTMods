﻿using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanMerchantArea
{
    [BepInPlugin("aedenthorn.CleanMerchantArea", "Clean Merchant Area", "0.1.0")]
    public partial class BepInExPlugin : BaseUnityPlugin
    {
        private static BepInExPlugin context;

        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<string> removePrefixes;

        //ConfigEntry<int> nexusID;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }
        private void Awake()
        {

            context = this;
            modEnabled = Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug logs");

            removePrefixes = Config.Bind<string>("Options", "MaidModel", "trunk,chest", "Remove any object with names starting with these, comma-separated.");

            //nexusID = Config.Bind<int>("General", "NexusID", 1, "Nexus mod ID for updates");

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Info.Metadata.GUID);
            Dbgl("Plugin awake");

        }

        private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
        {
            if (!modEnabled.Value)
                return;
            if (arg0.name == "The Palace")
            {
                Dbgl("Altering palace");

                string[] removePrefixList = removePrefixes.Value.Split(',');
                Transform t = GameObject.Find("/Scene/Statics/Bar").transform;
                for(int i = t.childCount - 1; i >= 0; i--)
                {
                    if (removePrefixList.ToList().Exists(s => t.GetChild(i).name.StartsWith(s)))
                        Destroy(t.GetChild(i).gameObject);
                }

            }
        }
    }
}
