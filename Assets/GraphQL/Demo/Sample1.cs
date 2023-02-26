using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphQL4Unity;
using UnityEngine.UI;

public class Sample1 : MonoBehaviour
{
    public Text City1Text;
    public Text City2Text;
    public Text StatusText;
    public Text ValueText;

    public GraphQLHttp HttpConnection;
    public GraphQLWebsocket WebsocketConnection;

    public GraphQLQueryBaseVar Temperature1;
    public GraphQLQueryBaseVar Temperature2;

    public GraphQLQuery Query;
    public GameObject Cube;

    // Start is called before the first frame update
    void Start()
    {        
        if (HttpConnection != null)
        {
            HttpConnection.Headers = new List<Header>() // Example how to set headers
            {
                new Header() { Key = "a", Value = "b"}
            };

            HttpConnection.ExecuteQuery("query { test }", null, (message) => // Execute a query in code
            {
                if (message.Type != MessageType.GQL_DATA)
                {
                    Debug.Log("Exception: " + message.Type + " " + message.Result.ToString());
                }
                else
                {
                    Debug.Log("Result: " + message.Result.ToString());
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (City1Text != null && Temperature1 != null) 
            City1Text.text = "Temperature in " + City1Text.name + " is " + Temperature1.AsString();
        if (City2Text != null && Temperature2 != null)
            City2Text.text = "Temperature in " + City2Text.name + " is " + Temperature2.AsString();
        if (StatusText != null && WebsocketConnection != null) 
            StatusText.text = WebsocketConnection.Connected ? "GraphQL WS Connected" : "Not connected";
    }

    public void ResultEvent(GraphQLResult result)
    {
        //Debug.Log("Data is here! "+result.Data.ToString());
        if (ValueText != null && Cube != null && result != null && result.Data.ContainsKey("NodeValue"))
        {
            ValueText.text = result.Data["NodeValue"]["Value"].ToString();
            var value = result.Data["NodeValue"]["Value"].ToObject<float>();
            Cube.transform.Rotate(new Vector3(value, value, value));
        }
    }

    public void ExecuteQuery()
    {
        Query.ExecuteQuery = true;
    }

}
