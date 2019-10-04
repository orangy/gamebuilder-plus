using GBWorldGen.Core.Models.Abstractions;
using System;
using System.Collections.Generic;

namespace GBWorldGen.Core.Models
{
    public class Actor : BaseActor<short>
    {
        public string Tag { get; set; }

        public double AdjustX { get; } = 2.5;
        public double AdjustZ { get; } = 2.5;
        public double AdjustY { get; } = 1.5;

        public List<string> ValidTags { get; } = new List<string>
        {
            "WG_Fern_Large",
            "WG_Fern_Small",
            "WG_ForestRock_Large01",
            "WG_ForestRock_Large02",
            "WG_ForestRock_Medium01",
            "WG_ForestRock_Medium02",
            "WG_ForestRock_Medium03",
            "WG_ForestRock_Tiny01",
            "WG_ForestRock_Tiny02",
            "WG_ForestRock_Tiny03",
            "WG_LeavesTwigPile",
            "WG_Mushroom_Large",
            "WG_Mushroom_Small",
            "WG_Stump",
            "WG_Stump_Variant",
            "WG_Tree",
            "WG_Tree_Variant"
        };

        public Actor() { }
        public Actor(short x, short y, short z, string tag = null)
            : base(x, y, z)
        {
            var random = new Random();

            Tag = !string.IsNullOrEmpty(tag) ? tag : ValidTags[random.Next(ValidTags.Count)];
        }

        public override string ToString()
        {
            return $"spawnTaggedActor('{Tag}', {{x: {X * AdjustX}, y: {Y * AdjustY}, z: {Z * AdjustZ}}});";
        }
    }
}
