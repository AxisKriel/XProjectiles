using System;
using TShockAPI;
using TerrariaApi.Server;
using Terraria;
using Terraria.ID;

namespace Projectiles
{
	/* All we need is:
		1. Spawn projectiles relative to player's location
		2. Alter projectile velocity/direction
		3. Alter projectile damage
	*/

	[ApiVersion(1, 25)]
	public partial class Plugin : TerrariaPlugin
	{
		public override string Author
		{
			get { return "Enerdy"; }
		}

		public override string Name
		{
			get { return "Projectiles"; }
		}

		public override Version Version
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
		}

		public Plugin(Main game) : base(game)
		{

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ServerApi.Hooks.GameInitialize.Deregister(this, onInitialize);
			}
		}

		public override void Initialize()
		{
			ServerApi.Hooks.GameInitialize.Register(this, onInitialize);
		}

		void onInitialize(EventArgs e)
		{
			RegisterCommands();
		}

		protected int NewProjectile(float x, float y, float speedX, float speedY, int type, int damage, float knockback, int owner = 255, int ai0 = 0, int ai1 = 0)
		{
			// Copied from TerrariaServerAPI with slight modifications

			int num = 1000;
			for (int i = 0; i < 1000; i++)
			{
				if (!Main.projectile[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == 1000)
			{
				return num;
			}
			Projectile projectile = Main.projectile[num];
			projectile.SetDefaults(type);
			projectile.position.X = x - (float)projectile.width * 0.5f;
			projectile.position.Y = y - (float)projectile.height * 0.5f;
			projectile.owner = owner;
			projectile.velocity.X = speedX;
			projectile.velocity.Y = speedY;
			projectile.damage = damage;
			projectile.knockBack = knockback;
			projectile.identity = num;
			projectile.gfxOffY = 0f;
			projectile.stepSpeed = 1f;
			projectile.wet = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
			if (projectile.ignoreWater)
			{
				projectile.wet = false;
			}
			projectile.honeyWet = Collision.honey;
			if (projectile.aiStyle == 1)
			{
				while (projectile.velocity.X >= 16f || projectile.velocity.X <= -16f || projectile.velocity.Y >= 16f || projectile.velocity.Y < -16f)
				{
					projectile.velocity.X *= 0.97f;
					projectile.velocity.Y *= 0.97f;
				}
			}

			if (type == 206)
			{
				projectile.ai[0] = (float)Main.rand.Next(-100, 101) * 0.0005f;
				projectile.ai[1] = (float)Main.rand.Next(-100, 101) * 0.0005f;
			}
			else if (type == 335)
			{
				projectile.ai[1] = (float)Main.rand.Next(4);
			}
			else if (type == 358)
			{
				projectile.ai[1] = (float)Main.rand.Next(10, 31) * 0.1f;
			}
			else if (type == 406)
			{
				projectile.ai[1] = (float)Main.rand.Next(10, 21) * 0.1f;
			}
			else
			{
				projectile.ai[0] = ai0;
				projectile.ai[1] = ai1;
			}

			if (type == 434)
			{
				projectile.ai[0] = projectile.position.X;
				projectile.ai[1] = projectile.position.Y;
			}
			if (type > 0 && type < 662)
			{
				if (ProjectileID.Sets.NeedsUUID[type])
				{
					projectile.projUUID = projectile.identity;
				}
				if (ProjectileID.Sets.StardustDragon[type])
				{
					int num2 = Main.projectile[(int)projectile.ai[0]].projUUID;
					if (num2 >= 0)
					{
						projectile.ai[0] = (float)num2;
					}
				}
			}

			// Always send the projectile data
			NetMessage.SendData(27, -1, -1, "", num, 0f, 0f, 0f, 0, 0, 0);

			if (type == 28)
			{
				projectile.timeLeft = 180;
			}
			if (type == 516)
			{
				projectile.timeLeft = 180;
			}
			if (type == 519)
			{
				projectile.timeLeft = 180;
			}
			if (type == 29)
			{
				projectile.timeLeft = 300;
			}
			if (type == 470)
			{
				projectile.timeLeft = 300;
			}
			if (type == 637)
			{
				projectile.timeLeft = 300;
			}
			if (type == 30)
			{
				projectile.timeLeft = 180;
			}
			if (type == 517)
			{
				projectile.timeLeft = 180;
			}
			if (type == 37)
			{
				projectile.timeLeft = 180;
			}
			if (type == 75)
			{
				projectile.timeLeft = 180;
			}
			if (type == 133)
			{
				projectile.timeLeft = 180;
			}
			if (type == 136)
			{
				projectile.timeLeft = 180;
			}
			if (type == 139)
			{
				projectile.timeLeft = 180;
			}
			if (type == 142)
			{
				projectile.timeLeft = 180;
			}
			if (type == 397)
			{
				projectile.timeLeft = 180;
			}
			if (type == 419)
			{
				projectile.timeLeft = 600;
			}
			if (type == 420)
			{
				projectile.timeLeft = 600;
			}
			if (type == 421)
			{
				projectile.timeLeft = 600;
			}
			if (type == 422)
			{
				projectile.timeLeft = 600;
			}
			if (type == 588)
			{
				projectile.timeLeft = 180;
			}
			if (type == 443)
			{
				projectile.timeLeft = 300;
			}

			if (type == 249)
			{
				projectile.frame = Main.rand.Next(5);
			}

			return num;
		}
	}
}

