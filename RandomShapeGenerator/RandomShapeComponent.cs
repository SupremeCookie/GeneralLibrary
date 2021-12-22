using UnityEngine;

namespace RandomShapeGenerator
{
	public class RandomShapeComponent : MonoBehaviour
	{
		[Readonly] [SerializeField] private RandomShape _shape;

		public void Injectdata(RandomShape shape)
		{
			_shape = shape;
		}

		private void OnDrawGizmos()
		{
			if (_shape != null)
			{
				if (_shape.Positions.IsNullOrEmpty())
				{
					return;
				}

				Vector2 transPos = transform.position;

				for (int i = 0; i < _shape.Positions.Count; ++i)
				{
					var currentIndex = i;
					var nextIndex = (i + 1) % _shape.Positions.Count;
					var pointData = _shape.Positions[currentIndex];
					var nextPoint = _shape.Positions[nextIndex];

					Gizmos.color = Color.white;
					Gizmos.DrawRay(_shape.Center + pointData + transPos, nextPoint - pointData);

					Gizmos.color = Color.green;
					Gizmos.DrawSphere(_shape.Center + pointData + transPos, 0.1f);
				}
			}
		}
	}
}
