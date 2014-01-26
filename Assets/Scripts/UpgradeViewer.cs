using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeViewer : Singleton<UpgradeViewer> {
	[System.Serializable]
	public class SpriteDef {
		public string name;
		public Sprite sprite;
	}

	[System.Serializable]
	public class MessageDef {
		public string name;
		public string message;
	}

	public int lineSize = 10;
	public Transform loveSprite;
	public Transform moneySprite;
	public Transform knowledgeSprite;
	public SpriteDef[] swordSprites;
	public MessageDef[] loveMessages;
	public MessageDef[] knowledgeMessages;
	int nextSword = 0;
	int nextLove = 0;
	int nextMind = 0;
	Transform player;
	SwordControl sword;
	string swordAdj = "Selfish";
	string swordMaterial = "Copper";
	string swordMind = "the Dunce";
	Transform moneyBase;
	Transform loveBase;
	Transform mindBase;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		sword = player.GetComponentInChildren<SwordControl>();
		moneyBase = new GameObject("Moneys").transform;
		moneyBase.parent = transform;
		loveBase = new GameObject("Loves").transform;
		loveBase.parent = transform;
		mindBase = new GameObject("Smarts").transform;
		mindBase.parent = transform;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			AddLove();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			AddGreed();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			AddKnowledge();
		}
	}

	void LateUpdate()
	{
		float offset = Camera.main.orthographicSize;
		transform.localPosition = new Vector3(0f, offset, 10f);

		float right = offset * Camera.main.aspect;
		loveBase.localPosition = new Vector3(right - 2f, -1f, 0f);
		moneyBase.localPosition = new Vector3(right - 1.5f, -1f, 0f);
		mindBase.localPosition = new Vector3(right - 1f, -1f, 0f);
	}

	void OnGUI()
	{
		string swordName = string.Format("{0} {1} Sword of {2}", swordAdj, swordMaterial, swordMind);
		GUI.Label(new Rect(10, 10, 250, 24), swordName);
	}
	

	public void AddLove()
	{
		if(nextLove < loveMessages.Length) {
			player.SendMessage(loveMessages[nextLove].message);
			swordAdj = loveMessages[nextLove].name;
			nextLove ++;
		}
		Transform item = (Transform)Instantiate(loveSprite);
		AddItem(item, loveBase);
	}

	public void AddGreed()
	{
		if(nextSword < swordSprites.Length) {
			sword.SetSprite(swordSprites[nextSword].sprite);
			swordMaterial = swordSprites[nextSword].name;
			nextSword ++;
		}
		Transform item = (Transform)Instantiate(moneySprite);
		AddItem(item, moneyBase);
	}

	public void AddKnowledge()
	{
		if(nextMind < knowledgeMessages.Length) {
			player.SendMessage(knowledgeMessages[nextMind].message);
			swordMind = knowledgeMessages[nextMind].name;
			nextMind ++;
		}
		Transform item = (Transform)Instantiate(knowledgeSprite);
		AddItem(item, mindBase);
	}

	void AddItem(Transform item, Transform parent)
	{
		item.parent = parent;
		item.localPosition = new Vector3(0f, -0.5f * (parent.childCount - 1), 0f);
	}
}
