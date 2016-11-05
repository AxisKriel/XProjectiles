using System;
using TShockAPI;
using Terraria;
using Terraria.ID;
using NDesk.Options;
using System.Collections.Generic;

namespace Projectiles
{
	public partial class Plugin
	{
		void RegisterCommands()
		{
			Commands.ChatCommands.Add(new Command("projectiles.spawn", doProj, "xproj", "projectiles")
			{
				AllowServer = false,
				HelpText = "Commands for spawning projectiles."
			});
		}

		void doProj(CommandArgs e)
		{
			// Create a projectile with default values
			var proj = new XProjectile(
				           e.Player.X,
				           e.Player.Y,
				           8f * e.Player.TPlayer.direction,
				           0f,
				           ProjectileID.JestersArrow,
				           1,
				           0f,
				           e.Player.Index);

			// Parse parameters
			List<string> parsed = proj.Parse(e.Parameters);

			// Number of projectiles to spawn
			uint count = 1;

			// Attempt to get a custom number of projectiles from the unparsed output
			if (parsed?.Count > 0)
				UInt32.TryParse(parsed[0], out count);

			if (proj.Type < 0 || proj.Type >= ProjectileID.Count)
			{
				e.Player.SendErrorMessage("Invalid projectile ID!");
				return;
			}

			if (count == 0 || count > Main.maxProjectiles)
				e.Player.SendErrorMessage("Invalid projectile count!");
			else if (count == 1)
			{
				proj.Spawn();

				if (!e.Silent)
					e.Player.SendInfoMessage($"[{proj.Index}] Spawned {proj.Type} @ ({proj.Position.X},{proj.Position.Y}).");
			}
			else
			{
				uint total = count;

				// Initial projectile
				proj.Spawn();
				count--;

				// Spawn further projectiles above and below the initial point, alternating
				for (int i = 1; count > 0; i++)
				{
					proj.Spawn(proj.Position.X, proj.Position.Y + (32 * i));
					count--;
					if (count > 0)
					{
						proj.Spawn(proj.Position.X, proj.Position.Y - (32 * i));
						count--;
					}
				}

				if (!e.Silent)
					e.Player.SendInfoMessage($"Spawned {total} projectiles of type {proj.Type}.");
			}
		}
	}
}

