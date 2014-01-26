using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	public enum PickupType {
		MONEY,
		LOVE,
		KNOWLEDGE
	};

	public PickupType pickupType;

	void OnTriggerEnter2D (Collider2D other) {
		switch(pickupType) {
		case PickupType.LOVE:
			UpgradeViewer.instance.AddLove();
			break;
		case PickupType.MONEY:
			UpgradeViewer.instance.AddGreed();
			break;
		case PickupType.KNOWLEDGE:
			UpgradeViewer.instance.AddKnowledge();
			break;
		}
		Destroy(gameObject);
	}
}
