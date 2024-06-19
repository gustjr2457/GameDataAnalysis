using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PlayLogManager : Singleton<PlayLogManager>
{
    public void UserKillLog(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "log");
        form.AddField("id", id);
        form.AddField("type", "userkilllog");
        form.AddField("description", "���� ų");
        StartCoroutine(GoogleSheetManager.Instance.Post(form));
    }

    public void MonsterKillLog(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "log");
        form.AddField("id", id);
        form.AddField("type", "monsterkilllog");
        form.AddField("description", "���� ų");
        StartCoroutine(GoogleSheetManager.Instance.Post(form));
    }

    public void DeadLog(int id, string reason)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "log");
        form.AddField("id", id);
        form.AddField("type", "userdead");
        form.AddField("description", reason);
        StartCoroutine(GoogleSheetManager.Instance.Post(form));
    }
}