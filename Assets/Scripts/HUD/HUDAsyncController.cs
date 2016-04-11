using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
	public class HUDAsyncController : MonoBehaviour
	{

		private HUDAudioController hudAudioController;
		private HUDController hudTextController;

		// use GetComponentInParents<...> in your scenario
		public void Configure(HUDAudioController hudAC, HUDController hudC)
		{
			hudAudioController = hudAC;
			hudTextController = hudC;
		}

		public void RepeatAudio (int idx, int numTimes)
		{
			StartCoroutine (CycleThroughAudio (idx, numTimes));
		}

		private IEnumerator CycleThroughAudio (int idx, int numTimes)
		{
			
			for (int i = 0; i < numTimes; ++i) 
			{
				hudAudioController.playAudio (idx);
				yield return new WaitForSeconds (hudAudioController.audioModel.durations [idx]);
			}
		}

		// cycle through the HUD models "repeats" num times in a row
		public void DoHUDUpdates(int repeats, float timeBetween)
		{
			StartCoroutine (RepeatCycleThroughHUDModels (repeats, timeBetween));
		}

		public void DoHUDUpdates ()
		{
			StartCoroutine (CycleThroughHUDModels ());
		}

		private IEnumerator RepeatCycleThroughHUDModels(int repeats, float timeBetween)
		{
			for (int i = 0; i < repeats; ++i) 
			{
				DoHUDUpdates ();
				yield return new WaitForSeconds (timeBetween);
			}
		}

		private IEnumerator CycleThroughHUDModels ()
		{
			for (int i = 0; i < hudTextController.models.Length; ++i) 
			{
				hudTextController.model = hudTextController.models [i]; 
				yield return new WaitForSeconds (hudTextController.durations [i]); 
			}
		}
	}

}