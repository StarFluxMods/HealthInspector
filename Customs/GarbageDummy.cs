using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;

namespace HealthInspector.Customs
{
    public class GarbageDummy : CustomAppliance
    {
        public override string UniqueNameID => "GarbageDummy";

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (
                Locale.English,
                new ApplianceInfo
                {
                    Name = "Unemptied Bins",
                    Description = "You really shouldn't even see this... What are you doing?"
                }
            )
        };
    }
}