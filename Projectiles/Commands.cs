using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using Terraria;
using Terraria.ID;
using TShockAPI;

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

			Commands.ChatCommands.Add(new Command("items.tweak", doAlter, "tweak", "alter")
			{
				AllowServer = false,
				HelpText = "Commands for tweaking item properties."
			});
		}

		void doAlter(CommandArgs e)
		{
			if (e.Parameters.Count < 1)
			{
				e.Player.SendInfoMessage($"Syntax: {Commands.Specifier}tweak <item> [flags]");
				return;
			}

			// if set, will alter currently existing item instead of spawning a new one
			short index = -1;

			string colorHex = "", damage = "", knockback = "", useAnimation = "", useTime = "", shoot = "",
				shootSpeed = "", width = "", height = "", scale = "", ammo = "", useAmmo = "", notAmmo = "";

			OptionSet o = new OptionSet
			{
				{ "i|index=", v => Int16.TryParse(v, out index) },
				{ "c|color=", v => colorHex = v },
				{ "d|dmg|damage=", v => damage = v },
				{ "k|knock|knockback=", v => knockback = v },
				{ "u|uA|useAnimation=", v => useAnimation = v },
				{ "t|uT|useTime=", v => useTime = v },
				{ "s|shoot=", v => shoot = v },
				{ "ss|sspeed|shootSpeed=", v => shootSpeed = v },
				{ "w|width=", v => width = v },
				{ "h|height=", v => height = v },
				{ "sc|scale=", v => scale = v },
				{ "a|ammo=", v => ammo = v },
				{ "ua|useAmmo=", v => useAmmo = v },
				{ "n|na|notAmmo:", v => notAmmo = String.IsNullOrWhiteSpace(v) ? "true" : v }
			};

			List<string> parsed = o.Parse(e.Parameters);

			if (parsed.Count > 0 && index == -1)
			{
				List<Item> items = TShock.Utils.GetItemByIdOrName(String.Join(" ", parsed));
				if (items?.Count > 1)
				{
					TShock.Utils.SendMultipleMatchError(e.Player, items.Select(i => i.name));
					return;
				}
				else if (items == null || items.Count < 0)
				{
					e.Player.SendErrorMessage("Invalid item!");
					return;
				}
					
				index = (short)Item.NewItem(
					(int)e.Player.X,
					(int)e.Player.Y,
					items[0].width,
					items[0].height,
					items[0].type,
					items[0].stack);
			}

			Item target = Main.item[index];

			byte flags = 0;

			if (!String.IsNullOrWhiteSpace(colorHex))
			{
				var color = Utils.ColorFromRGB(colorHex);
				if (color.HasValue)
				{
					target.color = color.Value;
					flags |= 1;
				}
			}

			if (!String.IsNullOrWhiteSpace(damage))
			{
				int pDamage;
				if (Int32.TryParse(damage, out pDamage))
				{
					target.damage = pDamage;
					flags |= 2;
				}
			}

			if (!String.IsNullOrWhiteSpace(knockback))
			{
				float pKnockback;
				if (Single.TryParse(knockback, out pKnockback))
				{
					target.knockBack = pKnockback;
					flags |= 4;
				}
			}

			if (!String.IsNullOrWhiteSpace(useAnimation))
			{
				int uA;
				if (Int32.TryParse(useAnimation, out uA))
				{
					target.useAnimation = uA;
					flags |= 8;
				}
			}

			if (!String.IsNullOrWhiteSpace(useTime))
			{
				int uT;
				if (Int32.TryParse(useTime, out uT))
				{
					target.useTime = uT;
					flags |= 16;
				}
			}

			if (!String.IsNullOrWhiteSpace(shoot))
			{
				int pShoot;
				if (Int32.TryParse(shoot, out pShoot))
				{
					target.shoot = pShoot;
					flags |= 32;
				}
			}

			if (!String.IsNullOrWhiteSpace(shootSpeed))
			{
				float pShootSpeed;
				if (Single.TryParse(shootSpeed, out pShootSpeed))
				{
					target.shootSpeed = pShootSpeed;
					flags |= 64;
				}
			}

			byte flags2 = 0;

			if (!String.IsNullOrWhiteSpace(width))
			{
				int pWidth;
				if (Int32.TryParse(width, out pWidth))
				{
					target.width = pWidth;
					flags2 |= 1;
				}
			}

			if (!String.IsNullOrWhiteSpace(height))
			{
				int pHeight;
				if (Int32.TryParse(height, out pHeight))
				{
					target.height = pHeight;
					flags2 |= 2;
				}
			}

			if (!String.IsNullOrWhiteSpace(scale))
			{
				float pScale;
				if (Single.TryParse(scale, out pScale))
				{
					target.scale = pScale;
					flags2 |= 4;
				}
			}

			if (!String.IsNullOrWhiteSpace(ammo))
			{
				int pAmmo;
				if (Int32.TryParse(ammo, out pAmmo))
				{
					target.ammo = pAmmo;
					flags2 |= 8;
				}
			}

			if (!String.IsNullOrWhiteSpace(useAmmo))
			{
				int uAmmo;
				if (Int32.TryParse(useAmmo, out uAmmo))
				{
					target.useAmmo = uAmmo;
					flags2 |= 16;
				}
			}

			if (!String.IsNullOrWhiteSpace(notAmmo))
			{
				bool nAmmo;
				if (Boolean.TryParse(notAmmo, out nAmmo))
				{
					target.notAmmo = nAmmo;
					flags2 |= 32;
				}
			}

			TSPlayer.All.SendData(PacketTypes.TweakItem, "", index, flags, flags2);
			if (!e.Silent)
				e.Player.SendSuccessMessage("Tweaked an item according to your input. ({0})", e.Message);
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

