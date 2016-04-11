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

		}

}
}