using System;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Noggog;

namespace FreeFastTravel
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "FreeFastTravel.esp")
                .Run(args);
        }

        private static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var cellContext in state.LoadOrder.PriorityOrder.Cell().WinningContextOverrides(state.LinkCache))
            {
                Console.WriteLine($"Processing {cellContext.Record}");
                Console.WriteLine($"Record override was found in {cellContext.ModKey}");

                var mutableCell = cellContext.GetOrAddAsOverride(state.PatchMod);
                mutableCell.Flags = mutableCell.Flags.SetFlag(Cell.Flag.CantTravelFromHere, mutableCell.Flags.HasFlag(Cell.Flag.IsInteriorCell));
            }
        }
    }
}