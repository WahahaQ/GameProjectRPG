using UnityEngine;

static public class GameUtilities
{
	static public bool CheckAnimatorParameter(Animator animator, string parameter)
	{
		foreach (AnimatorControllerParameter param in animator.parameters)
		{
			if (param.name == parameter)
				return true;
		}

		return false;
	}
}
