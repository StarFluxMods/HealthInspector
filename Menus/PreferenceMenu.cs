using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Preferences;
using UnityEngine;

namespace HealthInspector.Menus
{
    public class PreferenceMenu<T> : KLMenu<T>
    {
        public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        private Option<int> costReductionPerMess = new Option<int>(new List<int> { -1, -2, -3, -4, -5, -6, -7, -8, -9, -10 }, Mod.manager.GetPreference<PreferenceInt>("costReductionPerMess").Value, new List<string> { "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9", "That's Enough" });
        
        public override void Setup(int player_id)
        {
            AddLabel("Reduction Per Mess");
            AddSelect(costReductionPerMess);
            costReductionPerMess.OnChanged += delegate (object _, int result)
            {
                Mod.manager.GetPreference<PreferenceInt>("costReductionPerMess").Set(result);
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