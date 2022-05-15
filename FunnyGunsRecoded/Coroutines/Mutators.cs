using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace FunnyGunsRecoded.Coroutines
{
    public static class Mutators
    {
        public static IEnumerator<float> passiveRegen_engaged()
        {
            while (true)
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    pl.Heal(1f, false);
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public static IEnumerator<float> tutorialAssault_engaged()
        {
            foreach (var mut in Plugin.engagedMutators)
            {
                if (mut.commandName == "tutorialAssault")
                {
                    mut.displayName = "<color=#07f773>Штурм туториалов (Подготовка)</color>";
                    break;
                }
            }
            yield return Timing.WaitForSeconds(10f);
            foreach (var mut in Plugin.engagedMutators)
            {
                if (mut.commandName == "tutorialAssault")
                {
                    mut.displayName = "<color=#07f773>Штурм туториалов (Антисипация)</color>";
                }
            }
            yield return Timing.WaitForSeconds(29f);
            Qurre.API.Controllers.Cassie.Send("Danger . all .g4 .g1 .g1 .g1 personnel . Facility .g3 .g1 .g4 has been . . . Unknown . . error . error . error .g4 .g4 .g4 .g4 .g4 .g1");
            foreach (var mut in Plugin.engagedMutators)
            {
                if (mut.commandName == "tutorialAssault")
                {
                    mut.displayName = "<color=#07f773>Штурм туториалов (</color><color=red>В прогрессе</color><color=#07f773>)</color>";
                }
            }
            while (Plugin.SecondsBeforeNextStage >= 20)
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    if (pl.Role == RoleType.Spectator)
                    {
                        pl.Role = RoleType.Tutorial;
                        pl.AddItem(ItemType.Flashlight);
                        pl.AddItem(ItemType.Adrenaline);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.GunFSP9);
                        pl.AddItem(ItemType.KeycardNTFLieutenant);
                        pl.EnableEffect(Qurre.API.Objects.EffectType.Flashed, 3f);
                        Timing.CallDelayed(3f, () =>
                        {
                            pl.GodMode = true;
                            pl.Ahp = 80;
                            pl.Position = new UnityEngine.Vector3(86.7f, 988.5f, -68.2f);
                            Timing.CallDelayed(5f, () => pl.GodMode = false);
                        });
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            foreach (var mut in Plugin.engagedMutators)
            {
                if (mut.commandName == "tutorialAssault")
                {
                    mut.displayName = "<color=#07f773>Штурм туториалов (Стихание)</color>";
                }
            }
            while (Plugin.SecondsBeforeNextStage > 1)
            {
                yield return Timing.WaitForSeconds(1f);
            }
            foreach (var mut in Plugin.engagedMutators)
            {
                if (mut.commandName == "tutorialAssault")
                {
                    mut.disengaged.Invoke();
                    mut.displayName = "<color=#07f773>Штурм туториалов (Подготовка)</color>"; // For somewhat reason, if we change the mutator in engaged mutators (the reason is pointers), it is changed in loaded mutators. Maybe because we just point to the same place in memory. Maybe not.
                    Plugin.engagedMutators.Remove(mut);
                    yield break;
                }
            }
        }
    }
}
