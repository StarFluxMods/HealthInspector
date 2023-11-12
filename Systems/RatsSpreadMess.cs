using System;
using System.Collections.Generic;
using HealthInspector.Components;
using Kitchen;
using Kitchen.Layouts;
using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HealthInspector.Systems
{
    public class RatsSpreadMess : GameSystemBase, IModSystem
    {
        private EntityQuery rats;
        protected override void Initialise()
        {
            base.Initialise();
            rats = GetEntityQuery(typeof(CRat), typeof(CDirtyFeet));
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> dirtyRats = rats.ToEntityArray(Allocator.TempJob);
            
            for (int i = 0; i < dirtyRats.Length; i++)
            {
                if (Require(dirtyRats[i], out CPosition cPosition) && Require(dirtyRats[i], out CDirtyFeet cDirtyFeet))
                {
                    if (GetOccupant(cPosition.Position, OccupancyLayer.Floor) == Entity.Null)
                    {
                        if (Random.Range(1, 3) == 2)
                        {
                            Entity mess = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CMess));
                            EntityManager.SetComponentData(mess, new CCreateAppliance
                            {
                                ID = cDirtyFeet.MessID,
                                ForceLayer = OccupancyLayer.Floor
                            });
                            EntityManager.SetComponentData(mess, new CPosition
                            {
                                Position = cPosition.Position.Rounded()
                            });
                        }
                        EntityManager.RemoveComponent<CDirtyFeet>(dirtyRats[i]);
                    }
                }
            }

            dirtyRats.Dispose();
        }
    }
}