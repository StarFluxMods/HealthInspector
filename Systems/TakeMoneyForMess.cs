using Kitchen;
using KitchenLib.Preferences;
using KitchenLib.References;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace HealthInspector.Systems
{
    [UpdateBefore(typeof(DestroyAppliancesAtNight))]
    [UpdateBefore(typeof(CreateEndOfDayPopup))]
    public class TakeMoneyForMess : StartOfNightSystem, IModSystem
    {
        private EntityQuery m_MessQuery;
        protected override void Initialise()
        {
            m_MessQuery = GetEntityQuery(typeof(CMess));
            base.Initialise();
        }

        private int messAmount = 0;
        
        protected override void OnUpdate()
        {
            messAmount = 0;
            
            NativeArray<Entity> messes = m_MessQuery.ToEntityArray(Allocator.TempJob);
            
            for (int i = 0; i < messes.Length; i++)
            {
                Entity mess = messes[i];
                if (Require(mess, out CAppliance cAppliance))
                {
                    if (cAppliance.ID == ApplianceReferences.MessCustomer1 || cAppliance.ID == ApplianceReferences.MessCustomer2 || cAppliance.ID == ApplianceReferences.MessCustomer3 ||
                        cAppliance.ID == ApplianceReferences.MessKitchen1 || cAppliance.ID == ApplianceReferences.MessKitchen3 || cAppliance.ID == ApplianceReferences.MessKitchen3)
                    {
                        messAmount++;
                    }
                }
            }
            
            for (int i = messAmount; i > 0; i--)
            {
                Entity e = EntityManager.CreateEntity(typeof(CMoneyTrackEvent));
                EntityManager.SetComponentData(e, new CMoneyTrackEvent
                {
                    Identifier = Mod.HealthInspectorDummyAppliance,
                    Amount = Mod.manager.GetPreference<PreferenceInt>("costReductionPerMess").Value
                });
            }

            if (HasSingleton<SMoney>())
            {
                SMoney money = GetSingleton<SMoney>();
                money.Amount += Mod.manager.GetPreference<PreferenceInt>("costReductionPerMess").Value * messAmount;
                if (money.Amount < 0)
                    money.Amount = 0;
                SetSingleton(money);
            }

            messes.Dispose();
        }
    }
}