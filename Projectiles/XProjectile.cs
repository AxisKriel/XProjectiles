using System;
using Terraria;
using Terraria.ID;
using TShockAPI;
using System.Collections.Generic;
using NDesk.Options;

namespace Projectiles
{
	public class XProjectile
	{
		private float x;
		private float y;

		private float speedX;
		private float speedY;

		private int type;
		private int damage;

		private float knockback;
		private int owner;

		public int Index { get; set; }

		public Vector2 Position { get { return new Vector2(x, y); } }

		public Vector2 Speed { get { return new Vector2(speedX, speedY); } }

		public int Type
		{
			get { return type; }
			set { type = value; }
		}

		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		public float KnockBack
		{
			get { return knockback; }
			set { knockback = value; }
		}

		public TSPlayer Owner { get { return TShock.Players[owner]; } }

		public int Ai0 { get; set; }

		public int Ai1 { get; set; }

		public Projectile TProjectile { get { return Index > -1 ? Main.projectile[Index] : null; } }

		public XProjectile()
		{
			Index = -1;
		}

		public XProjectile(float x, float y, float speedX, float speedY, int type, int damage, float knockback, int owner = 255, int ai0 = 0, int ai1 = 0) : this()
		{
			this.x = x;
			this.y = y;

			this.speedX = speedX;
			this.speedY = speedY;

			this.type = type;
			this.damage = damage;
			this.knockback = knockback;

			this.owner = owner;

			Ai0 = ai0;
			Ai1 = ai1;
		}

		/// <summary>
		/// Modifies a <see cref="XProjectile"/> object by parsing a list of arguments.
		/// </summary>
		/// <param name="args">A list of arguments to parse.</param>
		/// <param name="parsed">Unparsed arguments are outputted here (example: projectile count).</param>
		public XProjectile Parse(IEnumerable<string> args, out List<string> parsed)
		{
			XProjectile proj = new XProjectile();

			OptionSet o = new OptionSet
			{
				{ "t|type=", v => Int32.TryParse(v, out proj.type) },
				{ "x|posX=", v => Single.TryParse(v, out proj.x) },
				{ "y|posY=", v => Single.TryParse(v, out proj.y) },
				{ "X|speedX=", v => Single.TryParse(v, out proj.speedX) },
				{ "Y|speedY=", v => Single.TryParse(v, out proj.speedY) },
				{ "d|dmg|damage=", v => Int32.TryParse(v, out proj.damage) },
				{ "o|owner=", v => Int32.TryParse(v, out proj.owner) },
				{ "k|knockback=", v => Single.TryParse(v, out proj.knockback) }
			};

			parsed = o.Parse(args);
			return proj;
		}

		/// <summary>
		/// Spawns this projectile instance into the world.
		/// </summary>
		/// <returns>>The projectile index.</returns>
		public int Spawn()
		{
			Index = Utils.NewProjectile(this);
			return Index;
		}

		/// <summary>
		/// Kills this projectile instance if it is alive.
		/// </summary>
		public bool Kill()
		{
			if (Index > -1 && TProjectile.active)
			{
				Main.projectile[Index].Kill();
				return true;
			}

			return false;
		}
	}
}

