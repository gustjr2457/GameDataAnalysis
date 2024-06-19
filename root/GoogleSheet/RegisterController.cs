using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
    [Header("Input Box")]
    public TMP_InputField RegIDInput;
    public TMP_InputField RegPWInput;
    public TMP_InputField RegPWIncInput;
    public TMP_InputField RegNicknameInput;

    [Header("TextMeshPro")]
    public TextMeshProUGUI PWIncorrectText;
    

    string RegId, RegPass, RegPassAgain, nickname;

    [SerializeField] TextMeshProUGUI statusTmpReg;

    private void Start()
    {
        PWIncorrectText.text = "";
    }

    bool SetRegIDPass()
    {
        RegId = RegIDInput.text.Trim();
        RegPass = RegPWInput.text.Trim();
        RegPassAgain = RegPWIncInput.text.Trim();
        nickname = RegNicknameInput.text.Trim();

        if (RegId == "" || RegPass == "") return false;
        else return true;
    }

    public void Register()
    {
        if (!SetRegIDPass())
        {
            print("ID or password is empty.");
            statusTmpReg.text = "ID or password is empty.";
            return;
        }
        else if (RegPassAgain != RegPass)
        {
            PWIncorrectText.text = "Password doesn't match";
            return;
        }
        else if (nickname == "")
        {
            print("Please enter your nickname");
            statusTmpReg.text = "Please enter your nickname";
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", RegId);
        form.AddField("pass", RegPass);
        form.AddField("nickname", nickname);
        statusTmpReg.text = "Signing up for membership...";
        StartCoroutine(GoogleSheetManager.Instance.Post(form));
    }
}