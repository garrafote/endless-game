using UnityEngine;
using System.Collections;

public class Token : MonoBehaviour {

    UIManager ui;
    int abilityIndex;

	void Start () {
        ui = UIManager.Instance;
        abilityIndex = Random.Range(1, 10) + 1;
        Debug.Log("Ability/" + (abilityIndex % 10).ToString("00"));
        var sprite = Resources.Load<Sprite>("Abilities/" + (abilityIndex % 10).ToString("00"));
        var srenderer = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
        srenderer.sprite = sprite;
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
			ui.Abilities[abilityIndex-1].IncrementCharge();
        }

        Destroy(gameObject);
    }
}
