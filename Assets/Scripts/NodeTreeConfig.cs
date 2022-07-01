using UnityEngine;

namespace Cubie.Game
{
	[CreateAssetMenu(fileName = "TreeConfig", menuName = "Cubie/NodeTreeConfig")]
	public class NodeTreeConfig : ScriptableObject
	{
		public int maxLeaves = 2;
		public int minDepth = 2;
		public int maxDepth = 10;
		public int maxNumber = 100;
		public int minTargetNodes = 1;
		public int maxTargetNodes = 5;
	}
}
