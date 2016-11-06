using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/* 
 * Credit to: mindlube (Original idea); KEMBL, Chris Sinclair (Suggestions)
 * (from C# Serialization + PlayerPrefs mystery: http://forum.unity3d.com/threads/72156-C-Serialization-PlayerPrefs-mystery)
 *
 * Issues:
 *  + May not work on iOS
 *    - Workaround solution: set the stripping level to UseMicroMSCorlib (http://docs.unity3d.com/Documentation/Manual/iphone-playerSizeOptimization.html)
 */

public class PlayerPrefsSerializer
{
		private static BinaryFormatter bf = new BinaryFormatter ();
		
		// serializableObject is any struct or class marked with [System.Serializable]
		public static void Save (string prefKey, object serializableObject)
		{
				MemoryStream memoryStream = new MemoryStream ();
				bf.Serialize (memoryStream, serializableObject);
				string tmp = System.Convert.ToBase64String (memoryStream.ToArray ());
				PlayerPrefs.SetString (prefKey, tmp);
		}

		public static T Load<T> (string prefKey)
		{
				if (!PlayerPrefs.HasKey (prefKey))
						return default(T);

				string serializedData = PlayerPrefs.GetString (prefKey);
				MemoryStream dataStream = new MemoryStream (System.Convert.FromBase64String (serializedData));
				T deserializedObject = (T)bf.Deserialize (dataStream);
				return deserializedObject;
		}

		//Edit Here - B
		//
		//
		//Edit Here - E
}
