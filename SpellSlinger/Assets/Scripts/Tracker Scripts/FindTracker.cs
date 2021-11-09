using UnityEngine;
using Valve.VR;
using System.Text;

namespace SpellSlinger
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class FindTracker : MonoBehaviour
    {
        // Hand Tracker to be used
        public TrackerID trackerToUse = TrackerID.LeftHandTracker;

		private void Awake()
		{
            TryGetTracker();
		}

        /// <summary>
        /// Search for all available trackers and assign the matching one to this current object
        /// </summary>
        void TryGetTracker()
        {
            for (int i = 0; i < SteamVR.connected.Length; ++i)
            {
                ETrackedPropertyError error = new ETrackedPropertyError();
                StringBuilder serialNumber = new StringBuilder();
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, serialNumber, OpenVR.k_unMaxPropertyStringSize, ref error);
           
                if (serialNumber.ToString() == trackerToUse.GetStringValue())
                {
                    GetComponent<SteamVR_TrackedObject>().SetDeviceIndex(i);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Stores the tracker serial name in a string on enum values
    /// </summary>
    public enum TrackerID : int
    {
        [StringValue("LHR-91E2A79C")]
        LeftHandTracker = 1,

        [StringValue("LHR-F635291B")]
        RightHandTracker = 2,
    }
}
