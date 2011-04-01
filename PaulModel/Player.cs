﻿namespace PaulBenchmark.PaulModel
{
	public class Player
	{
		private readonly Gun gun;

		public Player(Game game, Gun gun)
		{
			this.gun = gun;
		}

		public void Shoot()
		{
			gun.Shoot();
		}
	}
}