using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text
using System;
using UnityEngine.Networking;

/// <summary>
/// Manager main.
/// </summary>
public class ManagerMain : MonoBehaviour
{
    [SerializeField] private Text displayField;
    [SerializeField] private Text inputName;
    [SerializeField] private Text inputMessage;

    /// <summary>
    /// Raises the click clear display event.
    /// </summary>
    public void OnClickClearDisplay()
    {
        displayField.text = " ";
    }

    /// <summary>
    /// Raises the click get json from www event.
    /// </summary>
    public void OnClickGetMessagesApi()
    {
        displayField.text = "wait...";
        GetJsonFromWww();
    }

    /// <summary>
    /// Raises the click get json from www event.
    /// </summary>
    public void OnClickSetMessageApi()
    {
        displayField.text = "wait...";
        SetJsonFromWww();
    }

    /// <summary>
    /// Gets the json from www.
    /// </summary>
    private void GetJsonFromWww()
    {
        // APIが設置してあるURLパス
        const string url = "http://localhost/potomemosystem/potomemo/Getmemo";

        // Wwwを利用して json データ取得をリクエストする
        StartCoroutine(GetMessages(url, CallbackWwwSuccess, CallbackWwwFailed));
    }

    /// <summary>
    /// Callbacks the www success.
    /// </summary>
    /// <param name="response">Response.</param>
    private void CallbackWwwSuccess(string response)
    {
        // json データ取得が成功したのでデシリアライズして整形し画面に表示する
        List<MessageData> messageList = MessageDataModel.DeserializeFromJson(response);

        string sStrOutput = "";
        foreach (MessageData message in messageList)
        {
            sStrOutput += $"Name:{message.Name}\n";
            sStrOutput += $"Message:{message.Message}\n";
            sStrOutput += $"Date:{message.Date}\n";
        }

        displayField.text = sStrOutput;
    }

    /// <summary>
    /// Callbacks the www failed.
    /// </summary>
    private void CallbackWwwFailed()
    {
        // jsonデータ取得に失敗した
        displayField.text = "Www Failed";
    }

    /// <summary>
    /// Callbacks the API success.
    /// </summary>
    /// <param name="response">Response.</param>
    private void CallbackApiSuccess(string response)
    {
        // json データ取得が成功したのでデシリアライズして整形し画面に表示する
        displayField.text = response;
    }

    /// <summary>
    /// Downloads the json.
    /// </summary>
    /// <returns>The json.</returns>
    /// <param name="url">S tgt UR.</param>
    /// <param name="cbkSuccess">Cbk success.</param>
    /// <param name="cbkFailed">Cbk failed.</param>
    private IEnumerator GetMessages(string url, Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        // WWWを利用してリクエストを送る
        var webRequest = UnityWebRequest.Get(url);

        //タイムアウトの指定
        webRequest.timeout = 5;

        // WWWレスポンス待ち
        yield return webRequest.SendWebRequest();

        if (webRequest.error != null)
        {
            //レスポンスエラーの場合
            Debug.LogError(webRequest.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (webRequest.isDone)
        {
            // リクエスト成功の場合
            Debug.Log($"Success:{webRequest.downloadHandler.text}");
            if (null != cbkSuccess)
            {
                cbkSuccess(webRequest.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Response the check for time out WWW.
    /// </summary>
    /// <returns>The check for time out WWW.</returns>
    /// <param name="webRequest">Www.</param>
    /// <param name="timeout">Timeout.</param>
    private IEnumerator ResponseCheckForTimeOutWWW(UnityWebRequest webRequest, float timeout)
    {
        float requestTime = Time.time;

        while (!webRequest.isDone)
        {
            if (Time.time - requestTime < timeout)
            {
                yield return null;
            }
            else
            {
                Debug.LogWarning("TimeOut"); //タイムアウト
                break;
            }
        }

        yield return null;
    }


    /// <summary>
    /// Sets the json from www.
    /// </summary>
    private void SetJsonFromWww()
    {
        // APIが設置してあるURLパス
        string sTgtURL = "http://localhost/potomemosystem/potomemo/Setmemo";

        string name = inputName.text;
        string message = inputMessage.text;

        // Wwwを利用して json データ取得をリクエストする
        StartCoroutine(SetMessage(sTgtURL, name, message, CallbackApiSuccess, CallbackWwwFailed));
    }

    /// <summary>
    /// Sets the message.
    /// </summary>
    /// <returns>The message.</returns>
    /// <param name="url">URL target.</param>
    /// <param name="name">Name.</param>
    /// <param name="message">Message.</param>
    /// <param name="cbkSuccess">Cbk success.</param>
    /// <param name="cbkFailed">Cbk failed.</param>
    private IEnumerator SetMessage(string url, string name, string message, Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("message", message);

        // WWWを利用してリクエストを送る
        var webRequest = UnityWebRequest.Post(url, form);

        //タイムアウトの指定
        webRequest.timeout = 5;

        // WWWレスポンス待ち
        yield return webRequest.SendWebRequest();

        if (webRequest.error != null)
        {
            //レスポンスエラーの場合
            Debug.LogError(webRequest.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (webRequest.isDone)
        {



            // リクエスト成功の場合
            Debug.Log($"Success:{webRequest.downloadHandler.text}");
            if (null != cbkSuccess)
            {
                cbkSuccess(webRequest.downloadHandler.text);
            }
        }
    }
}