using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;

namespace HealthInspector.Customs
{
    public class HealthInspectorDummyAppliance : CustomAppliance
    {
        public override string UniqueNameID => "HealthInspectorDummyAppliance";

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (
                Locale.English,
                new ApplianceInfo
                {
                    Name = "Health Inspector",
                    Description = "You really shouldn't even see this... What are you doing?"
                }
            )
        };
    }
}