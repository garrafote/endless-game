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
	public List<GameObject> Abilities;

	public Canvas pause_Menu;

	public Sprite[] Icons;

	public bool paused;

	public override void Awake()
	{
		base.Awake();

		Abilities = new List<GameObject>();

		//Load the UI element
		
		AbilityIconPrefab = Resources.Load<GameObject>("UI/AbilityIcon");
		ScreenSpaceOverlayPrefab = Resources.Load<GameObject>("UI/SS - Overlay");

		GameObject go = GameObject.Find("SS - Overlay");
		if(go != null)
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
		//Icons = Resources.LoadAll<Sprite>("Icons/");

		GameObject abilityFolder = UIROOT.transform.FindChild("Abilities").gameObject;

		GameObject newAbility;
		for (int i = 0; i < 10; i++)
		{
			newAbility = (GameObject)GameObject.Instantiate(AbilityIconPrefab, Vector3.zero, Quaternion.identity);

			newAbility.transform.SetParent(abilityFolder.transform);
			newAbility.transform.position = new Vector3(i * 64, 64, 0);
			newAbility.name = "Ability (" + (i+1)%10 + ")";
			Abilities.Add(newAbility);
		}
	}

	void Start()
	{
		if (pause_Menu != null)
		{
			pause_Menu.gameObject.SetActive(false);
			UnpauseGame();
		}
	}

	bool wasFullScreen;
	void Update()
	{
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
