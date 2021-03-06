﻿using UnityEngine;

public class EnemyCtrl : MonoBehaviour {

	// 게임판의 고정 크기
	private const int levelSize = 512;
	// 스프라이트의 기본 크기
	private const float spriteMaxScale = 500f;

	// 방향 -> 0 : 오른쪽, 1 : 아래쪽, 2 : 왼쪽, 3 : 위쪽
	public Direction dir = Direction.Right;
	// 시작 점
	public int spawnPos = 0;
	// 속도
	public float speed = 10f;
	// 가속도
	public float velo = 0f;

	private float time = 0;

	// 오브젝트의 스프라이트 렌더러
	private SpriteRenderer sprRen;

	// 오브젝트가 생성될 때 실행
	private void Awake() {
		// 스프라이트 렌더러를 가져옴
		sprRen = gameObject.GetComponent<SpriteRenderer>();
		// 색 초기화
		sprRen.color = LevelData.Instance.enemyColor;
	}

	// 매 프레임마다 실행
	private void Update() {
		if (!LevelCtrl.Instance.isPaused) {
			ReplaceEnemy();
			ColorUpdate();
			KillEnemy();
		}
	}

	/*
	// 게임 화면을 벗어났을 때
	private void OnBecameInvisible() {
		Destroy(gameObject);
	}
	*/

	// 충돌이 발생했을 때
	private void OnTriggerEnter2D(Collider2D collision) {
		// 충돌한 대상이 무적이 아닌 플레이어일 때
		if (collision.CompareTag("Player") && !LevelData.Instance.isInvincible && !LevelData.Instance.invincibleMode) {
			// 플레이어의 생명을 1 감소시킴
			LevelData.Instance.playerHeart -= 10;
			HeartText.Instance.TextUpdate();
			// 무적 상태로 전환
			LevelData.Instance.isInvincible = true;
			// 생명이 없으면
			if (LevelData.Instance.playerHeart <= 0) {
				// 게임 오버
				LevelCtrl.Instance.GameOver();
			}
			// 해당 오브젝트 파괴
			Destroy(gameObject);
		}
	}

	private void KillEnemy() {
		switch (dir) {
			case Direction.Right:
			case Direction.Left:
				if (transform.position.x > 300 || transform.position.x < -300)
					Destroy(gameObject);
				return;
			case Direction.Down:
			case Direction.Up:
				if (transform.position.y > 300 || transform.position.y < -300)
					Destroy(gameObject);
				return;
		}
	}

	// 속도에 따른 움직임 / 게임판 사이즈에 따른 크기 설정 - 매 프레임마다 실행
	private void ReplaceEnemy() {
		// 크기 설정
		gameObject.transform.localScale = new Vector3(spriteMaxScale / ((LevelData.Instance.levelWidth * 2) + 1), spriteMaxScale / ((LevelData.Instance.levelHeight * 2) + 1), 1);

		// 움직임 설정
		switch (dir) {
			case Direction.Right:
				gameObject.transform.position += new Vector3(-speed, 0, 0);
				break;
			case Direction.Down:
				gameObject.transform.position += new Vector3(0, speed, 0);
				break;
			case Direction.Left:
				gameObject.transform.position += new Vector3(speed, 0, 0);
				break;
			case Direction.Up:
				gameObject.transform.position += new Vector3(0, -speed, 0);
				break;
		}
		speed += velo;
	}

	// 색 변화 - 매 프레임마다 실행
	private void ColorUpdate() {
		if (LevelCtrl.Instance.eChangeEnable)
			sprRen.color = LevelData.Instance.enemyColor;
	}
}
