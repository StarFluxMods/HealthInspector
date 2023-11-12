using System;
using HealthInspector.Components;
using Kitchen;
using KitchenLib.Preferences;
using KitchenMods;
using Unity.Entities;
using Random = UnityEngine.Random;

namespace HealthInspector.Systems
{
    public class SpawnRats : DaySystem, IModSystem
    {
        private EntityQuery messes;
        private EntityQuery rats;
        private long nextSpawn = 0;
        protected override void Initialise()
        {
            base.Initialise();
            messes = GetEntityQuery(typeof(CMess));
            rats = GetEntityQuery(typeof(CRat));
        }

        protected override void OnUpdate()
        {
            if (nextSpawn == 0)
                nextSpawn = DateTimeOffset.Now.ToUnixTimeSeconds();
            
            if (Mod.manager.GetPreference<PreferenceBool>("enableRats").Value == false)
                return;
            
            if (messes.CalculateEntityCount() == 0 && Mod.manager.GetPreference<PreferenceInt>("messAmountToTriggerRats").Value != 0)
                return;
            
            if (rats.CalculateEntityCount() >= Mod.manager.GetPreference<PreferenceInt>("maxRats").Value)
                return;

            if (messes.CalculateEntityCount() < Mod.manager.GetPreference<PreferenceInt>("messAmountToTriggerRats").Value)
                return;
            
            if (nextSpawn > DateTimeOffset.Now.ToUnixTimeSeconds())
                return;
            
            CLayoutRoomTile tile = Tiles[Random.Range(0, Tiles.Length)];
            if (GetOccupant(tile.Position) != Entity.Null)
                return;
            Entity entity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CDestroyApplianceAtNight), typeof(CRat));
            EntityManager.SetComponentData(entity, new CCreateAppliance
            {
                ID = Mod.Rat
            });
            EntityManager.SetComponentData(entity, new CPosition
            {
                Position = tile.Position
            });
            nextSpawn = DateTimeOffset.Now.ToUnixTimeSeconds() + (long)Mod.manager.GetPreference<PreferenceInt>("ratSpawnRate").Value;
        }
    }
}