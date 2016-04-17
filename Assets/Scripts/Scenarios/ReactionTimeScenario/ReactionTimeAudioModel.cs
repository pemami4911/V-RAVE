using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
public class ReactionTimeAudioModel : HUDAudioModel {

		public ReactionTimeAudioModel() : base()
		{
			// #0
			addClip ("changing-to-AI-mode", 2f);
			// #1
			addClip ("changing-to-manual-mode", 2f);
			// #2
			addClip ("car crashing", 6f);
			// #3
			addClip ("intersection-briefing", 9f);
			// #4 
			addClip ("drive-to-stop-sign", 3f);
			// #5
			addClip ("intersection-AI-briefing", 4f);
			// #6
			addClip ("Tires", 2f);
			// #7
			addClip ("intersection-briefing-2", 6f);
			// #8
			addClip ("trashcan-briefing", 11f);
			// #9
			addClip ("trashcan-briefing-2", 4f);
			// #10 
			addClip ("beep", 0.5f);
			// #11
			addClip ("right-paddle", 2f); 
		}

}
}