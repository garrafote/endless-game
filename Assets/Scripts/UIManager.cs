using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class stores references to key pieces of UI so that they do not need to be looked up multiple times.
/// </summary>
public class UIManager : Singleton<UIManager>
{
	public GameObject ScreenSpaceOverlayPrefab;
	public GameObject AbilityIconPrefab;
	public Canvas UIROOT;
	public List<Ability> Abilities;

	public Canvas pause_Menu;

	public Sprite[] Icons;

	public bool canUse;
	public bool paused;

	public override void Awake()
	{
		base.Awake();

		
	}

	public void Init()
	{
		Abilities = new List<Ability>();

		//Load the UI element

		AbilityIconPrefab = Resources.Load<GameObject>("UI/AbilityIcon");
		ScreenSpaceOverlayPrefab = Resources.Load<GameObject>("UI/SS - Overlay");

		GameObject go = GameObject.Find("SS - Overlay");
		if (go != null)
		{
			UIROOT = go.GetComponent<Canvas>();
		}
		if (UIROOT == null)
		{
			UIROOT = ((GameObject)GameObject.Instantiate(ScreenSpaceOverlayPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Canvas>();
			UIROOT.gameObject.name = ScreenSpaceOverlayPrefab.name;
		}

		//pause_Menu = GameObject.Find("Pause Menu").GetComponent<Canvas>();

		Icons = new Sprite[10];

		Icons = Resources.LoadAll<Sprite>("Abilities");

		//Icons = Resources.LoadAll<Sprite>("Icons/");

		GameObject abilityFolder = UIROOT.transform.FindChild("Abilities").gameObject;

		GameObject newAbilityGO;
		for (int i = 0; i < 10; i++)
		{
			Ability newAbility = new Ability();

			newAbilityGO = (GameObject)GameObject.Instantiate(AbilityIconPrefab, Vector3.zero, Quaternion.identity);

			newAbilityGO.transform.SetParent(abilityFolder.transform);
			newAbilityGO.transform.position = new Vector3(i * 65, 65, 0);
			newAbilityGO.name = "Ability (" + (i + 1) % 10 + ")";

			newAbility.index = i;
			if (i == 0)
			{
				newAbility.charges = 999999;
				newAbility.unlimited = true;
				newAbility.cooldownDuration = Random.Range(8, 15);
			}
			else
			{
				newAbility.cooldownDuration = 10;
				newAbility.charges = Random.Range(0, 5);
				newAbility.unlimited = false;

			}

			newAbility.cooldownLeft = 0;

			newAbility.AbilityContainer = newAbilityGO;
			newAbility.AbilityBackground = newAbilityGO.transform.FindChild("Background").GetComponent<Image>();
			newAbility.AbilityIcon = newAbilityGO.transform.FindChild("AbilityIcon").GetComponent<Image>();

			newAbility.AbilityIcon.sprite = Icons[i];
			newAbility.AbilityBackground.sprite = Icons[11];
			newAbility.CooldownDisplay = newAbilityGO.transform.FindChild("Cooldown").GetComponent<Image>();
			newAbility.CooldownDisplay.sprite = Icons[11];

			newAbility.ChargeDisplay = newAbilityGO.transform.FindChild("Remainder").GetComponent<Text>();
			newAbility.ChargeDisplay.text = newAbility.charges.ToString();

			Abilities.Add(newAbility);
		}

		UnpauseGame();
	}

	bool wasFullScreen;
	void Update()
	{
		for (int i = 0; i < Abilities.Count; i++)
		{
			Abilities[i].Update();
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			


			if(paused)
			{
				UnpauseGame();
			}
			else
			{
				PauseGame();
			}
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			for (int i = 0; i < Abilities.Count; i++)
			{
				Abilities[i].DisableAbility();
			}
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			for (int i = 0; i < Abilities.Count; i++)
			{
				Abilities[i].EnableAbility();
			}
		}

		if (!paused)
		{
#if !UNITY_EDITOR
			Screen.lockCursor = true;
#endif
		}
	}

	public void PauseGame()
	{
		paused = true;
		Time.timeScale = 0f;
		//pause_Menu.gameObject.SetActive(paused);

		//Bring in the elements for the pause menu
		//Unlock the mouse
		Screen.lockCursor = false;
		Screen.showCursor = true;
	}

	public void UnpauseGame()
	{
		paused = false;
		Time.timeScale = 1.0f;
		//pause_Menu.gameObject.SetActive(paused);
		
#if !UNITY_EDITOR
		//Lock the mouse
		Screen.lockCursor = true;
		Screen.showCursor = false;
#endif
	}
}
