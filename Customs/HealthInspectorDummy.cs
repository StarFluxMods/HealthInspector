using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;

namespace HealthInspector.Customs
{
    public class HealthInspectorDummy : CustomDish
    {
        public override string UniqueNameID => "HealthInspectorDummy";

        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (
                Locale.English,
                new UnlockInfo
                {
                    Name = "Health Inspector"
                }
            )
        };
    }
}