using UnityEngine;
using System.Collections;

public class SetupUI : MonoBehaviour 
{
	void Awake ()
	{
		UIManager.Instance.Init();
		UIManager.Instance.paused = false;
		Destroy(gameObject);
	}
}
