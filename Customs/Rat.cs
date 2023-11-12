using System.Collections.Generic;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;

namespace HealthInspector.Customs
{
    public class Rat : CustomAppliance
    {
        public override string UniqueNameID => "Rat";
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>
        {
            new CMobileAppliance
            {
                Speed = 3,
                AimForDirt = false
            },
            new CFireImmune(),
            new CDoesNotOccupy()
        };

        public override GameObject Prefab => MaterialUtils.AssignMaterialsByNames(Mod.Bundle.LoadAsset<GameObject>("Rat"));
        public override bool SkipRotationAnimation => true;
        public override OccupancyLayer Layer => OccupancyLayer.Ceiling;
    }
}