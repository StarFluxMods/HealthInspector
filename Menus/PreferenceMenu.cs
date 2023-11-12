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
        public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        
        public override void Setup(int player_id)
        {
            
            AddSubmenuButton("Health Inspector", typeof(HealthInspectorMenu<T>), false);
            AddSubmenuButton("Rats", typeof(RatsMenu<T>), false);
            
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