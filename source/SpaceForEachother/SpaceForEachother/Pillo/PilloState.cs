using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveCommander.Pillo {
	public class PilloState {		
		public static bool LeftJustPressed { get; set; }
		public static bool LeftHolding { get; set; }
		public static bool LeftReleased { get; set; }

		public static bool RightJustPressed { get; set; }
		public static bool RightHolding { get; set; }
		public static bool RightReleased { get; set; }

		public static bool Holding(PlayerHand hand) {
			if (hand == null) {
				return false;
			}
			if (hand.Hand == Microsoft.Kinect.JointType.HandLeft) {
				return LeftHolding;
			}

			if (hand.Hand == Microsoft.Kinect.JointType.HandRight) {
				return RightHolding;
			}

			return false;
		}
	}
}
