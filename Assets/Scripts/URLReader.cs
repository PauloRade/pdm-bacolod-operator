using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
// http://localhost:8000/?username=Alex&usercode=93ANS&securitycode=pmaby  this is how you pass it
public class URLReader : MonoBehaviour
{
    //public TextMeshProUGUI textDisplay;
    private Dictionary<string, string> urlParameters = new Dictionary<string, string>();

    public GameObject blockerUI;

    void Awake()
    {
        blockerUI.SetActive(true);
        // Only attempt to parse if we are running in an actual WebGL build
        #if UNITY_WEBGL && !UNITY_EDITOR
        ParseURLParameters(Application.absoluteURL);
        #else
        Debug.Log("Not running in WebGL build. Using fallback/editor logic.");
        // Optional: Hardcode test data for working inside the Unity Editor
        urlParameters["username"] = "UserName1";
        urlParameters["usercode"] = "any";
        urlParameters["securitycode"] = "malasakit8921";
        #endif
    }

    void Start()
    {
        // Example: Retrieve a parameter safely
        string username = GetParam("username", "GuestUser");
        string usercode = GetParam("usercode", "0");
        string securitycode = GetParam("securitycode", "0");

        Debug.Log($"Loaded User: {username}, Room: {usercode}");

        //textDisplay.text = $"User: {username}\nRoom: {roomId}";

        //textDisplay.text = $"{username}";
        if(securitycode == "malasakit8921")
        {
            blockerUI.SetActive(false);
        }

        
    }

    private void ParseURLParameters(string url)
    {
        if (string.IsNullOrEmpty(url)) return;

        // Find the start of the query string (the '?' character)
        int questionMarkIndex = url.IndexOf('?');
        if (questionMarkIndex == -1) return;

        // Extract everything after the '?'
        string queryString = url.Substring(questionMarkIndex + 1);

        // Separate individual key-value chunks by splitting on '&'
        string[] pairs = queryString.Split('&');

        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split('=');
            if (keyValue.Length == 2)
            {
                string key = CleanURLString(keyValue[0]);
                string value = CleanURLString(keyValue[1]);

                // Store or overwrite the parameter key-value pair
                urlParameters[key] = value;
            }
        }
    }
    // A simple, bulletproof decoder that won't crash the WebGL call stack
    private string CleanURLString(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        
        // Convert URL plus signs back to spaces
        string processed = input.Replace("+", " ");
        
        // Handle basic hex URL encoding (like %20 for space) if necessary
        return System.Uri.UnescapeDataString(processed);
    }

    /// <summary>
    /// Public helper method to safely query parameters with a fallback value.
    /// </summary>
    public string GetParam(string key, string defaultValue = "")
    {
        if (urlParameters.TryGetValue(key, out string value))
        {
            return value;
        }
        return defaultValue;
    }
}