using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class GestureReceiver : MonoBehaviour
    {
        

        [Header("Available Spells")]
        [SerializeField] private List<SpellType> _spells;

        void Start()
        {
            GestureCaster.LHandPose += PrintLeftHandPose;
            GestureCaster.RHandPose += PrintRightHandPose;
            GestureCaster.StartCastHandPose += OnStartCastPose;
            GestureCaster.CastSpellHandPose += OnCastSpellPose;
        }

        void Update()
        {
        
        }

        private void PrintLeftHandPose(object source, string value)
        {
            Debug.Log($"L HAND POSE: { value }");
        }

        private void PrintRightHandPose(object source, string value)
        {
            //Debug.Log($"R HAND POSE: { value }");
        }

        private void OnStartCastPose(object source, EventArgs e)
        {
            Debug.Log("Starting Cast...");
        }

        private void OnCastSpellPose(object source, EventArgs e)
        {
            Debug.Log("Casting spell...");
        }

        // UI Feedback
        //public TMP_Text spellNameText;
        //public Slider waitGauge;

        // Hands and HandEngine Clients
        public GameObject hmd;
        public GameObject leftHand;
        public GameObject rightHand;
        private HandEngine_Client leftHandClient;
        private HandEngine_Client rightHandClient;

        // Duration for posing a sign (letter)
        public float spellSignWaitTimeInSeconds;
        public List<Spell> spells;

        private Spell currentSpell = null; // Cache the spell in order to use it against enemies
        //StringBuilder currentSpellName;
        Coroutine currentCoroutine;
        bool castingSpell = false;

        //public static event Action<string> OnLetterSpelled = (letter) => { };
        //public static event Action SpellSuccessful;
        //public static event Action SpellFailed;
        //public static event Action SpellStarted;

        //void Start()
        //{
        //    currentSpellName = new StringBuilder();
        //    waitGauge.maxValue = spellSignWaitTimeInSeconds;
        //    leftHandClient = leftHand.GetComponent<HandEngine_Client>();
        //    rightHandClient = rightHand.GetComponent<HandEngine_Client>();
        //}
    
        //void Update()
        //{
        //    // If you are not casting a spell and show the OK sign, you start spell recognition
        //    if (!castingSpell && leftHandClient.poseName == _castPoseLeftHand && rightHandClient.poseName == _castPoseRightHand) // Both hands
        //        StartSpellListener();

        //    // Show the ASL letters that have been spelled out
        //    spellNameText.text = currentSpellName.ToString();

        //    // UI Feedback update
        //    if (castingSpell) 
        //        waitGauge.value += Time.deltaTime;

        //    //if (currentSpell != null && rightHandClient.poseName == castPose) 
        //    //CastSpellOnEnemy(GetEnemyTarget());

        //    if (currentSpell != null & Vector3.Distance(rightHand.transform.position, hmd.transform.position) > 0.5 && rightHandClient.poseName == castPose)
        //        CastSpellOnEnemy(GetEnemyTarget());
        //}

        //IEnumerator SpellListener()
        //{
        //    SpellStarted();
        //    bool keepListening = true;
        //    waitGauge.gameObject.SetActive(true);

        //    while (keepListening)
        //    {
        //        waitGauge.value = 0;
        //        yield return new WaitForSeconds(spellSignWaitTimeInSeconds);
        //        if (leftHandClient.poseName == _castPoseLeftHand && rightHandClient.poseName == _castPoseRightHand) // Both hands
        //        {
        //            keepListening = false;
        //        }
        //        else if (IsSignLetterValid(leftHandClient.poseName[0]))
        //        {
        //            currentSpellName.Append(leftHandClient.poseName[0]);
        //            OnLetterSpelled(leftHandClient.poseName);
        //        }
        //    }

        //    Spell spell = CheckIfSpellAvailable(currentSpellName.ToString());

        //    if (spell != null)
        //        StartCoroutine(CastSpell(spell));
        //    else
        //    {
        //        SpellFailed();
        //        currentSpellName.Clear();
        //        waitGauge.gameObject.SetActive(false);
        //        yield return new WaitForSeconds(1.0f);
        //        castingSpell = false;
        //    }
        //}

        //IEnumerator CastSpell(Spell spell)
        //{
        //    currentSpell = spell;
        //    yield return new WaitForSeconds(spellSignWaitTimeInSeconds);
        //    castingSpell = false;
        //    waitGauge.gameObject.SetActive(false);
        //}

        //public void CastSpellOnEnemy(Enemy enemy)
        //{
        //    if(enemy != null)
        //    {
        //        enemy.TakeDamage(currentSpell);
        //        currentSpell = null;
        //    }
        //}

        // Get the enemy target if pointing at one
        //public Enemy GetEnemyTarget()
        //{
        //    //rightHand.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit hit);

        //    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 500.0f))
        //        return hit.collider.gameObject.CompareTag("Enemy") ? hit.collider.gameObject.GetComponent<Enemy>() : null;
        //    else
        //        return null;
        //}

        // Starts listening to spell signs
        //private void StartSpellListener()
        //{
        //    currentSpellName.Clear();
        //    castingSpell = true;
        //    currentCoroutine = StartCoroutine(SpellListener());
        //}

        // Check if said spell is available
        //private Spell CheckIfSpellAvailable(string spellName)
        //{
        //    for (int i = 0; i < spells.Count; i++)
        //        if (spells[i].GetElementTypeName() == spellName)
        //        {
        //            SpellSuccessful();
        //            return spells[i];
        //        }

        //    return null;
        //}

        // Checks if spelled letter/sign is valid in the ASL Alphabet
        //private bool IsSignLetterValid(char letter)
        //{
        //    return ASLAlphabet.Contains(letter);
        //}
    }
}
