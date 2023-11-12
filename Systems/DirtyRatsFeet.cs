using HealthInspector.Components;
using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace HealthInspector.Systems
{
    public class DirtyRatsFeet : GameSystemBase, IModSystem
    {
        private EntityQuery rats;

        protected override void Initialise()
        {
            base.Initialise();
            rats = GetEntityQuery(typeof(CRat));
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> ratsArray = rats.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < ratsArray.Length; i++)
            {
                if (!Require(ratsArray[i], out CDirtyFeet cDirtyFeet) && Require(ratsArray[i], out CPosition cPosition))
                {
                    Entity mess = GetOccupant(cPosition.Position, OccupancyLayer.Floor);
                    if (mess != Entity.Null)
                    {
                        if (Require(mess, out CAppliance cAppliance))
                        {
                            int messID = 0;
                            if (cAppliance.ID == ApplianceReferences.MessCustomer1 || cAppliance.ID == ApplianceReferences.MessCustomer2 || cAppliance.ID == ApplianceReferences.MessCustomer3)
                                messID = ApplianceReferences.MessCustomer1;
                            else if (cAppliance.ID == ApplianceReferences.MessKitchen1 || cAppliance.ID == ApplianceReferences.MessKitchen2 || cAppliance.ID == ApplianceReferences.MessKitchen3)
                                messID = ApplianceReferences.MessKitchen1;

                            if (messID != 0)
                            {
                                EntityManager.AddComponent<CDirtyFeet>(ratsArray[i]);
                                EntityManager.SetComponentData(ratsArray[i], new CDirtyFeet
                                {
                                    MessID = messID
                                });
                            }
                        }
                    }
                    /*
                     EntityManager.AddComponent<CDirtyFeet>(ratsArray[i]);
                    EntityManager.SetComponentData(ratsArray[i], new CDirtyFeet
                    {
                        MessID = ApplianceReferences.MessCustomer1
                    });
                    */
                }
            }
            ratsArray.Dispose();
        }
    }
}