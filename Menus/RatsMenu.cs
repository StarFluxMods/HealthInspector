using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Preferences;
using UnityEngine;

namespace HealthInspector.Menus
{
    public class RatsMenu<T> : KLMenu<T>
    {
        public RatsMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        
        private Option<bool> enableRats = new Option<bool>(new List<bool> { true, false }, Mod.manager.GetPreference<PreferenceBool>("enableRats").Value, new List<string> { "Enabled", "Disabled" });
        private Option<int> maxRats = new Option<int>(new List<int> { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 }, Mod.manager.GetPreference<PreferenceInt>("maxRats").Value, new List<string> { "0", "2", "4", "6", "8", "10", "12", "14", "16", "18", "20"});
        private Option<int> ratSpawnRate = new Option<int>(new List<int> { 0, 5, 10, 15, 20 }, Mod.manager.GetPreference<PreferenceInt>("ratSpawnRate").Value, new List<string> { "0", "5", "10", "15", "20"});
        private Option<int> ratDespawnRate = new Option<int>(new List<int> { 0, 5, 10, 15, 20 }, Mod.manager.GetPreference<PreferenceInt>("ratDespawnRate").Value, new List<string> { "0", "5", "10", "15", "20"});
        private Option<int> messAmountToTriggerRats = new Option<int>(new List<int> { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 }, Mod.manager.GetPreference<PreferenceInt>("messAmountToTriggerRats").Value, new List<string> { "0", "2", "4", "6", "8", "10", "12", "14", "16", "18", "20"});

        public override void Setup(int player_id)
        {
            AddLabel("Enable Rats");
            AddSelect(enableRats);
            enableRats.OnChanged += delegate (object _, bool result)
            {
                Mod.manager.GetPreference<PreferenceBool>("enableRats").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Maximum Rats At Once");
            AddSelect(maxRats);
            maxRats.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("maxRats").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Rat Spawn Rate (Seconds)");
            AddSelect(ratSpawnRate);
            ratSpawnRate.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("ratSpawnRate").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Rat Despawn Rate (Seconds)");
            AddSelect(ratDespawnRate);
            ratDespawnRate.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("ratDespawnRate").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Mess Amount To Trigger Rats");
            AddSelect(messAmountToTriggerRats);
            messAmountToTriggerRats.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("messAmountToTriggerRats").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            New<SpacerElement>(true);
            
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
            {
                Mod.manager.Save();
                RequestPreviousMenu();
            }, 0, 1f, 0.2f);
        }
    }
}