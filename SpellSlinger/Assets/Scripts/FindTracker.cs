using UnityEngine;
using Valve.VR;
using System.Text;

namespace SpellSlinger
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class FindTracker : MonoBehaviour
    {
        public TrackerID trackerToUse = TrackerID.LeftHandTracker;

		private void Start()
		{
            TryGetTracker();
		}

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

    public enum TrackerID : int
    {
        [StringValue("LHR-91E2A79C")]
        LeftHandTracker = 1,

        [StringValue("LHR-F635291B")]
        RightHandTracker = 2,
    }
}
