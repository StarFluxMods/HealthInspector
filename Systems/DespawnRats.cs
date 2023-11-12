using System;
using HealthInspector.Components;
using Kitchen;
using KitchenLib.Preferences;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace HealthInspector.Systems
{
    public class DespawnRats : DaySystem, IModSystem
    {
        private EntityQuery messes;
        private EntityQuery rats;
        private long nextDespawn = 0;
        protected override void Initialise()
        {
            base.Initialise();
            messes = GetEntityQuery(typeof(CMess));
            rats = GetEntityQuery(typeof(CRat));
        }

        protected override void OnUpdate()
        {
            if (nextDespawn == 0)
                nextDespawn = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (Mod.manager.GetPreference<PreferenceBool>("enableRats").Value == false)
            {
                NativeArray<Entity> ratsArray = this.rats.ToEntityArray(Allocator.TempJob);
                for (int i = 0; i < ratsArray.Length; i++)
                {
                    EntityManager.DestroyEntity(ratsArray[i]);
                }
                ratsArray.Dispose();
                return;
            }

            if (messes.CalculateEntityCount() >= Mod.manager.GetPreference<PreferenceInt>("messAmountToTriggerRats").Value)
                return;
            
            if (nextDespawn > DateTimeOffset.Now.ToUnixTimeSeconds())
                return;
            
            NativeArray<Entity> ratsArray2 = this.rats.ToEntityArray(Allocator.TempJob);
            if (ratsArray2.Length > 0)
            {
                EntityManager.DestroyEntity(ratsArray2[0]);
            }
            nextDespawn = DateTimeOffset.Now.ToUnixTimeSeconds() + (long)Mod.manager.GetPreference<PreferenceInt>("ratDespawnRate").Value;
            ratsArray2.Dispose();
        }
    }
}