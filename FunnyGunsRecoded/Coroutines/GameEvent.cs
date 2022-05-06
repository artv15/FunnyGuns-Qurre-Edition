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

            foreach (var door in Qurre.API.Map.Doors)
            {
                door.Open = false;
                if (door.Name == "SURFACE_GATE" || door.Name == "GATE_A" || door.Name == "GATE_B")
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
                        pl.AddItem(ItemType.GunAK);
                        pl.AddItem(ItemType.KeycardChaosInsurgency);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        CITickets -= 1;
                    }
                    else
                    {
                        pl.Role = RoleType.NtfSergeant;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.GunE11SR);
                        pl.AddItem(ItemType.KeycardNTFCommander);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        MTFTickets -= 1;
                    }
                }
                if (random == 0)
                {
                    if (MTFTickets > 0)
                    {
                        pl.Role = RoleType.NtfSergeant;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.GunE11SR);
                        pl.AddItem(ItemType.KeycardNTFCommander);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        MTFTickets -= 1;
                    }
                    else
                    {
                        pl.Role = RoleType.ChaosRifleman;
                        pl.ClearInventory();
                        pl.AddItem(ItemType.GunAK);
                        pl.AddItem(ItemType.KeycardChaosInsurgency);
                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
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
                if (door.Name == "SURFACE_GATE" || door.Name == "GATE_A" || door.Name == "GATE_B")
                {
                    door.Locked = false;
                    break;
                }
            }
            Timing.RunCoroutine(endgameChecker(), "endgameChecker");
            while (Plugin.Stage != 4)
            {
                if (!Plugin.isStageFrozen)
                Plugin.SecondsBeforeNextStage -= 1;
                if (Plugin.SecondsBeforeNextStage == 0)
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
                                Qurre.Log.Info("Selection failed due to mutator already being engaged.");
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
                    Qurre.API.Controllers.Cassie.Send(".g4 .g4 .g4", false, false, true);
                    Plugin.Stage++;
                    Plugin.SecondsBeforeNextStage = 90;
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
    }
}
