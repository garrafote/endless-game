using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ability : ScriptableObject
{
	//Cooldown Indicator
	//Charges Display

	public GameObject AbilityContainer;

	public Image AbilityBackground;
	public Image AbilityIcon;


	public int index;
	public Text ChargeDisplay;
	public Image CooldownDisplay;

	public bool unlimited;
	public int charges;
	public float cooldownLeft;
	public float cooldownDuration;

	public void Update()
	{
		if(cooldownLeft > 0)
		{
			cooldownLeft -= Time.deltaTime;
			SetCooldown(cooldownLeft, cooldownDuration);
			DisableAbility();
		}
		else
		{
			if (!UIManager.Instance.canUse)
			{
				DisableAbility();
			}
			else
			{
				EnableAbility();
			}
			cooldownLeft = 0;
		}
	}

	public bool UseAbility()
	{
		if(cooldownLeft <= 0 && (unlimited || charges > 0))
		{
			StartCooldown();
			DisableAbility();

			if (!unlimited)
			{
				charges--;
				SetCharges(charges);
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public void StartCooldown()
	{
		cooldownLeft = cooldownDuration;
	}

	public void DisableAbility()
	{
		AbilityBackground.sprite = UIManager.Instance.Icons[10];
	}

	public void EnableAbility()
	{
		AbilityBackground.sprite = UIManager.Instance.Icons[11];
	}

	public void IncrementCharge(int amountGained = 1)
	{
		charges += amountGained;
		SetCharges(charges);
	}

	public void SetCharges(int amount)
	{
		ChargeDisplay.text = amount.ToString();
	}

	public void SetCooldown(float curTime, float cooldownLength)
	{
		CooldownDisplay.fillAmount = curTime / cooldownLength;
	}
}
