using UnityEngine;
using System.Collections;

public class Vending : MonoBehaviour {
	public Rigidbody2D loveItem;
	public Rigidbody2D knowledgeItem;
	public Rigidbody2D greedItem;
	public GameObject loveFire; 
	public GameObject knowledgeFire; 
	public GameObject greedFire;

	void YouHaveChosen(int buttonIndex)
	{
		switch(buttonIndex) {
		default:
		case 0:
			loveItem.isKinematic = false;
			greedFire.SetActive(true);
			knowledgeFire.SetActive(true);
			Destroy(greedItem.gameObject, 1f);
			Destroy(knowledgeItem.gameObject, 1f);
			break;
		case 1:
			knowledgeItem.isKinematic = false;
			knowledgeFire.SetActive(true);
			loveFire.SetActive(true);
			greedFire.SetActive(true);
			Destroy(loveItem.gameObject, 1f);
			Destroy(greedItem.gameObject, 1f);
			break;
		case 2:
			greedItem.isKinematic = false;
			loveFire.SetActive(true);
			knowledgeFire.SetActive(true);
			Destroy(loveItem.gameObject, 1f);
			Destroy(knowledgeItem.gameObject, 1f);
			break;
		}
	}
}
