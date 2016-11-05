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

		private float[] ai = new float[2];

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

		public TSPlayer Owner { get { return owner >= 255 ? TSPlayer.Server : TShock.Players[owner]; } }

		public int Ai0 { get { return (int)ai[0]; } }

		public int Ai1 { get { return (int)ai[1]; } }

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

			this.ai[0] = ai0;
			this.ai[1] = ai1;
		}

		/// <summary>
		/// Modifies a <see cref="XProjectile"/> object by parsing a list of arguments.
		/// </summary>
		/// <param name="args">A list of arguments to parse.</param>
		/// <param name="parsed">Unparsed arguments are outputted here (example: projectile count).</param>
		public List<string> Parse(IEnumerable<string> args)
		{
			return new OptionSet
			{
				{ "t|type=", v => Int32.TryParse(v, out type) },
				{ "x|posX=", v => Single.TryParse(v, out x) },
				{ "y|posY=", v => Single.TryParse(v, out y) },
				{ "X|speedX=", v => Single.TryParse(v, out speedX) },
				{ "Y|speedY=", v => Single.TryParse(v, out speedY) },
				{ "d|dmg|damage=", v => Int32.TryParse(v, out damage) },
				{ "o|owner=", v => Int32.TryParse(v, out owner) },
				{ "k|knockback=", v => Single.TryParse(v, out knockback) },
				{ "a0|ai|ai0=", v => Single.TryParse(v, out ai[0]) },
				{ "a1|ai1=", v => Single.TryParse(v, out ai[1]) }
			}.Parse(args);
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
		/// Spawn this projectile isntance into the world at the specified coordinates.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public int Spawn(float x, float y)
		{
			Index = Utils.NewProjectile(this, new Vector2(x, y));
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

