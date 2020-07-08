using UnityEngine;

public enum VisabilityLevels
{
	LongRange,
	MiddleRange,
	ShortRange
}

public enum MovementSpeedLevels
{
	Fast = 9,
	Normal = 6,
	Slow = 4
}

public interface IFollowingBehavior
{
	void FollowObject(Transform ojbectPosition,
		VisabilityLevels visabilityLevel,
		MovementSpeedLevels movementSpeedLevel);
}
