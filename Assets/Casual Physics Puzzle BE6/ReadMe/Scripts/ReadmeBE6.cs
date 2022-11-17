using System;
using UnityEngine;

namespace Assets.Casual_Physics_Puzzle_BE6.ReadMe.Scripts
{
	public class ReadmeBE6 : ScriptableObject {
		public Texture2D icon;
		public string title;
		public Section[] sections;
		public bool loadedLayout;
	
		[Serializable]
		public class Section {
			public string heading, text, linkText, url;
		}
	}
}
