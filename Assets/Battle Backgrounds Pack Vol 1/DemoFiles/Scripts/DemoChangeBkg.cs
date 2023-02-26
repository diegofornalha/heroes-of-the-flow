using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoChangeBkg : MonoBehaviour{
	[SerializeField]
	private Sprite[] backgrounds;
	
	[SerializeField]
	private SpriteRenderer bkgSprite;
	
	
	private int currentBack = 0;
	
	
	void Start(){
		bkgSprite.sprite = backgrounds[0];
	}
	
	
	
	public void NextBack(){
		if(currentBack< backgrounds.Length-1){
			currentBack++;
			bkgSprite.sprite = backgrounds[currentBack];
		}
	}
	public void PrevBack(){
		if(currentBack>0){
			currentBack--;
			bkgSprite.sprite = backgrounds[currentBack];
		}
	}
	
}
