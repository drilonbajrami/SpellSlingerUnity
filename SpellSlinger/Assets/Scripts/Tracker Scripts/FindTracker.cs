using UnityEngine;
using Valve.VR;
using System.Text;
using System;

namespace SpellSlinger
{
    /// <summary>
    /// Finds the specified tracker and assigns it automatically to the gameObject it is a component of.
    /// </summary>
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class FindTracker : MonoBehaviour
    {
        // Hand Tracker to be used
        public TrackerID trackerToUse = TrackerID.LeftHandTracker;

        private void Start()
        {
            SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
            try { TryGetTracker(trackedObject); }
            catch { Debug.Log("Failed to find tracker... Trying agian!"); }
        }

        /// <summary>
        /// Search for all available trackers and assign the matching one to this current object
        /// </summary>
        void TryGetTracker(SteamVR_TrackedObject pTrackedObject)
        {
            for (int i = 0; i < SteamVR.connected.Length; ++i) {
                ETrackedPropertyError error = new ETrackedPropertyError();
                StringBuilder serialNumber = new StringBuilder();
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, serialNumber, OpenVR.k_unMaxPropertyStringSize, ref error);
           
                if (serialNumber.ToString() == trackerToUse.GetStringValue()) {
                    pTrackedObject.SetDeviceIndex(i);
                    break;
                }
            }
        }

        /// <summary>
        /// List all available tracking devices and print out the serial and model numbers
        /// </summary>
		void ListDevices()
        {
            for (int i = 0; i < SteamVR.connected.Length; ++i)
            {
                ETrackedPropertyError error = new ETrackedPropertyError();
                StringBuilder sb = new StringBuilder();
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var SerialNumber = sb.ToString();

                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var ModelNumber = sb.ToString();
                if (SerialNumber.Length > 0 || ModelNumber.Length > 0)
                    Debug.Log("Device " + i.ToString() + " = " + SerialNumber + " | " + ModelNumber);
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