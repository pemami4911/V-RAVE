using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{

	public class WeatherAudioModel : HUDAudioModel {

		public WeatherAudioModel() : base() {
			// #0
			addClip ("changing-to-AI-mode", 2f);
			// #1
			addClip ("changing-to-manual-mode", 2f);
			// #2
			addClip ("car crashing", 6f);
			// #3
			addClip ("weather_intro", 9f);
			// #4
			addClip ("Tires", 2f);
			// #5 
			addClip ("beep", 0.5f);

			// #5 
			//addClip ("drive-to-stop-sign", 3f);
			// #6
			//addClip ("intersection-AI-briefing", 4f);

			// #7
			//addClip ("intersection-briefing-2", 6f);
			// #8
			//addClip ("trashcan-briefing", 11f);
			// #9
			//addClip ("trashcan-briefing-2", 4f);

		}
	}
}
