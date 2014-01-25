using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
	static T _instance;
	
	public static T instance {
		get {
			if(!_instance) {
				_instance = (T)FindObjectOfType(typeof(T));
			}
			return _instance;
		}
	}
}
