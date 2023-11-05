using KitchenLib;
using KitchenMods;
using System.Reflection;
using HealthInspector.Customs;
using HealthInspector.Menus;
using Kitchen;
using KitchenLib.Event;
using KitchenLib.Preferences;
using UnityEngine;

namespace HealthInspector
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.healthinspector";
        public const string MOD_NAME = "Health Inspector";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.4";

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        public static int HealthInspectorDummy = 0;
        public static PreferenceManager manager;
        
        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            manager = new PreferenceManager(MOD_GUID);
            manager.RegisterPreference(new PreferenceInt("costReductionPerMess", -1));
            manager.Load();
            manager.Save();
            
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(PreferenceMenu<PauseMenuAction>), typeof(PauseMenuAction));
            ModsPreferencesMenu<MainMenuAction>.RegisterMenu(MOD_NAME, typeof(PreferenceMenu<MainMenuAction>), typeof(MainMenuAction));
            
            Events.MainMenuView_SetupMenusEvent += (s, args) =>
			{
                args.addMenu.Invoke(args.instance, new object[] { typeof(PreferenceMenu<MainMenuAction>), new PreferenceMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};
			            
			Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
			{
				args.addMenu.Invoke(args.instance, new object[] { typeof(PreferenceMenu<PauseMenuAction>), new PreferenceMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};
            
            HealthInspectorDummy = AddGameDataObject<HealthInspectorDummy>().ID;
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}

