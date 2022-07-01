using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Cubie.Game
{
	public class ImageDownloader : MonoBehaviour
	{
		[SerializeField] private RawImage texture;

		// Start is called before the first frame update
		void OnEnable()
		{
			int rand = new System.Random().Next(0, 10000);
			string url = string.Format("https://picsum.photos/seed/{0}/200", rand);
			StartCoroutine(GetTexture(url));
		}

		IEnumerator GetTexture(string url)
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
			yield return www.SendWebRequest();

			Debug.Log(www.result);

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				texture.texture = myTexture;
			}
		}
	}
}
