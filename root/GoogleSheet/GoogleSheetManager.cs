using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginData
{
    public string order;
    public string result;
    public string msg;
}

public class GoogleSheetManager : Singleton<GoogleSheetManager>
{
    const string URL = "https://script.google.com/macros/s/AKfycbx0OuvkMvvCIC1qUtMzrt7eo8yRlTjU4x_qmojJGuHQxyXDNILp8oZIlbgjTR3SkdJk/exec";
    
    public CanvasSwapHandler canvasSwap;
    public TextMeshProUGUI statusTmp;

    [Header("Scriptable Object")]
    public LoginSO loginSo;


    IEnumerator Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("value", "��");

        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
    }

    public IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                print(www.downloadHandler.text);
                LoginData data = JsonUtility.FromJson<LoginData>(www.downloadHandler.text);

                Debug.Log($"{data.order}, {data.result}");

                if (data.order == "login" && data.result == "OK")
                {
                    SceneManager.LoadScene("Game");

                    string msg = data.msg;
                    string[] resultData = msg.Split(':');

                    loginSo.PlayerId = int.Parse(resultData[0]);
                    loginSo.UserName = resultData[1].ToString();

                }
                if (data.order == "register" && data.result == "OK")
                {
                    yield return new WaitForSecondsRealtime(1.0f);
                    canvasSwap.GoLogin();
                }
                if (data.order == "attack" && data.result == "OK")
                {
                    // TODO
                }
                if (data.order == "dead" && data.result == "OK")
                {
                    // TODO
                }
                if (data.order == "ondamaged" && data.result == "OK")
                {
                    // TODO
                }
                if (data.order == "move" && data.result == "OK")
                {
                    // TODO
                }
                if (statusTmp)
                    statusTmp.text = data.msg;
            }
            else
            {
                print("No response from the web.");
                statusTmp.text = "No response from the web.";
            }
        }
    }
}