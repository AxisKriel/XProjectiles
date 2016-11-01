using System;
using TShockAPI;
using Terraria;
using Terraria.ID;
using NDesk.Options;

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
			// Defaults
			int type = ProjectileID.JestersArrow;
			float x = e.Player.X;
			float y = e.Player.Y;
			float speedX = 8f * e.Player.TPlayer.direction;
			float speedY = 0f;
			int damage = 1;
			uint count = 1;

			OptionSet o = new OptionSet
			{
				{ "t|type=", v => Int32.TryParse(v, out type) },
				{ "x|posX=", v => Single.TryParse(v, out x) },
				{ "y|posY=", v => Single.TryParse(v, out y) },
				{ "X|speedX=", v => Single.TryParse(v, out speedX) },
				{ "Y|speedY=", v => Single.TryParse(v, out speedY) },
				{ "d|dmg|damage=", v => Int32.TryParse(v, out damage) },
				{ "c|count=", v => UInt32.TryParse(v, out count) }
			};

			// Parse parameters
			o.Parse(e.Parameters);

			if (type < 0 || type >= ProjectileID.Count)
			{
				e.Player.SendErrorMessage("Invalid projectile ID!");
				return;
			}

			if (count < 1 || count > Main.maxProjectiles)
				e.Player.SendErrorMessage("Invalid projectile count!");
			else if (count == 1)
			{
				int proj = Projectile.NewProjectile(x, y, speedX, speedY, type, damage, 0f);
				e.Player.SendInfoMessage($"[{proj}] Spawned {type} @ ({x},{y}).");
			}
			else
			{
				uint total = count;

				// Initial projectile
				Projectile.NewProjectile(x, y, speedX, speedY, type, damage, 0f);

				// Spawn further projectiles above and below the initial point, alternating
				for (int i = 32; count > 0; i = i + 32)
				{
					Projectile.NewProjectile(x, y - i, speedX, speedY, type, damage, 0f);
					count--;
					if (count > 0)
					{
						Projectile.NewProjectile(x, y + i, speedX, speedY, type, damage, 0f);
						count--;
					}
				}
				e.Player.SendInfoMessage($"Spawned {total} projectiles of type {type}.");
			}
		}
	}
}

