using System;
using Terraria;
using TerrariaApi.Server;

namespace Projectiles
{
	/* All we need is:
		1. Spawn projectiles relative to player's location
		2. Alter projectile velocity/direction
		3. Alter projectile damage
	*/

	[ApiVersion(2, 1)]
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
	}
}

