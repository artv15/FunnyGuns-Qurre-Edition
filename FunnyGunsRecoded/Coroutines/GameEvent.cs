using MEC;

namespace FunnyGunsRecoded.Coroutines
{
    public static class GameEvent
    {
        public static IEnumerator<float> gameController()
        {
            Plugin.Stage = 0;
            Plugin.SecondsBeforeNextStage = 45;

            int MTFTickets = 0;
            int CITickets = 0;

            Timing.RunCoroutine(zoneDecontCoroutine(), "zoneDecont"); //ZONE DECONT (ZONE DECONT (DECONTAMINATION OF ZONES))

            foreach (var item in Qurre.API.Map.Pickups)
            {
                if (item.Type == ItemType.GrenadeHE)
                {
                    item.Destroy();
                }
            }

            foreach (var door in Qurre.API.Map.Doors)
            {
                door.Open = false;
                if (door.Name == "SURFACE_GATE" || door.Type == Qurre.API.Objects.DoorType.Gate_A || door.Type == Qurre.API.Objects.DoorType.Gate_B)
                {
                    door.Locked = true;
                    break;
                }
            }

            if (Qurre.API.Player.List.Count() % 2 == 0)
            {
                MTFTickets = Qurre.API.Player.List.Count() / 2;
                CITickets = MTFTickets;
            }
            else
            {
                MTFTickets = Qurre.API.Player.List.Count() / 2 + 1;
                CITickets = Qurre.API.Player.List.Count() / 2;
            }

            foreach (var pl in Qurre.API.Player.List)
            {
                int random = UnityEngine.Random.Range(0, 2);
                if (random == 1)
                {
                    if (CITickets > 0)
                    {
                        pl.Role = RoleType.ChaosRifleman;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.KeycardChaosInsurgency);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.GunAK);
                        CITickets -= 1;
                    }
                    else
                    {
                        pl.Role = RoleType.NtfSergeant;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.KeycardNTFLieutenant);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.GunE11SR);
                        MTFTickets -= 1;
                    }
                }
                if (random == 0)
                {
                    if (MTFTickets > 0)
                    {
                        pl.Role = RoleType.NtfSergeant;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.KeycardNTFLieutenant);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.GunE11SR);
                        MTFTickets -= 1;
                    }
                    else
                    {
                        pl.Role = RoleType.ChaosRifleman;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.KeycardChaosInsurgency);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.GunAK);
                        CITickets -= 1;
                    }
                }
            }



            while (Plugin.Stage == 0)
            {
                Plugin.SecondsBeforeNextStage -= 1;
                if (Plugin.SecondsBeforeNextStage <= 0)
                {
                    Plugin.Stage++;
                    Plugin.SecondsBeforeNextStage = 90;
                    Qurre.API.Controllers.Cassie.Send(".g4 .g4 .g4", false, false, true);
                }
                yield return Timing.WaitForSeconds(1f);
            }
            foreach (var door in Qurre.API.Map.Doors)
            {
                if (door.Name == "SURFACE_GATE" || door.Type == Qurre.API.Objects.DoorType.Gate_A || door.Type == Qurre.API.Objects.DoorType.Gate_B)
                {
                    door.Locked = false;
                    break;
                }
            }
            Timing.RunCoroutine(endgameChecker(), "endgameChecker");
            while (Plugin.Stage != 5)
            {
                if (!Plugin.isStageFrozen)
                    Plugin.SecondsBeforeNextStage -= 1;
                if (Plugin.SecondsBeforeNextStage == 0)
                {
                    MutatorAssignment();

                    Qurre.API.Controllers.Cassie.Send(".g4 .g4 .g4", false, false, true);
                    Plugin.Stage++;
                    Plugin.SecondsBeforeNextStage = 90; // Maybe change that to 60?
                    // LCZ lockdown
                    if (Plugin.Stage == 3)
                    {
                        foreach (var lift in Qurre.API.Map.Lifts)
                        {
                            if (lift.Type == Qurre.API.Objects.LiftType.ElBRight || lift.Type == Qurre.API.Objects.LiftType.ElBLeft ||
                                lift.Type == Qurre.API.Objects.LiftType.ElALeft || lift.Type == Qurre.API.Objects.LiftType.ElARight)
                            {
                                lift.Locked = true;
                                lift.Status = Lift.Status.Up;
                            }
                        }
                    }
                    // HCZ lockdown
                    if (Plugin.Stage == 4)
                    {
                        foreach (var door in Qurre.API.Map.Doors)
                        {
                            if (door.Type == Qurre.API.Objects.DoorType.HCZ_Door)
                            {
                                door.Open = false;
                                door.Locked = true;
                            }
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            Qurre.API.Controllers.Cassie.Send("PITCH_0.6 .g4 .g4 .g4", false, false, true);
            Timing.RunCoroutine(instantDeath(), "insDeath");
            foreach (var mut in Plugin.engagedMutators)
            {
                mut.disengaged.Invoke();
            }
            Plugin.engagedMutators.Clear();
        }

        public static IEnumerator<float> instantDeath()
        {
            while (Plugin.isEngaged)
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.Damage(1, PlayerStatsSystem.DeathTranslations.Unknown);
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public static IEnumerator<float> endgameChecker()
        {
            while (true)
            {
                int mtf = 0, ci = 0;
                foreach (var pl in Qurre.API.Player.List)
                {
                    if (pl.Role == RoleType.NtfSergeant)
                    {
                        mtf++;
                    }
                    else if (pl.Role == RoleType.ChaosRifleman)
                    {
                        ci++;
                    }
                }
                if (Plugin.checkForEndgame)
                {
                    if (mtf == 0 || ci == 0)
                    {
                        foreach (var mut in Plugin.engagedMutators)
                        {
                            mut.disengaged.Invoke();
                            Plugin.engagedMutators.Remove(mut);
                        }

                        foreach (var pl in Qurre.API.Player.List)
                        {
                            pl.Broadcast($"<color=green>Ивент окончен.</color> Победа за {(ci == 0 ? "<color=blue>MTF</color>" : "<color=green>Хаосом</color>")}.", 10, true);
                        }
                        Qurre.API.Controllers.Cassie.Send(".g4 .g4 .g4", false, false, true);
                        var ev = new Events();
                        ev.StopAllEventShit();
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }

        public static IEnumerator<float> zoneDecontCoroutine()
        {
            while (true)
            {
                if (Plugin.Stage >= 3)
                {
                    foreach (var pl in Qurre.API.Player.List)
                    {
                        if (pl.Zone == Qurre.API.Objects.ZoneType.Light)
                        {
                            pl.Damage(10f, PlayerStatsSystem.DeathTranslations.Unknown);
                        }
                    }
                }
                if (Plugin.Stage >= 4)
                {
                    foreach (var pl in Qurre.API.Player.List)
                    {
                        if (pl.Zone == Qurre.API.Objects.ZoneType.Heavy)
                        {
                            pl.Damage(10f, PlayerStatsSystem.DeathTranslations.Unknown);
                        }
                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public static void MutatorAssignment()
        {
            int spectators = 0;
            int plrs = Qurre.API.Player.List.Count();
            int plrsAlive = plrs - spectators;
            foreach (var pl in Qurre.API.Player.List)
            {
                if (pl.Role == RoleType.Spectator)
                {
                    spectators++;
                }
            }
            if (!(plrsAlive <= spectators) && !(UnityEngine.Random.Range(0, 11) <= 4) && Plugin.AssaultWasStaredHowManyTimes < 1)
            {
                while (true)
                {
                    try
                    {
                        int randsel = UnityEngine.Random.Range(0, Plugin.loadedMutators.Count);
                        Qurre.Log.Info("Called for mutator selection. Index: " + randsel.ToString());
                        if (!Classes.Mutator.isEngaged(Plugin.loadedMutators.ElementAt(randsel).commandName))
                        {
                            Qurre.Log.Info("Mutator is not already engaged, assigning it, invoking all stageChange methods.");
                            foreach (var mut in Plugin.engagedMutators)
                            {
                                mut.stageChange.Invoke();
                            }
                            Plugin.engagedMutators.Add(Plugin.loadedMutators[randsel]);
                            Plugin.loadedMutators[randsel].engaged.Invoke();
                            Qurre.Log.Info("Engaged mutator, breaking out of loop.");
                            break;
                        }
                        else
                        {
                            Qurre.Log.Info("Selection failed due to mutator already being engaged, retrying.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Qurre.Log.Error($"An error occured during mutator assignment! Error: {ex.Message}");
                        var errorMutator = new Classes.Mutator("error", "<color=red>[ERROR] Error.</color>", () => { }, () => { }, (ev) => { }, () => { });
                        Plugin.engagedMutators.Add(errorMutator);
                        break;
                    }
                }
            }
            else
            {
                Plugin.AssaultWasStaredHowManyTimes++; //Hotfix, but it works, I guess...
                Qurre.Log.Info("Called for mutator selection. Bypassed normal assignment, due to spectactors. Assigning tutorialAssault mutator.");
                foreach (var mut in Plugin.loadedMutators)
                {
                    if (mut.commandName == "tutorialAssault")
                    {
                        mut.engaged.Invoke();
                        Plugin.engagedMutators.Add(mut);
                    }
                }
            }
        }
    }
}
