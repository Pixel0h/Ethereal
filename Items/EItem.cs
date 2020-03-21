using Ethereal.Enums;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ethereal.Items
{
    public class EItem : GlobalItem
    {
        /* Base for updating tooltip damage of items based on stat */
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
        }
    }
}
