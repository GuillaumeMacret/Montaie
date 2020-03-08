using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGOTrigger : MonoBehaviour
{
    // Public vars
    [Header("[Source]")]
    public TownManager ManagerScript;
    public GameObject GoTarget;

    [Header("[AnimGO Settings]")]
    public bool Locked = false;
    public bool AutoAnim = false;

    // Private vars
    [HideInInspector]
    public KeyCode UnlockKey;
    private Collider _activator;
    private bool _iscolliding = false;
    [HideInInspector]
    public Animator animator;
	private List<OcclusionPortal> occlusionPortals;

    // Use this for initialization
    void Start()
    {
        // make this object invisible
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        // set animator
        animator = GoTarget.GetComponent<Animator>();
		// set default anim values
		//animator.SetBool("Opened", true);
		occlusionPortals = new List<OcclusionPortal>();
		GetComponentsInChildren(occlusionPortals);
		foreach(OcclusionPortal occlusionPortal in occlusionPortals)
			occlusionPortal.open = false;
    }

    // When any collider hits the trigger.
    void OnTriggerEnter(Collider trig)
    {
        // get player collider
        _activator = ManagerScript.PlayerCollider;
        // check if Key pressed and collider hit was from correct target
        if (trig.GetComponent<Collider>() == _activator)
        {
            // set the door ready to move
            _iscolliding = true;
			Debug.Log(trig.name + "has entered the activator trigger");
			// AUTOANIM
			if (AutoAnim && (!Locked))
			{
				animator.SetBool("Opened", false);
				animator.SetTrigger("Actived");
			}
			foreach (OcclusionPortal occlusionPortal in occlusionPortals) {
				occlusionPortal.open = true;
				Debug.Log(occlusionPortal.name);
			}
		}
    }
    void OnTriggerExit(Collider trig)
    {
		_activator = ManagerScript.PlayerCollider;

		if(trig.GetComponent<Collider>() == _activator) {
			// set the door out of reach
			_iscolliding = false;
			//debug
			Debug.Log(trig.name + "has exit the activator trigger");

			//AUTOANIM
			if (AutoAnim && (!Locked))
			{
				animator.SetTrigger("Actived");
				animator.SetBool("Opened", true);
			}
			foreach (OcclusionPortal occlusionPortal in occlusionPortals)
				occlusionPortal.open = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        // check if ready to move
        if (_iscolliding)
        {
            // get unlock key code
            UnlockKey = ManagerScript.DoorKeyCode;
            // Set movement on when Key pressed
            if ((Input.GetKeyDown(ManagerScript.DoorKeyCode) && (!Locked) && (!AutoAnim)))
            {
                animator.SetTrigger("Actived");
                if (animator.GetBool("Opened") == false)
                {
                    animator.SetBool("Opened", true);
				}
                else
                {
                    animator.SetBool("Opened", false);
				}
            }
        }
    }
}
