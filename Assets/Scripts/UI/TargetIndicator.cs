using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
#pragma warning disable 0649

	[Space]
	[SerializeField]
	private Transform target;

#pragma warning restore 0649

	public float minDistance;

	private void Start()
	{
		SetIndicatorActiveState(false);
	}

	private void Update()
	{
		if (target != null)
		{
			PointAtTarget();
		}
	}

	public void SetActiveTarget(Transform target)
	{
		if (target == null)
		{
			SetIndicatorActiveState(false);
			return;
		}

		this.target = target;   // Set the currently active target
	}

	private void PointAtTarget()
	{
		Vector3 direction = target.position - transform.position;   // Get the target direction

		if (direction.magnitude < minDistance)
		{
			SetIndicatorActiveState(false);
		}
		else
		{
			SetIndicatorActiveState(true);

			// Set transform rotation
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	private void SetIndicatorActiveState(bool state)
	{
		// Set an active state for every transform child
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(state);
		}
	}
}
