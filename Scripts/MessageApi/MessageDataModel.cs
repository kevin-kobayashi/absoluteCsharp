using System.Collections;
using System.Collections.Generic;
using MiniJSON;     // Json

/// <summary>
/// Json response manager.
/// </summary>
public class MessageDataModel
{

	/// <summary>
	/// Deserialize from json.
	/// MessageData型のリストがjsonに入っていると仮定して
	/// </summary>
	/// <returns>The from json.</returns>
	/// <param name="sStrJson">S string json.</param>
	public static List<MessageData> DeserializeFromJson(string sStrJson)
	{
		var ret = new List<MessageData>();

		// JSONデータは最初は配列から始まるので、Deserialize（デコード）した直後にリストへキャスト      
		IList jsonList = (IList)Json.Deserialize(sStrJson);

		// リストの内容はオブジェクトなので、辞書型の変数に一つ一つ代入しながら、処理
		foreach (IDictionary jsonOne in jsonList)
		{

			//新レコード解析開始
			var tmp = new MessageData();

			if (jsonOne.Contains("Name"))
			{
				tmp.Name = (string)jsonOne["Name"];
			}
			if (jsonOne.Contains("Message"))
			{
				tmp.Message = (string)jsonOne["Message"];
			}
			if (jsonOne.Contains("Date"))
			{
				tmp.Date = (string)jsonOne["Date"];
			}

			//現レコード解析終了
			ret.Add(tmp);
		}
		return ret;
	}
}

