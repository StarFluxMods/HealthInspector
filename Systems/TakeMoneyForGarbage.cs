using Kitchen;
using KitchenLib.Preferences;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace HealthInspector.Systems
{
    [UpdateBefore(typeof(DestroyAppliancesAtNight))]
    [UpdateBefore(typeof(CreateEndOfDayPopup))]
    public class TakeMoneyForGarbage : StartOfNightSystem, IModSystem
    {
        private EntityQuery m_BinQuery;
        protected override void Initialise()
        {
            m_BinQuery = GetEntityQuery(typeof(CApplianceBin));
            base.Initialise();
        }

        private int garbageAmount = 0;
        
        protected override void OnUpdate()
        {
            garbageAmount = 0;
            
            NativeArray<Entity> bins = m_BinQuery.ToEntityArray(Allocator.TempJob);
            
            for (int i = 0; i < bins.Length; i++)
            {
                Entity bin = bins[i];
                if (Require(bin, out CApplianceBin cAppliance) && !Has<CApplianceExternalBin>(bin))
                {
                    garbageAmount += cAppliance.CurrentAmount;
                }
            }
            
            for (int i = garbageAmount; i > 0; i--)
            {
                Entity e = EntityManager.CreateEntity(typeof(CMoneyTrackEvent));
                EntityManager.SetComponentData(e, new CMoneyTrackEvent
                {
                    Identifier = Mod.GarbageDummy,
                    Amount = Mod.manager.GetPreference<PreferenceInt>("costReductionPerGarbage").Value
                });
            }

            if (HasSingleton<SMoney>())
            {
                SMoney money = GetSingleton<SMoney>();
                money.Amount += Mod.manager.GetPreference<PreferenceInt>("costReductionPerGarbage").Value * garbageAmount;
                if (money.Amount < 0)
                    money.Amount = 0;
                SetSingleton(money);
            }

            bins.Dispose();
        }
    }
}