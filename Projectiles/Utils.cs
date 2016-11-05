using System;
using Terraria;
using Terraria.ID;

namespace Projectiles
{
	public class Utils
	{
		public static int NewProjectile(XProjectile projectile)
		{
			if (projectile.Owner == null)
				return -1;
			
			return NewProjectile(
				projectile.Position.X,
				projectile.Position.Y,
				projectile.Speed.X,
				projectile.Speed.Y,
				projectile.Type,
				projectile.Damage,
				projectile.KnockBack,
				projectile.Owner.Index,
				projectile.Ai0,
				projectile.Ai1);
		}

		public static int NewProjectile(XProjectile projectile, Vector2 position)
		{
			if (projectile.Owner == null)
				return -1;

			return NewProjectile(
				position.X,
				position.Y,
				projectile.Speed.X,
				projectile.Speed.Y,
				projectile.Type,
				projectile.Damage,
				projectile.KnockBack,
				projectile.Owner.Index,
				projectile.Ai0,
				projectile.Ai1); 
		}

		public static int NewProjectile(float x, float y, float speedX, float speedY, int type, int damage, float knockback, int owner = 255, int ai0 = 0, int ai1 = 0)
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


			if (type == ProjectileID.Leaf)
			{
				projectile.ai[0] = (float)Main.rand.Next(-100, 101) * 0.0005f;
				projectile.ai[1] = (float)Main.rand.Next(-100, 101) * 0.0005f;
			}
			else if (type == ProjectileID.OrnamentFriendly)
			{
				projectile.ai[1] = (float)Main.rand.Next(4);
			}
			else if (type == ProjectileID.WaterGun)
			{
				projectile.ai[1] = (float)Main.rand.Next(10, 31) * 0.1f;
			}
			else if (type == ProjectileID.SlimeGun)
			{
				projectile.ai[1] = (float)Main.rand.Next(10, 21) * 0.1f;
			}
			else
			{
				projectile.ai[0] = ai0;
				projectile.ai[1] = ai1;
			}

			if (type == ProjectileID.ScutlixLaserFriendly)
			{
				projectile.ai[0] = projectile.position.X;
				projectile.ai[1] = projectile.position.Y;
			}
			if (type > 0 && type < ProjectileID.Count)
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

			if (type == ProjectileID.Bomb)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.BouncyBomb)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.BombFish)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.Dynamite)
			{
				projectile.timeLeft = 300;
			}
			if (type == ProjectileID.StickyDynamite)
			{
				projectile.timeLeft = 300;
			}
			if (type == ProjectileID.BouncyDynamite)
			{
				projectile.timeLeft = 300;
			}
			if (type == ProjectileID.Grenade)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.BouncyGrenade)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.StickyBomb)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.HappyBomb)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.GrenadeI)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.GrenadeII)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.GrenadeIII)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.GrenadeIV)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.StickyGrenade)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.FireworkFountainYellow)
			{
				projectile.timeLeft = 600;
			}
			if (type == ProjectileID.FireworkFountainRed)
			{
				projectile.timeLeft = 600;
			}
			if (type == ProjectileID.FireworkFountainBlue)
			{
				projectile.timeLeft = 600;
			}
			if (type == ProjectileID.FireworkFountainRainbow)
			{
				projectile.timeLeft = 600;
			}
			if (type == ProjectileID.PartyGirlGrenade)
			{
				projectile.timeLeft = 180;
			}
			if (type == ProjectileID.Electrosphere)
			{
				projectile.timeLeft = 300;
			}

			if (type == ProjectileID.StyngerShrapnel)
			{
				projectile.frame = Main.rand.Next(5);
			}

			return num;
		}
	}
}

