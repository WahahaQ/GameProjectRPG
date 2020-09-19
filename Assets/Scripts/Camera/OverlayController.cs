using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
	public List<MenuEntity> overlays = new List<MenuEntity>();

	private void Start()
	{
		foreach (MenuEntity overlay in overlays)
		{
			overlay.canvasGameObject.SetActive(overlay.isActive);
		}
	}

	public bool IsActive()
	{
		foreach (MenuEntity overlay in overlays)
		{
			if (overlay.isActive == true)
			{
				return true;
			}
		}

		return false;
	}

	public void DisableOverlays()
	{
		foreach (MenuEntity overlay in overlays)
		{
			if (overlay.isActive)
			{
				SwitchMenuState(overlay.menuName);
			}
		}
	}

	public MenuEntity GetMenuByName(string menuName)
	{
		return overlays.Where(x => x.menuName == menuName).FirstOrDefault();
	}

	public void SwitchMenuState(string menuName)
	{
		if (overlays != null && overlays.Count > 0)
		{
			MenuEntity menuEntity = GetMenuByName(menuName);

			if (menuEntity != null)
			{
				menuEntity.isActive = !menuEntity.isActive;
				menuEntity.canvasGameObject.SetActive(menuEntity.isActive);
				Time.timeScale = menuEntity.isActive ? 0f : 1f;
			}
		}
	}
}
