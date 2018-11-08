using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour {

	public Transform content;

	public GameObject chatBarPrefab;

	public List<string> chatData = new List<string>();

	public Sprite user1Sprite;
	public Sprite user2Sprite;

	public Color user1ImageColor;
	public Color user2ImageColor;

	public Sprite user1ChatBarSprite;
	public Sprite user2ChatBarSprite;

	public Font chatFont;
	public Color textColor;
	public int fontSize;

	private VerticalLayoutGroup verticalLayoutGroup; 

	// ratio = heightinoriginalscreenheight/originalscreenheight
	// Use this for initialization
	void Start () {
		string[] chats = new string[]{
			"Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
			"Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
			"It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
			"It was popularized in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
			"Where does it come from?",
			"Contrary to popular belief, Lorem Ipsum is not simply random text.",
			"It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source."};

		for(int i = 0; i < 40; i++) {

			if(Random.Range(0,2) == 0)
				chatData.Add(chats[Random.Range(0,chats.Length)]+"~0");
			else
				chatData.Add(chats[Random.Range(0,chats.Length)]+"~1");
		}

		ShowUserMsg ();
		verticalLayoutGroup = content.GetComponent<VerticalLayoutGroup>();
	
	}


	public void ShowUserMsg () {
		
		for(int i = 0; i < chatData.Count; i++) {
			StartCoroutine(ShowUserMsgCoroutine (chatData[i]));
		}
	}

	public void ShowMessage (string msg) {
		chatData.Add(msg+"~1");
		StartCoroutine(ShowUserMsgCoroutine (chatData[chatData.Count - 1]));
	}

	IEnumerator ShowUserMsgCoroutine (string msg) {

		GameObject chatObj = Instantiate(chatBarPrefab,Vector3.zero, Quaternion.identity) as GameObject;
		chatObj.transform.SetParent(content.transform,false);

		chatObj.SetActive(true);

		ChatListObject clb = chatObj.GetComponent<ChatListObject>();

		string[] split  = msg.Split('~');
		msg = split[0];

		clb.parentText.fontSize = fontSize;
		clb.childText.fontSize = fontSize;
			
		clb.parentText.text = msg;

		clb.childText.color = Color.black;
		

		yield return new WaitForEndOfFrame();

		float height = chatObj.GetComponent<RectTransform>().rect.height;
		float width = chatObj.GetComponent<RectTransform>().rect.width;

		clb.chatbarImage.rectTransform.sizeDelta = new Vector2(width+5,height+6);
		clb.childText.rectTransform.sizeDelta = new Vector2(width,height);

		//clb.senderName.text = "Santa Claus";
		clb.childText.text = msg;
		// clb.childText.rectTransform.anchoredPosition();

		if (split[1] == "0") {
			clb.chatbarImage.color = user1ImageColor;

			clb.chatbarImage.sprite = user1ChatBarSprite;

			clb.chatbarImage.rectTransform.anchoredPosition = new Vector2(-3f,
				clb.chatbarImage.rectTransform.anchoredPosition.y);

		} else if (split[1] == "1") {

			clb.chatbarImage.color = user2ImageColor;

			clb.chatbarImage.sprite = user2ChatBarSprite;

			clb.chatbarImage.rectTransform.anchoredPosition = new Vector2(
				((content.GetComponent<RectTransform>().rect.width-
				(verticalLayoutGroup.padding.left+verticalLayoutGroup.padding.right))
				-chatObj.GetComponent<RectTransform>().rect.width)
				,clb.chatbarImage.rectTransform.anchoredPosition.y);

		}
	}
}
