using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginManager : Singleton<LoginManager>
{
    [Header("Input Box")]
    public TMP_InputField IDInput;
    public TMP_InputField PassInput;
    
    [Header("TextMeshPro")] 
    public TextMeshProUGUI statusTmp;

    string id, pass;

    bool SetIDPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();

        if (id == "" || pass == "") return false;
        else return true;
    }

    public void Login()
    {
        if (!SetIDPass())
        {
            print("ID or password is empty.");
            statusTmp.text = "ID or password is empty.";
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);
        statusTmp.text = "Logging in...";

        StartCoroutine(GoogleSheetManager.Instance.Post(form));
    }

    public void Logout()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(GoogleSheetManager.Instance.Post(form));
    }

    void OnApplicationQuit()
    {
        Logout();
    }
}