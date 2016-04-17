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
			// #5
			addClip("ai-briefing", 7f);
			// #6
			addClip("ai-passing-briefing", 5f);
			// #7
			addClip("adaptive-cruise-control", 7f);
		//	// #8
		//	addClip("ai-passing-command", 11f);
		//	// #9
		//	addClip("scenario-end", 4f);
		//  // #10
		//	addClip("leave-scenario", 5f);
		}

	}
}