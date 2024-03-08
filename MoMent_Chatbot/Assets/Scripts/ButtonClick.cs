using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

	public Animator anim;
	public Animator anim2;
	public Animator anim3;
	public Animator anim4;


	public void Run () {
		anim.Play ("Run", -1, 3f);
		anim2.Play ("Run", -1, 3f);
		anim3.Play ("Run", -1, 3f);
		anim4.Play ("Run", -1, 3f);
	
	}

	public void Idle () {
		anim.Play ("Idle", -1, 0f);
		anim2.Play ("Idle", -1, 0f);
		anim3.Play ("Idle", -1, 0f);
		anim4.Play ("Idle", -1, 0f);
	}

	public void Sit () {
		anim.Play ("Sit", -1, 3f);
		anim2.Play ("Sit", -1, 3f);
		anim3.Play ("Sit", -1, 3f);
		anim4.Play ("Sit", -1, 3f);

	}

	public void Walk () {
		anim.Play ("Walk", -1, 3f);
		anim2.Play ("Walk", -1, 3f);
		anim3.Play ("Walk", -1, 3f);
		anim4.Play ("Walk", -1, 3f);

	}

	public void Eat () {
		anim.Play ("Eat", -1, 3f);
		anim2.Play ("Eat", -1, 3f);
		anim3.Play ("Eat", -1, 3f);
		anim4.Play ("Eat", -1, 3f);

	}

	public void Sleep () {
		anim.Play ("Sleep", -1, 3f);
		anim2.Play ("Sleep", -1, 3f);
		anim3.Play ("Sleep", -1, 3f);
		anim4.Play ("Sleep", -1, 3f);

	}

	public void Barking () {
		anim.Play ("Barking", -1, 3f);
		anim2.Play ("Barking", -1, 3f);
		anim3.Play ("Barking", -1, 3f);
		anim4.Play ("Barking", -1, 3f);

	}

	public void Play () {
		anim.Play ("Play", -1, 3f);
		anim2.Play ("Play", -1, 3f);
		anim3.Play ("Play", -1, 3f);
		anim4.Play ("Play", -1, 3f);

	}

	public void Jump () {
		anim.Play ("Jump", -1, 3f);
		anim2.Play ("Jump", -1, 3f);
		anim3.Play ("Jump", -1, 3f);
		anim4.Play ("Jump", -1, 3f);

	}
}
