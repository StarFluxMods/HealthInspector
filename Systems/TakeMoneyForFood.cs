using Kitchen;
using KitchenData;
using KitchenLib.Preferences;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace HealthInspector.Systems
{
    [UpdateBefore(typeof(DestroyAppliancesAtNight))]
    [UpdateBefore(typeof(CreateEndOfDayPopup))]
    public class TakeMoneyForFood : StartOfNightSystem, IModSystem
    {
        private EntityQuery m_ItemHolderQuery;
        protected override void Initialise()
        {
            m_ItemHolderQuery = GetEntityQuery(typeof(CItemHolder));
            base.Initialise();
        }

        private int foodAmount = 0;
        
        protected override void OnUpdate()
        {
            foodAmount = 0;
            
            NativeArray<Entity> itemHolders = m_ItemHolderQuery.ToEntityArray(Allocator.TempJob);
            
            for (int i = 0; i < itemHolders.Length; i++)
            {
                Entity item = itemHolders[i];
                if (Require(item, out CItemHolder cItemHolder) && !Has<CPreservesContentsOvernight>(item) && !Has<CItemProvider>(item) && cItemHolder.HeldItem != Entity.Null && Require(cItemHolder.HeldItem, out CItem cItem) && GameData.Main.TryGet(cItem.ID, out Item held) && !held.IsIndisposable)
                {
                    foodAmount++;
                }
            }
            
            for (int i = foodAmount; i > 0; i--)
            {
                Entity e = EntityManager.CreateEntity(typeof(CMoneyTrackEvent));
                EntityManager.SetComponentData(e, new CMoneyTrackEvent
                {
                    Identifier = Mod.ItemsDummy,
                    Amount = Mod.manager.GetPreference<PreferenceInt>("costReductionPerItem").Value
                });
            }

            if (HasSingleton<SMoney>())
            {
                SMoney money = GetSingleton<SMoney>();
                money.Amount += Mod.manager.GetPreference<PreferenceInt>("costReductionPerItem").Value * foodAmount;
                if (money.Amount < 0)
                    money.Amount = 0;
                SetSingleton(money);
            }

            itemHolders.Dispose();
        }
    }
}