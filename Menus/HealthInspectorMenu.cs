using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Preferences;
using UnityEngine;

namespace HealthInspector.Menus
{
    public class HealthInspectorMenu<T> : KLMenu<T>
    {
        public HealthInspectorMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        
        private Option<int> costReductionPerMess = new Option<int>(new List<int> { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10 }, Mod.manager.GetPreference<PreferenceInt>("costReductionPerMess").Value, new List<string> { "0", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9", "That's Enough" });
        private Option<bool> messMultiplyBySize = new Option<bool>(new List<bool> { true, false }, Mod.manager.GetPreference<PreferenceBool>("messMultiplyBySize").Value, new List<string> { "Enabled", "Disabled" });
        private Option<int> costReductionPerGarbage = new Option<int>(new List<int> { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10 }, Mod.manager.GetPreference<PreferenceInt>("costReductionPerGarbage").Value, new List<string> { "0", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9", "That's Enough" });
        private Option<int> costReductionPerItem = new Option<int>(new List<int> { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10 }, Mod.manager.GetPreference<PreferenceInt>("costReductionPerItem").Value, new List<string> { "0", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9", "That's Enough" });

        public override void Setup(int player_id)
        {
            AddLabel("Reduction Per Mess");
            AddSelect(costReductionPerMess);
            costReductionPerMess.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("costReductionPerMess").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            AddLabel("Larger Messes Cost More");
            AddSelect(messMultiplyBySize);
            messMultiplyBySize.OnChanged += delegate (object _, bool result)
            {
                Mod.manager.GetPreference<PreferenceBool>("messMultiplyBySize").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Reduction Per Garbage");
            AddSelect(costReductionPerGarbage);
            costReductionPerGarbage.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("costReductionPerGarbage").Set(result);
                Mod.manager.Save();
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Reduction Per Item");
            AddSelect(costReductionPerItem);
            costReductionPerItem.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("costReductionPerItem").Set(result);
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