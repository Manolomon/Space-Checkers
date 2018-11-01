using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour {

    // Apariencia
    public Sprite playerChatBarSprite;
    public Sprite otherPlayerChatBarSprite;

    public Font chatFont;
    public Color textColor;
    public int fontSize;

    public Color user1ImageColor;
    public Color user2ImageColor;

    // Mensajes
    public List<string> chatData = new List<string>();

    public GameObject chatBarPrefab;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color playerMessage, otherPlayer;

   

	void Start () 
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToChat(chatBox.text, Message.MessageType.playerMessage);
            chatBox.text = "";
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
        }

        if (!chatBox.isFocused){

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("Presionaste la barra espaciadora", Message.MessageType.otherPlayer);
                Debug.Log("Space");
            }
        }

	}

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        // Nuevos mensajes
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        //chatData.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = otherPlayer;

        switch(messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        otherPlayer
    }
}