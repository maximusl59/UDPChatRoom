using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public bool isSelected = false;
    public UDP udp;

    public TMP_InputField username;
    public TMP_InputField discourse;
    public TMP_Text content;

    public void flipIsSelected() {
        isSelected = !isSelected;
    }

    void Update() {
        if (isSelected) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                udp.Send("[" + username.text + "]: " + discourse.text);
                discourse.text = "";
            }
        }      
    }

    public void addText(string text) {
        if (content.text == "") {
            content.text = text;
        } else {
            content.text = content.text + "\n" + text;
        }
    }
}
