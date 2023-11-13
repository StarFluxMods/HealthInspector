using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;

namespace HealthInspector.Customs
{
    public class ItemsDummy : CustomAppliance
    {
        public override string UniqueNameID => "ItemsDummy";

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (
                Locale.English,
                new ApplianceInfo
                {
                    Name = "Left Out Items",
                    Description = "You really shouldn't even see this... What are you doing?"
                }
            )
        };
    }
}