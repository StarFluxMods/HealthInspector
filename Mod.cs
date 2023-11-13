using System.Linq;
using KitchenLib;
using KitchenMods;
using System.Reflection;
using HealthInspector.Customs;
using HealthInspector.Menus;
using Kitchen;
using KitchenLib.Event;
using KitchenLib.Logging.Exceptions;
using KitchenLib.Preferences;
using UnityEngine;

namespace HealthInspector
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.healthinspector";
        public const string MOD_NAME = "Health Inspector";
        public const string MOD_VERSION = "0.1.3";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.4";

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        public static int HealthInspectorDummy = 0;
        public static int HealthInspectorDummyAppliance = 0;
        public static int GarbageDummy = 0;
        public static int ItemsDummy = 0;
        public static int Rat = 0;
        public static PreferenceManager manager;
        public static AssetBundle Bundle;
        
        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            manager = new PreferenceManager(MOD_GUID);
            manager.RegisterPreference(new PreferenceInt("costReductionPerMess", -1));
            manager.RegisterPreference(new PreferenceBool("messMultiplyBySize", true));
            manager.RegisterPreference(new PreferenceInt("costReductionPerGarbage", -1));
            manager.RegisterPreference(new PreferenceInt("costReductionPerItem", -1));
            manager.RegisterPreference(new PreferenceBool("enableRats", true));
            manager.RegisterPreference(new PreferenceInt("maxRats", 10));
            manager.RegisterPreference(new PreferenceInt("ratSpawnRate", 5));
            manager.RegisterPreference(new PreferenceInt("ratDespawnRate", 10));
            manager.RegisterPreference(new PreferenceInt("messAmountToTriggerRats", 8));
            manager.Load();
            manager.Save();
            
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(PreferenceMenu<PauseMenuAction>), typeof(PauseMenuAction));
            ModsPreferencesMenu<MainMenuAction>.RegisterMenu(MOD_NAME, typeof(PreferenceMenu<MainMenuAction>), typeof(MainMenuAction));
            
            Events.MainMenuView_SetupMenusEvent += (s, args) =>
			{
                args.addMenu.Invoke(args.instance, new object[] { typeof(PreferenceMenu<MainMenuAction>), new PreferenceMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
                args.addMenu.Invoke(args.instance, new object[] { typeof(HealthInspectorMenu<MainMenuAction>), new HealthInspectorMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
                args.addMenu.Invoke(args.instance, new object[] { typeof(RatsMenu<MainMenuAction>), new RatsMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};
			            
			Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
			{
				args.addMenu.Invoke(args.instance, new object[] { typeof(PreferenceMenu<PauseMenuAction>), new PreferenceMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(HealthInspectorMenu<PauseMenuAction>), new HealthInspectorMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(RatsMenu<PauseMenuAction>), new RatsMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};
            
            HealthInspectorDummy = AddGameDataObject<HealthInspectorDummy>().ID;
            HealthInspectorDummyAppliance = AddGameDataObject<HealthInspectorDummyAppliance>().ID;
            GarbageDummy = AddGameDataObject<GarbageDummy>().ID;
            ItemsDummy = AddGameDataObject<ItemsDummy>().ID;
            Rat = AddGameDataObject<Rat>().ID;
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

