﻿using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LevelCtrl : Singleton<LevelCtrl> {

	// 장애물 프리팹
	#region Prefabs
	[Header("Enemy Objects")]
	public GameObject enemyPrefab;
	public GameObject laserPrefab;
	public GameObject heartPrefab;

	[Header("Particle Objects")]
	public GameObject particlePrefab;
	#endregion

	// 오브젝트
	#region Objects
	[Header("Publics")]
	// 메인 카메라
	public GameObject mainCamera;
	// 플레이어
	public GameObject player;
	// 일시정지 화면
	public GameObject pauseUI;
	// 캔버스
	public Transform canvas;

	public GameObject selectAudioUI;
	#endregion

	// 프리팹
	#region Prefabs
	[Header("Prefabs")]
	public GameObject levelNamePrefab;
	public GameObject levelDescPrefab;
	#endregion

	// Public
	#region Public
	[Header("Public")]
	public MoodManager moodManager;
	#endregion

	// UI
	#region UI
	[Header("UI")]
	public Text heartText;
	#endregion


	// 상수
	#region Const
	// 게임판의 고정 크기
	private const int levelSize = 512;
	// 카메라의 기본 사이즈
	private const int cameraSize = 375;
	#endregion

	// GetComp용 변수
	#region GetComponent
	// 플레이어 오브젝트의 스프라이트 렌더러를 받아오기 위한 변수
	private SpriteRenderer playerSprRen;
	// 플레이어 오브젝트의 PlayerCtrl 스크립트를 받아오기 위한 변수
	private PlayerCtrl playerCtrl;
	// 메인 카메라 오브젝트의 Camera 컴포넌트를 받아오기 위한 변수
	private Camera cameraComp;
	// 오디오 플레이어 컴포넌트를 받아오기 위한 변수
	[NonSerialized]
	public AudioSource audioSource;
	#endregion

	// 부드러운 변환용
	#region SmoothChange
	#region BackColor
	// 배경 색 변화 여부
	[NonSerialized]
	public bool bChangeEnable = false;
	// 담당 코루틴
	private IEnumerator bChangeCoroutine;
	#endregion

	#region LevelColor
	// 게임판 색 변화 여부
	[NonSerialized]
	public bool lChangeEnable = false;
	// 담당 코루틴
	private IEnumerator lChangeCoroutine;
	#endregion

	#region EnemyColor
	// 장애물 색 변화 여부
	[NonSerialized]
	public bool eChangeEnable = false;
	// 담당 코루틴
	private IEnumerator eChangeCoroutine;
	#endregion

	#region CameraRotate
	// 카메라 회전 여부
	private bool rotateEnable = false;
	// 담당 코루틴
	private IEnumerator rotateCoroutine;
	#endregion

	#region EnlargeLevel
	// 게임판 확대 여부
	private bool enlargeEnable = false;
	//담당 코루틴
	private IEnumerator enlargeCoroutine;
	#endregion

	#region MoveLevel
	// 게임판 이동 여부
	private bool moveEnable = false;
	// 담당 코루틴
	private IEnumerator moveCoroutine;
	#endregion

	#region Methods
	public float GetLerpTime(LerpType lerpType, int level, float currentTime, float time) {
		switch (lerpType) {
			case LerpType.None:
				return currentTime / time;
			case LerpType.EaseIn:
				return ExpMath.EaseIn(currentTime / time, level);
			case LerpType.EaseOut:
				return ExpMath.EaseOut(currentTime / time, level);
			case LerpType.SmoothStep:
				return ExpMath.SmoothStep(currentTime / time, level);
			case LerpType.InvSmoothStep:
				return ExpMath.InvSmoothStep(currentTime / time, level);
			case LerpType.Bounce:
				return ExpMath.Bounce(currentTime / time, level);
			case LerpType.TriWave:
				return ExpMath.TriWave(currentTime / time, level);
			case LerpType.SinWave:
				return ExpMath.SinWave(currentTime / time, level);
			case LerpType.SqrWave:
				return ExpMath.SqrWave(currentTime / time, level);
			case LerpType.SawWave:
				return ExpMath.SawWave(currentTime / time, level);
			default:
				return currentTime / time;
		}
	}
	#endregion
	#endregion

	// 플레이어 생존 여부 확인용 변수
	[NonSerialized]
	public bool playing = true;

	// 일시정지 여부 확인용 변수
	[NonSerialized]
	public bool isPaused = false;

	// 오브젝트가 생성되었을 때 실행
	private void Awake() {
		// 플레이어 오브젝트의 스프라이트 렌더러를 가져옴
		playerSprRen = player.GetComponent<SpriteRenderer>();
		// 플레이어 오브젝트의 PlayerCtrl 스크립트를 가져옴
		playerCtrl = player.GetComponent<PlayerCtrl>();
		// 메인 카메라의 Camera 컴포넌트를 가져옴
		cameraComp = mainCamera.GetComponent<Camera>();
		// 오디오 플레이어 컴포넌트를 가져옴
		audioSource = GameObject.Find("Manager").GetComponent<AudioSource>();
	}

	/// <summary>
	/// 게임을 시작한다.
	/// </summary>
	/// <param name="levelName">레벨의 이름</param>
	/// <param name="levelDesigner">레벨의 디자이너</param>
	/// <param name="width">초기 게임판 너비</param>
	/// <param name="height">초기 게임판 높이</param>
	public void StartGame(string levelName, string levelDesigner, int width, int height) {
		// 게임 이름을 지정된 값으로 설정한다.
		LevelData.Instance.levelName = levelName;
		// 게임 디자이너를 지정된 값으로 설정한다.
		LevelData.Instance.levelDesigner = levelDesigner;
		// UI Text 오브젝트를 생성한다.
		GameObject nameObj = Instantiate(levelNamePrefab, new Vector3(530, -152, 1), Quaternion.identity, canvas);
		GameObject descObj = Instantiate(levelDescPrefab, new Vector3(532, -200, 1), Quaternion.identity, canvas);
		nameObj.GetComponent<UITextCtrl>().type = UITextType.LevelName;
		descObj.GetComponent<UITextCtrl>().type = UITextType.LevelDescription;
		// 초기 너비를 지정된 값으로 설정한다.
		LevelData.Instance.levelWidth = width;
		// 초기 높이를 지정된 값으로 설정한다.
		LevelData.Instance.levelHeight = height;
		// 플레이어를 가운데에 위치시킴
		playerCtrl.playerPos = Vector2.zero;
		// 플레이어 생존 시작
		playing = true;
		// 하트 초기화
		HeartText.Instance.TextUpdate();
		heartText.enabled = true;
	}

	/// <summary>
	/// 게임 일시정지
	/// </summary>
	public void PauseResume() {
		if (playing) {
			if (!isPaused) {
				isPaused = true;
				pauseUI.SetActive(true);
				audioSource.Pause();
				Time.timeScale = 0;
			}
			else {
				isPaused = false;
				pauseUI.SetActive(false);
				audioSource.UnPause();
				Time.timeScale = 1f;
			}
		}
	}

	/// <summary>
	/// 플레이어를 초기화한다.
	/// </summary>
	public void Initialize() {
		// 게임을 초기화
		LevelData.Instance.gameOver = false;
		LevelData.Instance.backColor = Color.black;
		LevelData.Instance.levelColor = Color.white;
		LevelData.Instance.enemyColor = Color.white;
		LevelData.Instance.heartColor = Color.red;

		LevelData.Instance.invincibleMode = false;
		LevelData.Instance.playerHeart = LevelData.Instance.playerHeartMax;
		HeartText.Instance.TextUpdate();
		playerCtrl.playerPos = new Vector2(0, 0);
	}

	/// <summary>
	/// 랜덤으로 적 오브젝트를 생성한다.
	/// </summary>
	/// <param name="count"></param>
	/// <param name="speedRate"></param>
	public void DoRandom(int count, float mainEnergy, float outputEnergy, float beatEnergy, float maxMoodCount) {
		// 같은 시점에 등장하는 장애물은 같은 방향에서 나오도록
		Direction dir = Utility.GetRandomInEnum<Direction>();

		for (int i = 0; i < count; i++) {
			int randNum = (dir == Direction.Down || dir == Direction.Up) ? LevelData.Instance.levelWidth : LevelData.Instance.levelHeight;
			int pos = UnityEngine.Random.Range(-randNum, randNum + 1);

			float speed = UnityEngine.Random.Range(7f, 10f) * mainEnergy * outputEnergy * ((beatEnergy - (maxMoodCount / 2)) / (maxMoodCount / 2) + 1);
			speed = ClampRange(speed, 2f, 25f, 0.2f);

			float velo = UnityEngine.Random.Range(-0.01f * mainEnergy / 4, 0.05f * mainEnergy / 4);

			bool follow = (UnityEngine.Random.Range(0, 50) == 0);
			
			SpawnEnemy(dir, pos, speed, velo, false, follow);
		}
	}

	private float ClampRange(float value, float min, float max, float range) {
		if (value > max)
			return UnityEngine.Random.Range(max - range, max + range);
		else if (value < min)
			return UnityEngine.Random.Range(min - range, min + range);
		else
			return value;
	}

	/// <summary>
	/// Enemy 오브젝트를 지정한 위치에 생성한다.
	/// spawnPos를 100 ± n으로 설정하면 플레이어의 위치 ± n에서 생성한다.
	/// </summary>
	/// <param name="spawnDir">생성될 방향</param>
	/// <param name="spawnPos">생성될 위치</param>
	/// <param name="speed">Enemy 오브젝트의 속도</param>
	/// <param name="velo">Enemy 오브젝트의 가속도</param>
	public void SpawnEnemy(Direction spawnDir, int spawnPos, float speed, float velo, bool isHeart = false, bool follow = false) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 장애물의 EnemyCtrl 스크립트를 받아오기 위해 사용되는 변수
			EnemyCtrl enemyCtrl;
			// 하트의 HeartCtrl 스크립트를 받아오기 위해 사용되는 변수
			HeartCtrl heartCtrl;
			switch (spawnDir) {
				// 오른쪽
				case Direction.Right:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.y + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물인 경우
						if (!isHeart) {
							// 장애물을 지정된 위치에 스폰
							GameObject enemy = Instantiate(
								enemyPrefab,
								new Vector3(256, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
								Quaternion.identity);
							// 스폰된 장애물의 EnemyCtrl 스크립트를 가져옴
							enemyCtrl = enemy.GetComponent<EnemyCtrl>();
							// 방향 설정
							enemyCtrl.dir = Direction.Right;
							// 스폰 위치 설정
							enemyCtrl.spawnPos = spawnPos;
							// 속도 설정
							enemyCtrl.speed = speed;
							// 가속도 설정
							enemyCtrl.velo = velo;

							// 파티클을 지정된 위치에 스폰
							GameObject particle = Instantiate(
								particlePrefab,
								new Vector3(256, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
								Quaternion.Euler(0f, 90f, 0f));
							var mainModule = particle.GetComponent<ParticleSystem>().main;
							//mainModule.startColor = new Color(0, 0, 0, Mathf.Clamp(speed, 2, 20) / 20);
							mainModule.startColor = new Color(0, 0, 0);

						}
						// 하트인 경우
						else {
							// 하트를 지정된 위치에 스폰
							GameObject heart = Instantiate(
								heartPrefab,
								new Vector3(256, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
								Quaternion.identity);
							// 스폰된 하트의 HeartCtrl 스크립트를 가져옴
							heartCtrl = heart.GetComponent<HeartCtrl>();
							// 방향 설정
							heartCtrl.dir = Direction.Right;
							// 스폰 위치 설정
							heartCtrl.spawnPos = spawnPos;
							// 속도 설정
							heartCtrl.speed = speed;
							// 가속도 설정
							heartCtrl.velo = velo;
						}
					}
					break;
				// 아래쪽
				case Direction.Down:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.x + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물인 경우
						if (!isHeart) {
							// 장애물을 지정된 위치에 스폰
							GameObject enemy = Instantiate(
								enemyPrefab,
								new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), -256, 0),
								Quaternion.identity);
							// 스폰된 장애물의 EnemyCtrl 스크립트를 가져옴
							enemyCtrl = enemy.GetComponent<EnemyCtrl>();
							// 방향 설정
							enemyCtrl.dir = Direction.Down;
							// 스폰 위치 설정
							enemyCtrl.spawnPos = spawnPos;
							// 속도 설정
							enemyCtrl.speed = speed;
							// 가속도 설정
							enemyCtrl.velo = velo;

							// 파티클을 지정된 위치에 스폰
							GameObject particle = Instantiate(
								particlePrefab,
								new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), -256, 0),
								Quaternion.Euler(90f, 90f, 0f));
							var mainModule = particle.GetComponent<ParticleSystem>().main;
							//mainModule.startColor = new Color(0, 0, 0, Mathf.Clamp(speed, 2, 20) / 20);
							mainModule.startColor = new Color(0, 0, 0);
						}
						// 하트인 경우
						else {
							// 하트를 지정된 위치에 스폰
							GameObject heart = Instantiate(
								heartPrefab,
								new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), -256, 0),
								Quaternion.identity);
							// 스폰된 하트의 HeartCtrl 스크립트를 가져옴
							heartCtrl = heart.GetComponent<HeartCtrl>();
							// 방향 설정
							heartCtrl.dir = Direction.Down;
							// 스폰 위치 설정
							heartCtrl.spawnPos = spawnPos;
							// 속도 설정
							heartCtrl.speed = speed;
							// 가속도 설정
							heartCtrl.velo = velo;
						}
					}
					break;
				// 왼쪽
				case Direction.Left:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.y + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물인 경우
						if (!isHeart) {
							// 장애물을 지정된 위치에 스폰
							GameObject enemy = Instantiate(
								enemyPrefab,
								new Vector3(-256, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
								Quaternion.identity);
							// 스폰된 장애물의 EnemyCtrl 스크립트를 가져옴
							enemyCtrl = enemy.GetComponent<EnemyCtrl>();
							// 방향 설정
							enemyCtrl.dir = Direction.Left;
							// 스폰 위치 설정
							enemyCtrl.spawnPos = spawnPos;
							// 속도 설정
							enemyCtrl.speed = speed;
							// 가속도 설정
							enemyCtrl.velo = velo;

							// 파티클을 지정된 위치에 스폰
							GameObject particle = Instantiate(
								particlePrefab,
								new Vector3(-256, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
								Quaternion.Euler(180f, 90f, 0f));
							var mainModule = particle.GetComponent<ParticleSystem>().main;
							//mainModule.startColor = new Color(0, 0, 0, Mathf.Clamp(speed, 2, 20) / 20);
							mainModule.startColor = new Color(0, 0, 0);
						}
						// 하트인 경우
						else {
							// 하트를 지정된 위치에 스폰
							GameObject heart = Instantiate(
								heartPrefab,
								new Vector3(-256, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
								Quaternion.identity);
							// 스폰된 하트의 HeartCtrl 스크립트를 가져옴
							heartCtrl = heart.GetComponent<HeartCtrl>();
							// 방향 설정
							heartCtrl.dir = Direction.Left;
							// 스폰 위치 설정
							heartCtrl.spawnPos = spawnPos;
							// 속도 설정
							heartCtrl.speed = speed;
							// 가속도 설정
							heartCtrl.velo = velo;
						}
					}
					break;
				// 위쪽
				case Direction.Up:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.x + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물인 경우
						if (!isHeart) {
							// 장애물을 지정된 위치에 스폰
							GameObject enemy = Instantiate(
								enemyPrefab,
								new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), 256, 0),
								Quaternion.identity);
							// 스폰된 장애물의 EnemyCtrl 스크립트를 가져옴
							enemyCtrl = enemy.GetComponent<EnemyCtrl>();
							// 방향 설정
							enemyCtrl.dir = Direction.Up;
							// 스폰 위치 설정
							enemyCtrl.spawnPos = spawnPos;
							// 속도 설정
							enemyCtrl.speed = speed;
							// 가속도 설정
							enemyCtrl.velo = velo;

							// 파티클을 지정된 위치에 스폰
							GameObject particle = Instantiate(
								particlePrefab,
								new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), 256, -2),
								Quaternion.Euler(270f, 90f, 0f));
							var mainModule = particle.GetComponent<ParticleSystem>().main;
							//mainModule.startColor = new Color(0, 0, 0, Mathf.Clamp(speed, 2, 20) / 20);
							mainModule.startColor = new Color(0, 0, 0);
						}
						// 하트인 경우
						else {
							// 하트를 지정된 위치에 스폰
							GameObject heart = Instantiate(
								heartPrefab,
								new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), 256, 0),
								Quaternion.identity);
							// 스폰된 하트의 HeartCtrl 스크립트를 가져옴
							heartCtrl = heart.GetComponent<HeartCtrl>();
							// 방향 설정
							heartCtrl.dir = Direction.Up;
							// 스폰 위치 설정
							heartCtrl.spawnPos = spawnPos;
							// 속도 설정
							heartCtrl.speed = speed;
							// 가속도 설정
							heartCtrl.velo = velo;
						}
					}
					break;
			}
		}
	}

	/// <summary>
	/// Laser 오브젝트를 지정한 위치에 생성한다.
	/// spawnPos를 100 ± n으로 설정하면 플레이어의 위치 ± n에서 생성한다.
	/// </summary>
	/// <param name="spawnDir">생성될 방향</param>
	/// <param name="spawnPos">생성될 위치</param>
	/// <param name="time">유지 시간</param>
	public void SpawnLaser(Direction spawnDir, int spawnPos, float time, bool follow = false) {
		// 생존 중일때만 작동한다.
		if (playing) {
			switch (spawnDir) {
				// 오른쪽
				case Direction.Right:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.y + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물을 지정된 위치에 스폰
						GameObject enemy = Instantiate(
							laserPrefab,
							new Vector3(0, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
							Quaternion.identity);
						// 스폰된 장애물의 LaserCtrl 스크립트를 가져옴
						LaserCtrl laserCtrl = enemy.GetComponent<LaserCtrl>();
						// 방향 설정
						laserCtrl.dir = Direction.Right;
						// 스폰 위치 설정
						laserCtrl.spawnPos = spawnPos;
						// 유지 시간 설정
						laserCtrl.time = time;
					}
					break;
				// 아래쪽
				case Direction.Down:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.x + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물을 지정된 위치에 스폰
						GameObject enemy = Instantiate(
							laserPrefab, new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), 0, 0),
							Quaternion.identity);
						// 스폰된 장애물의 LaserCtrl 스크립트를 가져옴
						LaserCtrl laserCtrl = enemy.GetComponent<LaserCtrl>();
						// 방향 설정
						laserCtrl.dir = Direction.Down;
						// 스폰 위치 설정
						laserCtrl.spawnPos = spawnPos;
						// 유지 시간 설정
						laserCtrl.time = time;
					}
					break;
				// 왼쪽
				case Direction.Left:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.y + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물을 지정된 위치에 스폰
						GameObject enemy = Instantiate(
							laserPrefab,
							new Vector3(0, spawnPos * (levelSize / ((LevelData.Instance.levelHeight * 2) + 1)), 0),
							Quaternion.identity);
						// 스폰된 장애물의 LaserCtrl 스크립트를 가져옴
						LaserCtrl laserCtrl = enemy.GetComponent<LaserCtrl>();
						// 방향 설정
						laserCtrl.dir = Direction.Left;
						// 스폰 위치 설정
						laserCtrl.spawnPos = spawnPos;
						// 유지 시간 설정
						laserCtrl.time = time;
					}
					break;
				// 위쪽
				case Direction.Up:
					// 플레이어와의 상대적 위치에서 소환될 경우
					if (follow)
						// 플레이어의 위치에 맞춰 스폰 위치 설정
						spawnPos = (int)playerCtrl.playerPos.x + spawnPos;

					// 스폰 위치가 올바른 값일 때
					if (spawnPos <= LevelData.Instance.levelWidth && spawnPos >= -LevelData.Instance.levelWidth) {
						// 장애물을 지정된 위치에 스폰
						GameObject enemy = Instantiate(
							laserPrefab,
							new Vector3(spawnPos * (levelSize / ((LevelData.Instance.levelWidth * 2) + 1)), 0, 0),
							Quaternion.identity);
						// 스폰된 장애물의 LaserCtrl 스크립트를 가져옴
						LaserCtrl laserCtrl = enemy.GetComponent<LaserCtrl>();
						// 방향 설정
						laserCtrl.dir = Direction.Up;
						// 스폰 위치 설정
						laserCtrl.spawnPos = spawnPos;
						// 유지 시간 설정
						laserCtrl.time = time;
					}
					break;
			}
		}
	}

	/// <summary>
	/// 레벨의 칸 수를 조정한다.
	/// </summary>
	/// <param name="width">중심으로부터의 가로 칸 수</param>
	/// <param name="height">중심으로부터의 세로 칸 수</param>
	public void ResizeLevel(int width, int height) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 게임판의 너비를 지정된 값으로 바꾼다.
			LevelData.Instance.levelWidth = width;
			// 게임판의 높이를 지정된 값으로 바꾼다.
			LevelData.Instance.levelHeight = height;

			// Enemy 태그를 가진 모든 오브젝트를 배열로 저장한다.
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			for (int i = 0; i < enemies.Length; i++) {
				GameObject enemyObj = enemies[i];
				// 모든 장애물의 위치를 게임판 크기에 맞게 수정한다.
				EnemyCtrl enemyCtrl = enemyObj.GetComponent<EnemyCtrl>();
				// TODO: 좌표 줄이기 수정
				switch (enemyCtrl.dir) {
					// 오른쪽
					case Direction.Right:
						enemyObj.transform.position = new Vector3(enemyObj.transform.position.x, enemyCtrl.spawnPos * (levelSize / ((height * 2) + 1)), 0);
						break;
					// 아래쪽
					case Direction.Down:
						enemyObj.transform.position = new Vector3(enemyCtrl.spawnPos * (levelSize / ((width * 2) + 1)), enemyObj.transform.position.y, 0);
						break;
					// 왼쪽
					case Direction.Left:
						enemyObj.transform.position = new Vector3(enemyObj.transform.position.x, enemyCtrl.spawnPos * (levelSize / ((height * 2) + 1)), 0);
						break;
					// 위쪽
					case Direction.Up:
						enemyObj.transform.position = new Vector3(enemyCtrl.spawnPos * (levelSize / ((width * 2) + 1)), enemyObj.transform.position.y, 0);
						break;
				}
			}
			// TODO: 레이저 & 하트도 적용
		}
	}

	/// <summary>
	/// 배경색을 지정된 시간동안 서서히 바꾼다.
	/// </summary>
	/// <param name="color">바꿀 색상</param>
	/// <param name="time">바꿀 시간</param>
	public void ChangeBackColor(Color color, float time, LerpType lerpType, int level = 1) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 변화 중이라면
			if (bChangeEnable) {
				// 변화를 중지하고 코루틴을 멈춘다
				bChangeEnable = false;
				StopCoroutine(bChangeCoroutine);
			}
			bChangeCoroutine = BackChangeCoroutine(color, time, lerpType, level);
			StartCoroutine(bChangeCoroutine);
		}
	}

	// 배경색 변화 코루틴
	private IEnumerator BackChangeCoroutine(Color changeColor, float time, LerpType lerpType, int level) {
		bChangeEnable = true;
		Color originalColor = LevelData.Instance.backColor;
		float tempTime = 0;

		while (true) {
			if (playing) {
				// 변화 시간이 다 지나지 않았을 경우
				if (tempTime < time) {
					// 배경색을 본래 색에서 바꿀 색으로 지정된 시간동안 변화시킴
					LevelData.Instance.backColor = Color.Lerp(
						originalColor,
						changeColor,
						GetLerpTime(lerpType, level, tempTime, time));
					// 시간 카운트
					tempTime += Time.deltaTime;
				}
				// 변화 시간이 다 지났을 경우
				else {
					// 배경색을 지정된 색으로 변경
					LevelData.Instance.backColor = changeColor;
					// 변화 활성화를 해제
					bChangeEnable = false;
					// 코루틴 중지
					StopCoroutine(bChangeCoroutine);
				}
			}
			else {
				// 변화 활성화를 해제
				bChangeEnable = false;
				// 코루틴 중지
				StopCoroutine(bChangeCoroutine);
			}
			yield return null;
		}
	}

	/// <summary>
	/// 벽의 색을 지정된 시간동안 서서히 바꾼다.
	/// </summary>
	/// <param name="color">바꿀 색상</param>
	/// <param name="time">바꿀 시간</param>
	public void ChangeLevelColor(Color color, float time, LerpType lerpType, int level = 1) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 변화 중이라면
			if (lChangeEnable) {
				// 변화를 중지하고 코루틴을 멈춘다
				lChangeEnable = false;
				StopCoroutine(lChangeCoroutine);
			}
			lChangeCoroutine = LevelChangeCoroutine(color, time, lerpType, level);
			StartCoroutine(lChangeCoroutine);
		}
	}

	// 외벽 색 변화 코루틴
	private IEnumerator LevelChangeCoroutine(Color changeColor, float time, LerpType lerpType, int level) {
		lChangeEnable = true;
		Color originalColor = LevelData.Instance.levelColor;
		float tempTime = 0;

		while (true) {
			if (playing) {
				// 변화 시간이 다 지나지 않았을 경우
				if (tempTime < time) {
					// 배경색을 본래 색에서 바꿀 색으로 지정된 시간동안 변화시킴
					LevelData.Instance.levelColor = Color.Lerp(
						originalColor,
						changeColor,
						GetLerpTime(lerpType, level, tempTime, time));
					// 시간 카운트
					tempTime += Time.deltaTime;
				}
				// 변화 시간이 다 지났을 경우
				else {
					// 배경색을 지정된 색으로 변경
					LevelData.Instance.levelColor = changeColor;
					// 변화 활성화를 해제
					lChangeEnable = false;
					// 코루틴 중지
					StopCoroutine(lChangeCoroutine);
				}
			}
			else {
				// 변화 활성화를 해제
				lChangeEnable = false;
				// 코루틴 중지
				StopCoroutine(lChangeCoroutine);
			}
			yield return null;
		}
	}

	/// <summary>
	/// Enemy의 색을 지정된 시간동안 서서히 바꾼다.
	/// </summary>
	/// <param name="color">바꿀 색상</param>
	/// <param name="time">바꿀 시간</param>
	public void ChangeEnemyColor(Color color, float time, LerpType lerpType, int level = 1) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 변화 중이라면
			if (eChangeEnable) {
				// 변화를 중지하고 코루틴을 멈춘다
				eChangeEnable = false;
				StopCoroutine(eChangeCoroutine);
			}
			eChangeCoroutine = EnemyChangeCoroutine(color, time, lerpType, level);
			StartCoroutine(eChangeCoroutine);
		}
	}

	// 장애물 색 변화 코루틴
	private IEnumerator EnemyChangeCoroutine(Color changeColor, float time, LerpType lerpType, int level) {
		eChangeEnable = true;
		Color originalColor = LevelData.Instance.enemyColor;
		float tempTime = 0;

		while (true) {
			if (playing) {
				// 변화 시간이 다 지나지 않았을 경우
				if (tempTime < time) {
					// 배경색을 본래 색에서 바꿀 색으로 지정된 시간동안 변화시킴
					LevelData.Instance.enemyColor = Color.Lerp(
						originalColor,
						changeColor,
						GetLerpTime(lerpType, level, tempTime, time));
					// 시간 카운트
					tempTime += Time.deltaTime;
				}
				// 변화 시간이 다 지났을 경우
				else {
					// 배경색을 지정된 색으로 변경
					LevelData.Instance.enemyColor = changeColor;
					// 변화 활성화를 해제
					eChangeEnable = false;
					// 코루틴 중지
					StopCoroutine(eChangeCoroutine);
				}
			}
			else {
				// 변화 활성화를 해제
				eChangeEnable = false;
				// 코루틴 중지
				StopCoroutine(eChangeCoroutine);
			}
			yield return null;
		}
	}

	/// <summary>
	/// 게임 플레이 화면을 일정 각도 회전시킨다.
	/// </summary>
	/// <param name="angle">회전시킬 각도</param>
	/// <param name="time">회전시킬 시간</param>
	public void RotateLevel(float angle, float time, LerpType lerpType, int level = 1) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 변화 중이라면
			if (rotateEnable) {
				// 변화를 중지하고 코루틴을 멈춘다
				rotateEnable = false;
				StopCoroutine(rotateCoroutine);
			}
			rotateCoroutine = RotateCoroutine(angle, time, lerpType, level);
			StartCoroutine(rotateCoroutine);
		}
	}

	// 카메라 회전 코루틴
	private IEnumerator RotateCoroutine(float rotateAngle, float time, LerpType lerpType, int level) {
		rotateEnable = true;
		float originalAngle = mainCamera.transform.localRotation.z;
		float tempTime = 0;

		while (true) {
			if (playing) {
				// 변화 시간이 다 지나지 않았을 경우
				if (tempTime < time) {
					// 카메라의 각도를 본래 각도에서 바꿀 각도로 지정된 시간동안 변화시킴
					mainCamera.transform.localRotation = Quaternion.Lerp(
						new Quaternion(mainCamera.transform.localRotation.x, mainCamera.transform.localRotation.y, originalAngle, mainCamera.transform.localRotation.w),
						new Quaternion(mainCamera.transform.localRotation.x, mainCamera.transform.localRotation.y, rotateAngle, mainCamera.transform.localRotation.w),
						GetLerpTime(lerpType, level, tempTime, time));
					// 시간 카운트
					tempTime += Time.deltaTime;
				}
				// 변화 시간이 다 지났을 경우
				else {
					// 카메라 각도를 지정된 각도로 변경
					mainCamera.transform.localRotation = new Quaternion(
						mainCamera.transform.localRotation.x,
						mainCamera.transform.localRotation.y,
						rotateAngle,
						mainCamera.transform.localRotation.w);
					// 변화 활성화를 해제
					rotateEnable = false;
					// 코루틴 중지
					StopCoroutine(rotateCoroutine);
				}
			}
			else {
				// 변화 활성화를 해제
				rotateEnable = false;
				// 코루틴 중지
				StopCoroutine(rotateCoroutine);
			}
			yield return null;
		}
	}

	/// <summary>
	/// 게임 플레이 화면을 일정 비율 확대시킨다.
	/// 숫자가 작을수록 화면이 커진다.
	/// </summary>
	/// <param name="rate">확대시킬 비율</param>
	/// <param name="time">확대시킬 시간</param>
	public void EnlargeLevel(float rate, float time, LerpType lerpType, int level = 1) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 변화 중이라면
			if (enlargeEnable) {
				// 변화를 중지하고 코루틴을 멈춘다
				enlargeEnable = false;
				StopCoroutine(enlargeCoroutine);
			}
			enlargeCoroutine = EnlargeCoroutine(rate, time, lerpType, level);
			StartCoroutine(enlargeCoroutine);
		}
	}

	// 확대 코루틴
	private IEnumerator EnlargeCoroutine(float enlargeRate, float time, LerpType lerpType, int level) {
		enlargeEnable = true;
		float originalSize = cameraComp.orthographicSize;
		float tempTime = 0;

		while (true) {
			if (playing) {
				// 변화 시간이 다 지나지 않았을 경우
				if (tempTime < time) {
					// 카메라의 시야 크기를 본래 크기에서 바꿀 크기로 지정된 사간동안 변화시킴
					cameraComp.orthographicSize = Mathf.Lerp(
						originalSize,
						enlargeRate * cameraSize,
						GetLerpTime(lerpType, level, tempTime, time));
					// 시간 카운트
					tempTime += Time.deltaTime;
				}
				// 변화 시간이 다 지났을 경우
				else {
					// 카메라 시야 크기를 지정된 크기로 변경
					cameraComp.orthographicSize = enlargeRate * cameraSize;
					// 변화 활성화를 해제
					enlargeEnable = false;
					// 코루틴 중지
					StopCoroutine(enlargeCoroutine);
				}
			}
			else {
				// 변화 활성화를 해제
				enlargeEnable = false;
				// 코루틴 중지
				StopCoroutine(enlargeCoroutine);
			}
			yield return null;
		}
	}

	/// <summary>
	/// 게임 플레이 화면을 이동시킨다.
	/// </summary>
	/// <param name="x">이동시킬 x좌표</param>
	/// <param name="y">이동시킬 y좌표</param>
	/// <param name="time">이동시킬 시간</param>
	public void MoveLevel(float x, float y, float time, LerpType lerpType, int level = 1) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 변화 중이라면
			if (moveEnable) {
				// 변화를 중지하고 코루틴을 멈춘다
				moveEnable = false;
				StopCoroutine(moveCoroutine);
			}
			moveCoroutine = MoveCoroutine(x, y, time, lerpType, level);
			StartCoroutine(moveCoroutine);
		}
	}

	// 이동 코루틴
	private IEnumerator MoveCoroutine(float x, float y, float time, LerpType lerpType, int level) {
		moveEnable = true;
		Vector3 originalVector = mainCamera.transform.position;
		float tempTime = 0;

		Vector3 moveVector = new Vector3(-x, -y, -10);

		while (true) {
			if (playing) {
				// 변화 시간이 다 지나지 않았을 경우
				if (tempTime < time) {
					// 카메라의 위치를 본래 위치에서 바꿀 위치로 지정된 사간동안 변화시킴
					mainCamera.transform.position = Vector3.Lerp(
						originalVector,
						moveVector,
						GetLerpTime(lerpType, level, tempTime, time));
					// 시간 카운트
					tempTime += Time.deltaTime;
				}
				// 변화 시간이 다 지났을 경우
				else {
					// 카메라 위치를 지정된 위치로 변경
					mainCamera.transform.position = moveVector;
					// 변화 활성화를 해제
					moveEnable = false;
					// 코루틴 중지
					StopCoroutine(moveCoroutine);
				}
			}
			else {
				// 변화 활성화를 해제
				moveEnable = false;
				// 코루틴 중지
				StopCoroutine(moveCoroutine);
			}
			yield return null;
		}
	}

	/// <summary>
	/// 플레이어를 투명화시킨다.
	/// </summary>
	public void PlayerVisible(bool visible) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 플레이어의 투명 여부를 설정한다.
			playerSprRen.enabled = visible;
		}
	}

	/// <summary>
	/// 플레이어를 지정한 위치로 강제 이동시킨다.
	/// </summary>
	/// <param name="pos">이동시킬 위치</param>
	public void ReplacePlayer(Vector2 pos) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 플레이어의 위치를 지정된 좌표로 설정한다.
			playerCtrl.playerPos = pos;
		}
	}

	/// <summary>
	/// 플레이어를 지정한 위치로 강제 이동시킨다.
	/// </summary>
	/// <param name="x">x 좌표</param>
	/// <param name="y">y 좌표</param>
	public void ReplacePlayer(int x, int y) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 플레이어의 위치를 지정된 좌표로 설정한다.
			playerCtrl.playerPos = new Vector2(x, y);
		}
	}

	/// <summary>
	/// 현재 존재하는 모든 엔티티를 제거한다.
	/// </summary>
	public void KillAll() {
		// 모든 장애물을 제거한다.
		GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemyObjects.Length; i++) {
			Destroy(enemyObjects[i]);
		}
		GameObject[] laserObjects = GameObject.FindGameObjectsWithTag("Laser");
		for (int i = 0; i < laserObjects.Length; i++) {
			Destroy(laserObjects[i]);
		}
		GameObject[] heartObjects = GameObject.FindGameObjectsWithTag("Heart");
		for (int i = 0; i < heartObjects.Length; i++) {
			Destroy(heartObjects[i]);
		}
	}

	/// <summary>
	/// 현재 존재하는 특정 엔티티를 전부 제거한다.
	/// </summary>
	/// <param name="tag">제거할 엔티티 태그</param>
	public void KillEntity(string entityTag) {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 특정 태그를 가진 장애물을 제거한다.
			GameObject[] objects = GameObject.FindGameObjectsWithTag(entityTag);
			for (int i = 0; i < objects.Length; i++) {
				Destroy(objects[i]);
			}
		}
	}

	/// <summary>
	/// 플레이어의 하트가 전부 소진되었을 때 발동시킨다.
	/// </summary>
	public void GameOver() {
		// 생존 중일때만 작동한다.
		if (playing) {
			// 배경음악을 멈춘다.
			audioSource.Stop();
			// 생존 여부를 거짓으로 설정한다.
			playing = false;
			// 모든 장애물을 제거한다.
			KillAll();
			// 게임 오버 여부를 참으로 설정한다.
			LevelData.Instance.gameOver = true;
			// 모든 색을 검은색으로 설정한다.
			LevelData.Instance.backColor = Color.black;
			LevelData.Instance.levelColor = Color.black;
			LevelData.Instance.enemyColor = Color.black;
			LevelData.Instance.heartColor = Color.black;
			// 게임 오버 코루틴을 실행한다.
			StartCoroutine(GameOverCoroutine());
		}
	}

	/// <summary>
	/// 게임 오버되었을 때 실행되는 코루틴이다.
	/// </summary>
	/// <returns></returns>
	private IEnumerator GameOverCoroutine() {
		// 색을 변경하고 3초를 기다린다.
		bChangeEnable = true;
		lChangeEnable = true;
		eChangeEnable = true;

		yield return new WaitForSeconds(0.01f);

		bChangeEnable = false;
		lChangeEnable = false;
		eChangeEnable = false;
		//SceneManager.LoadScene("LevelSelect");

		yield return new WaitForSeconds(3f);

		Initialize();
		
		GameEnd();
	}

	public void GameEnd() {
		/*
		if (File.Exists(moodManager.tempPath)) {
			try {
				File.Delete(moodManager.tempPath);
			}
			catch {

			}
		}
		*/
		LevelData.Instance.playerHeart = LevelData.Instance.playerHeartMax;
		heartText.enabled = false;

		GameObject[] levelTexts = GameObject.FindGameObjectsWithTag("LevelText");
		foreach(GameObject text in levelTexts) {
			Destroy(text);
		}

		audioSource.Stop();
		selectAudioUI.SetActive(true);
	}
}
