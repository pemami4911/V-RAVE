using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
	public class LanePassingAudioModel : HUDAudioModel
	{

		public LanePassingAudioModel() : base()
		{
			// #0
			addClip("intro-briefing", 7f);
			// #1
			addClip("follow-briefing", 13f);
			// #2
			addClip("speed-changes-briefing", 5f);
			// #3
			addClip("passing-briefing", 5f);
			// #4 
			addClip("end-user-mode", 7f);
		//	// #5
		//	addClip("intersection-AI-briefing", 4f);
		//	// #6
		//	addClip("Tires", 2f);
		//	// #7
		//	addClip("intersection-briefing-2", 6f);
		//	// #8
		//	addClip("trashcan-briefing", 11f);
		//	// #9
		//	addClip("trashcan-briefing-2", 4f);
		}

	}
}