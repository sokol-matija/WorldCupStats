
namespace WFA_WorldCupStats
{
	public class PlayerControlComparer : IEqualityComparer<PlayerControl>
	{
		public bool Equals(PlayerControl x, PlayerControl y)
		{
			return x.Player.Name == y.Player.Name;
		}

		public int GetHashCode(PlayerControl obj)
		{
			return obj.Player.Name.GetHashCode();
		}
	}
}