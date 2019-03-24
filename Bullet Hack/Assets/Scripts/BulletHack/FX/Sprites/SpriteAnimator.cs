using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletHack.FX.Sprites
{
	[RequireComponent (typeof(SpriteSheet))]
	public class SpriteAnimator : MonoBehaviour
	{
		public bool running = true;

		public int minIndex = 0;
		public int maxIndex = 15;
		public int framesPerSecond = 15;
		public bool reverse = false;
		public bool ditherAnimation = false;
		public bool useDeltaTime = true;

		public LoopbackMode loopbackMode = LoopbackMode.Loop;

		private SpriteSheet sheet;

		private double tick;
		private float lastTime;

		private void Start ()
		{
			sheet = GetComponent<SpriteSheet> ();

			if (ditherAnimation && sheet)
				sheet.Index = Random.Range (minIndex, maxIndex);

			lastTime = Time.realtimeSinceStartup;
		}

		private void Update ()
		{
			if (!running)
				return;
		
			if(useDeltaTime)
				tick += Time.deltaTime;
			else
				tick += Time.realtimeSinceStartup - lastTime;

			lastTime = Time.realtimeSinceStartup;
			
			if (tick > 1F / framesPerSecond) {
				tick = 0;

				int cur = sheet.Index + (reverse ? -1 : 1);

				if ((!reverse && (cur > maxIndex || cur > sheet.SpriteCount)) || (reverse && (cur < minIndex || cur < 0))) {
					switch (loopbackMode) {
						case LoopbackMode.Reverse:
							reverse = !reverse;
							break;
						case LoopbackMode.Stop:
							running = false;
							break;
						case LoopbackMode.Loop:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					cur = reverse ? maxIndex : minIndex;
				}

				sheet.Index = cur;
			}
		}

		public enum LoopbackMode
		{
			Stop = 0,
			Loop = 1,
			Reverse = 2
		}
	}
}
