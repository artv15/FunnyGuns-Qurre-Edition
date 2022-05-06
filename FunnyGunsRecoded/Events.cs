using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

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
            if (Plugin.isEngaged && Plugin.isAmmoInfinite)
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

        public void hurting(Qurre.API.Events.DamageProcessEvent ev)
        {
            if (Plugin.damage3x)
            {
                ev.Amount = ev.Amount * 3;
            }
            if (Classes.Mutator.isEngaged("speed++") && ev.DamageType == Qurre.API.Objects.DamageTypes.Scp207)
            {
                ev.Allowed = false;
                Qurre.Log.Warn("Disabling damage, due to it being cola effect!");
            }
            else if (ev.Amount < 20)
            {
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Concussed, 10);
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Deafened, 10);
            }
            else if (ev.Amount < 50)
            {
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Concussed, 10);
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Deafened, 10);
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Blinded, 4);
            }
            else
            {
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Concussed, 14);
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Deafened, 14);
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.Blinded, 6);
                ev.Target.EnableEffect(Qurre.API.Objects.EffectType.SinkHole, 6);
            }
            
        }

        public void StopAllEventShit()
        {
            Plugin.isEngaged = false;
            Plugin.isAmmoInfinite = true;
            Plugin.engagedMutators.Clear();
            Plugin.damage3x = false;
            Plugin.checkForEndgame = true;
            Timing.KillCoroutines("endgameChecker");
            Timing.KillCoroutines("insDeath");
            Timing.KillCoroutines("gameController");
            Timing.KillCoroutines("playerHUD");
        }

        public void TeamRespawn(Qurre.API.Events.TeamRespawnEvent ev)
        {
            if (Plugin.isEngaged)
            ev.Allowed = false;
        }

        public void WaitingForPlayers()
        {
            StopAllEventShit();
            Plugin.loadedMutators.Clear();

            Plugin.loadedMutators.Add(new Classes.Mutator("passiveRegen", "<color=green>Пассивная регенерация</color>", () =>
            {
                Timing.RunCoroutine(Coroutines.Mutators.passiveRegen_engaged(), "passiveRegen");
            },
            () =>
            {
                Timing.KillCoroutines("passiveRegen");
            },
            (ev) => { },
            () => { }));

            Plugin.loadedMutators.Add(new Classes.Mutator("fogOfWar", "<color=orange>Густой туман</color>", () =>
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

            Plugin.loadedMutators.Add(new Classes.Mutator("lightsOut", "<color=orange>Нет света</color>", () =>
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

            Plugin.loadedMutators.Add(new Classes.Mutator("speed++", "<color=green>Скорость передвижения увеличена!</color>", () =>
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

            Plugin.loadedMutators.Add(new Classes.Mutator("noTeslaGates", "<color=orange>Тесла ворота отключены</color>", () =>
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

            Plugin.loadedMutators.Add(new Classes.Mutator("damage++", "<color=red>Урон увеличен (3x)</color>", () =>
            {
                Plugin.damage3x = true;
            }, () =>
            {
                Plugin.damage3x = false;
            }, (ev) =>
            {

            }, () =>
            {

            }));

            Plugin.loadedMutators.Add(new Classes.Mutator("legalWH", "<color=green>Рентгеновское зрение</color>", () =>
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

            Plugin.loadedMutators.Add(new Classes.Mutator("tutorialAssault", "<color=#07f773>Штурм туториалов [INITING]</color>", () =>
            {
                Timing.RunCoroutine(Coroutines.Mutators.tutorialAssault_engaged(), "TutorialAssault");
            }, () =>
            {
                Timing.KillCoroutines("TutorialAssault");
            }, (ev) =>
            {

            }, () =>
            {

            }));
        }
    }
}
