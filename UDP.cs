using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;


public class UDP : MonoBehaviour
{
    public ChatManager chat;
    public TMP_InputField IP;
    public TMP_InputField port;
    public GameObject networking;

    bool isServer;
    UdpClient instance;

    List<IPEndPoint> connections = new List<IPEndPoint>();

    void Awake() {
        IP.text = "127.0.0.1";
    }

    public void host() {
        try {
            isServer = true;
            instance = new UdpClient(int.Parse(port.text));
            instance.BeginReceive(OnReceive, null);
            networking.SetActive(false);
        } catch (Exception e) {
            Debug.LogError("Failed to open server");
        }
    }

    public void join() {
        try {
            isServer = false;
            instance = new UdpClient();
            AddClient(new IPEndPoint(IPAddress.Parse(IP.text), int.Parse(port.text)));
            instance.BeginReceive(OnReceive, null);
            networking.SetActive(false);
        } catch (Exception e) {
            Debug.LogError("Failed to join server");
        }
    }

    void OnReceive(IAsyncResult ar) {
        try {
            IPEndPoint ipEndpoint = null;
            byte[] data = instance.EndReceive(ar, ref ipEndpoint);

            print("Received message!");
            AddClient(ipEndpoint);

            string message = Encoding.UTF8.GetString(data);
            chat.addText(message);

            if (isServer) {
                BroadcastChatMessage(message);
            }
        }
        catch (SocketException e) {
            Debug.LogError(e);
        } finally {
            instance.BeginReceive(OnReceive, null);
        }
    }

    void BroadcastChatMessage(string message) {
        foreach (IPEndPoint ip in connections) {
            Send(message, ip);
            print("Sent!");
        }
    }

    void AddClient(IPEndPoint ip) {
        if (!connections.Contains(ip)) {
            connections.Add(ip);
            print("Connected to " + ip);
        }
    }

    void RemoveClient(IPEndPoint ip) {
        connections.Remove(ip);
    }

    public void Send(string message) {
        if (isServer) {
            chat.addText(message);
        }

        BroadcastChatMessage(message);
    }

    void Send(string message, IPEndPoint ip) {
        byte[] data = Encoding.UTF8.GetBytes(message);
        instance.Send(data, data.Length, ip);
    }
}
