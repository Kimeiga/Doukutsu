using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanaAnimator : MonoBehaviour {

	
	public SpriteAnimator spriteAnimator;
	public PlatformerController platformerController;
	public Rigidbody2D rigidbody2D;
	
	// Use this for initialization
	void Start ()
	{

		spriteAnimator = GetComponent<SpriteAnimator>();
		platformerController = transform.root.GetComponent<PlatformerController>();
		rigidbody2D = transform.root.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if she's ducking and not moving left or right or not on the ground, play duck
		if (platformerController.duck && (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 1f || !platformerController.grounded))
		{
			spriteAnimator.Play("Duck");
		}
		//if she's ducking and moving left or right and on the ground, play slide
		else if (platformerController.duck && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f && platformerController.grounded)
		{
			spriteAnimator.Play("Slide");
		}
		//if she's not grounded and moving up, play jump
		else if (!platformerController.grounded && rigidbody2D.velocity.y > 0)
		{
			spriteAnimator.Play("Jump");
		}
		//if she's not grounded and moving down, play fall
		else if (!platformerController.grounded && rigidbody2D.velocity.y < 0)
		{
			spriteAnimator.Play("Fall");
		}
		//if she's grounded and moving left or right, play Walk
		else if (platformerController.grounded && Mathf.Abs(rigidbody2D.velocity.x) > 0.01f)
		{
			spriteAnimator.Play("Walk");
		}
		//if none of these are the case, play idle
		else
		{
			spriteAnimator.Play("Idle");
		}

	}
}
