﻿using UnityEngine;

public class PlayerCombatController : CombatController
{
    PlayerInventory inventory;

    InventoryController inventoryController;

    public override void OnInitialize()
    {
        inventory = (controlledCharacter.profile as CombatantProfile).inventory as PlayerInventory;

        InitializeActionPack<CombatActionPack>();
    }

    private void Start()
    {
        inventoryController = controllerPack.GetController<InventoryController>();

        controllerPack.GetController<LocomotionController>()
            .actionPack.AddActionInitiatedListener<StartSprintAction>(action =>
            {
                if (aiming)
                    actionPack.TakeAction<AimFreeAction>();
            });

        controllerPack.GetController<PlayerInventoryController>()
            .inventory.OnQuickSlotUnEquipped += (entry =>
            {
                //if (entry.Value.Item is RangedWeapon)
                actionPack.TakeAction<AimFreeAction>();
            });
    }

    private void Update()
    {
        if (inventoryController.Equipped is Weapon)
        {
            #region Ranged

            if (inventoryController.Equipped is RangedWeapon)
            {
                actionPack.TakeAction<UpdateAimAction>();

                actionPack.TakeAction<ToggleAimAction>();

                #region Slug Change

                RangedWeapon weapon = inventoryController.Equipped as RangedWeapon;

                if (weapon.liveBarrel is PolyBarrel)
                {
                    if (Input.GetButtonDown(GameConstants.Fire3))
                    {
                        actionPack.TakeAction<SlugChangeAction>();
                    }
                }

                #endregion

                #region Reload

                if (Input.GetButtonDown(GameConstants.Reload))
                {
                    actionPack.TakeAction<ReloadAction>();
                }

                #endregion
            }

            #endregion

            #region Attack Mode

            switch ((inventoryController.Equipped as Weapon).AttackMode)
            {
                case AttackMode.CONTINIOUS:

                    if (Input.GetButton(GameConstants.Fire1))
                    {
                        actionPack.TakeAction<AttackAction>();
                    }

                    break;

                case AttackMode.SINGULAR:

                    if (Input.GetButtonDown(GameConstants.Fire1))
                    {
                        actionPack.TakeAction<AttackAction>();
                    }

                    break;
                default:
                    break;
            }

            #endregion
        }



        //Draw Target
        if (inventoryController.Equipped is RangedWeapon)
        {
            actionPack.TakeAction<DrawTargetAction>();
        }
    }
}
