using MEC;
using System.Linq;

namespace FunnyGunsRecoded
{
    public class Events
    {
        public void roleChanging(Qurre.API.Events.RoleChangeEvent ev)
        {
            if (Plugin.isEngaged)
            {
                if (ev.Player.Role == RoleType.Spectator && ev.NewRole != RoleType.Spectator) // Wow, such a strange check!
                {
                    foreach (var mut in Plugin.engagedMutators)
                    {
                        mut.respawn.Invoke(ev);
                    }
                }
            }
        }

        public void onReloading(Qurre.API.Events.RechargeWeaponEvent ev)
        {
            if (Plugin.isEngaged)
            {
                ev.Player.Ammo12Gauge = 24;
                ev.Player.Ammo44Cal = 120;
                ev.Player.Ammo762 = 120;
                ev.Player.Ammo9 = 120;
                ev.Player.Ammo556 = 120;
            }
        }

        public void playerDied(Qurre.API.Events.DiesEvent ev)
        {
            if (Plugin.isEngaged)
            {
                Plugin.HowManyDeathSinceLastAssault++;
                ev.Target.Inventory.UserInventory.Items.Clear();
                ev.Target.Ammo12Gauge = 0;
                ev.Target.Ammo44Cal = 0;
                ev.Target.Ammo762 = 0;
                ev.Target.Ammo556 = 0;
                ev.Target.Ammo9 = 0;
                ev.Target.AddItem(ItemType.Medkit);
                ev.Target.AddItem(ItemType.Medkit);
            }
        }

        public void droppingAmmo(Qurre.API.Events.DropAmmoEvent ev)
        {
            if (Plugin.isEngaged)
            {
                ev.Allowed = false;
                ev.Player.Broadcast("<color=red>слыш, так незя.</color>", 1, true);
            }
        }

        public void hurting(Qurre.API.Events.DamageProcessEvent ev)
        {
            if ((Plugin.isEngaged && ev.Allowed && ev.Target.Role != ev.Attacker.Role) || ev.DamageType == Qurre.API.Objects.DamageTypes.Scp207)
            {
                /*if (Plugin.damage3x)
                {
                    ev.Amount = ev.Amount * 3;
                }*/
                if (Classes.Mutator.isEngaged("bleeding"))
                {
                    if ((ev.DamageType == Qurre.API.Objects.DamageTypes.Com15 ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.AK ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Com18 ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.CrossVec ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.E11SR ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.FSP9 ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Logicer ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Revolver ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Shotgun)) // oof
                    {
                        ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Bleeding, 10, false);
                    }
                }
                if (Classes.Mutator.isEngaged("badBullets") && (ev.DamageType == Qurre.API.Objects.DamageTypes.Com15 ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.AK ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Com18 ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.CrossVec ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.E11SR ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.FSP9 ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Logicer ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Revolver ||
                        ev.DamageType == Qurre.API.Objects.DamageTypes.Shotgun)) // oof x2
                {
                    int rnd = UnityEngine.Random.Range(0, 11);
                    if (rnd < 4)
                    {
                        ev.Allowed = false;
                    }
                }
                if (Classes.Mutator.isEngaged("speed++") && ev.DamageType == Qurre.API.Objects.DamageTypes.Scp207)
                {
                    ev.Allowed = false;
                }
                else if (ev.Amount < 20)
                {
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Concussed, 3);
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Deafened, 3);
                }
                else if (ev.Amount < 50)
                {
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Concussed, 5);
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Deafened, 5);
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Blinded, 2);
                }
                else
                {
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Concussed, 7);
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Deafened, 7);
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Blinded, 3);
                    ev.Target.EnableEffect(Qurre.API.Objects.EffectType.SinkHole, 3);
                }
            }
        }

        public void StopAllEventShit()
        {
            foreach (var mut in Plugin.engagedMutators.ToList())
            {
                mut.disengaged.Invoke();
                Plugin.engagedMutators.Remove(mut);
            }
            Plugin.isEngaged = false;
            Plugin.engagedMutators.Clear();
            //Plugin.damage3x = false;
            Plugin.checkForEndgame = true;
            Timing.KillCoroutines("endgameChecker");
            Timing.KillCoroutines("insDeath");
            Timing.KillCoroutines("gameController");
            Timing.KillCoroutines("playerHUD");
            Timing.KillCoroutines("zoneDecont");
            foreach (var lift in Qurre.API.Map.Lifts.ToList())
            {
                if (lift.Type == Qurre.API.Objects.LiftType.ElBRight || lift.Type == Qurre.API.Objects.LiftType.ElBLeft ||
                    lift.Type == Qurre.API.Objects.LiftType.ElALeft || lift.Type == Qurre.API.Objects.LiftType.ElARight)
                {
                    lift.Locked = false;
                }
            }
            foreach (var door in Qurre.API.Map.Doors.ToList())
            {
                if (door.Type == Qurre.API.Objects.DoorType.HCZ_Door)
                {
                    door.Open = false;
                    door.Locked = false;
                }
            }
            foreach (var elev in Qurre.API.Map.Lifts.ToList())
            {
                if (elev.Type == Qurre.API.Objects.LiftType.GateA || elev.Type == Qurre.API.Objects.LiftType.GateB)
                {
                    elev.Locked = false;
                }
            }
        }

        public void TeamRespawn(Qurre.API.Events.TeamRespawnEvent ev)
        {
            // It JuSt WoRkS!
            if (Plugin.isEngaged)
                ev.Allowed = false;
        }

        public void WaitingForPlayers()
        {
            StopAllEventShit();
            Plugin.loadedMutators.Clear();

            Plugin.loadedMutators.Add(new Classes.Mutator("passiveRegen", Plugin.getDisplayByCommandName("passiveRegen"), () =>
            {
                Timing.RunCoroutine(Coroutines.Mutators.passiveRegen_engaged(), "passiveRegen");
            },
            () =>
            {
                Timing.KillCoroutines("passiveRegen");
            },
            (ev) => { },
            () => { }));

            Plugin.loadedMutators.Add(new Classes.Mutator("fogOfWar", Plugin.getDisplayByCommandName("fogOfWar"), () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.EnableEffect(Qurre.API.Objects.EffectType.Amnesia);
                }
            }, () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.DisableEffect(Qurre.API.Objects.EffectType.Amnesia);
                }
            },
            (ev) =>
            {
                Timing.CallDelayed(4f, () => ev.Player.EnableEffect(Qurre.API.Objects.EffectType.Amnesia));
            }, () =>
            {

            }));

            Plugin.loadedMutators.Add(new Classes.Mutator("lightsOut", Plugin.getDisplayByCommandName("lightsOut"), () =>
            {
                Qurre.API.Controllers.Lights.TurnOff(99999f);
            }, () =>
            {
                Qurre.API.Controllers.Lights.TurnOff(0f);
            }, (ev) =>
            {

            }, () =>
            {

            }));

            Plugin.loadedMutators.Add(new Classes.Mutator("speed++", Plugin.getDisplayByCommandName("speed++"), () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.EnableEffect(Qurre.API.Objects.EffectType.Scp207);
                }
            }, () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.DisableEffect(Qurre.API.Objects.EffectType.Scp207);
                }
            }, (ev) =>
            {
                Timing.CallDelayed(5f, () =>
                ev.Player.EnableEffect(Qurre.API.Objects.EffectType.Scp207));
            }, () =>
            {

            }));

            Plugin.loadedMutators.Add(new Classes.Mutator("noTeslaGates", Plugin.getDisplayByCommandName("noTeslaGates"), () =>
            {
                foreach (var tesla in Qurre.API.Map.Teslas)
                {
                    tesla.ImmunityRoles.Add(RoleType.ChaosRifleman);
                    tesla.ImmunityRoles.Add(RoleType.NtfSergeant);
                }
            }, () =>
            {
                foreach (var tesla in Qurre.API.Map.Teslas)
                {
                    tesla.ImmunityRoles.Remove(RoleType.ChaosRifleman);
                    tesla.ImmunityRoles.Remove(RoleType.NtfSergeant);
                }
            }, (ev) =>
            {

            }, () =>
            {

            }));

            /*Plugin.loadedMutators.Add(new Classes.Mutator("damage++", "<color=red>Урон увеличен (3x)</color>", () =>
            {
                Plugin.damage3x = true;
            }, () =>
            {
                Plugin.damage3x = false;
            }, (ev) =>
            {

            }, () =>
            {

            }));*/

            Plugin.loadedMutators.Add(new Classes.Mutator("legalWH", Plugin.getDisplayByCommandName("legalWH"), () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.EnableEffect(Qurre.API.Objects.EffectType.Visuals939);
                }
            }, () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.DisableEffect(Qurre.API.Objects.EffectType.Visuals939);
                }
            }, (ev) =>
            {
                Timing.CallDelayed(5f, () =>
                ev.Player.EnableEffect(Qurre.API.Objects.EffectType.Visuals939));
            }, () =>
            {

            }));

            Plugin.loadedMutators.Add(new Classes.Mutator("bleeding", Plugin.getDisplayByCommandName("bleeding"), () =>
            {
                //we do nothing, mutator exists just to be checked for.
            }, () =>
            {
                //we do nothing...
            }, (ev) =>
            {

            }, () =>
            {

            }));

            Plugin.loadedMutators.Add(new Classes.Mutator("badBullets", Plugin.getDisplayByCommandName("badBullets"), () =>
            {
                //we do nothing, mutator exists just to be checked for.
            }, () =>
            {
                //we do nothing...
            }, (ev) =>
            {

            }, () =>
            {

            }));
        }
    }
}
