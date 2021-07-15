using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class Control : MonoBehaviour
{
    public GameObject[] cube;

    int k = 1;
    int direction = 0;
    string clientId = "746633530";
    string username = "447855";
    string password = "ZCHBwqCmdzsEA3IFdQO8dCy6nBo=";
    string topic = "test";

    MqttClient mqttClient;

    // Start is called before the first frame update
    void Start()
    {   
        cube[0].GetComponent<Renderer>().material.color = Color.red;
        mqttClient = new MqttClient("183.230.40.39", 6002, false, null, null, MqttSslProtocols.None);
        mqttClient.Connect(clientId, username, password);
        mqttClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        
        Debug.Log(mqttClient.IsConnected.ToString());
    }

    void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string msg = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log(msg);
        if (msg.Equals("DOWN"))
            direction = -1;
        else if (msg.Equals("UP"))
            direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Text text = GameObject.FindWithTag("text").GetComponent<Text>();
        text.text = "楼层： "  + k.ToString();
        if (direction == 1)
        {
            if (k == 10)
            {
                Debug.Log("已到顶层");
            }
            else
            {
                cube[k - 1].GetComponent<Renderer>().material.color = Color.white;
                ++k;
                cube[k - 1].GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("电梯上行");
            }
            direction = 0;
        }
        if (direction == -1)
        {
            if (k == 1)
            {
                Debug.Log("已到底层");
            }
            else
            {
                cube[k - 1].GetComponent<Renderer>().material.color = Color.white;
                --k;
                cube[k - 1].GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("电梯下行");
            }
            direction = 0;
        }
    }
}
