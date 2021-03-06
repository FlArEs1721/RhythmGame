﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSettingData : MonoBehaviour {

	[NonSerialized]
	public GameObject preCommandObj = null;
	[NonSerialized]
	public GameObject commandObj;

	public Copy copyData;

	public GameObject[] tabs = new GameObject[10];

	public Dropdown typeDropDown;
	public Image typeColorImage;
	public InputField activeTimeInput;

	public Dropdown dirDropDown1;
	public InputField spawnPosInput1;
	public InputField speedInput;
	public InputField veloInput;
	public Toggle isHeartToggle;
	public Toggle followToggle1;

	public Dropdown dirDropDown2;
	public InputField spawnPosInput2;
	public InputField timeInput1;
	public Toggle followToggle2;

	public Dropdown colorTypeDropDown;
	public InputField rInput;
	public InputField gInput;
	public InputField bInput;
	public InputField timeInput2;
	public Dropdown modeDropDown1;
	public InputField levelInput1;

	public InputField widthInput;
	public InputField heightInput;

	public InputField angleInput;
	public InputField timeInput3;
	public Dropdown modeDropDown2;
	public InputField levelInput2;

	public InputField rateInput;
	public InputField timeInput4;
	public Dropdown modeDropDown3;
	public InputField levelInput3;

	public InputField xInput;
	public InputField yInput;
	public InputField timeInput5;
	public Dropdown modeDropDown4;
	public InputField levelInput4;

	public InputField playerXInput;
	public InputField playerYInput;

	public Toggle visibleToggle;

	public Dropdown tagDropDown;

	public Text tipText;
	public List<string> tips = new List<string>();

	private CommandData commandData;

	private void ReloadTabs(int tab) {
		for (int i = 0; i < tabs.Length; i++) {
			if (tabs[i] == null) continue;

			if (i == tab) {
				tabs[i].SetActive(true);
			}
			else {
				tabs[i].SetActive(false);
			}
		}
	}

	public void Load() {
		commandData = commandObj.GetComponent<CommandData>();
		activeTimeInput.text = commandData.activeTime.ToString();
		switch (commandData.type) {
			case CommandType.SpawnEnemy:
				#region SpawnEnemy
				typeDropDown.value = 0;
				ReloadTabs(0);
				switch (commandData.spawnDir) {
					case Direction.Right:
						dirDropDown1.value = 0;
						break;
					case Direction.Down:
						dirDropDown1.value = 1;
						break;
					case Direction.Left:
						dirDropDown1.value = 2;
						break;
					case Direction.Up:
						dirDropDown1.value = 3;
						break;
				}
				spawnPosInput1.text = commandData.spawnPos.ToString();
				speedInput.text = commandData.speed.ToString();
				veloInput.text = commandData.velo.ToString();
				isHeartToggle.isOn = commandData.isHeart;
				followToggle1.isOn = commandData.follow;
				foreach (var item in commandData.imageComps) {
					item.color = new Color(1, 0, 0);
				}
				typeColorImage.color = new Color(1, 0, 0);
				#endregion
				break;
			case CommandType.SpawnLaser:
				#region SpawnLaser
				typeDropDown.value = 1;
				ReloadTabs(1);
				switch (commandData.spawnDir) {
					case Direction.Right:
						colorTypeDropDown.value = 0;
						break;
					case Direction.Down:
						colorTypeDropDown.value = 1;
						break;
					case Direction.Left:
						colorTypeDropDown.value = 2;
						break;
					case Direction.Up:
						colorTypeDropDown.value = 3;
						break;
				}
				spawnPosInput2.text = commandData.spawnPos.ToString();
				timeInput1.text = commandData.time.ToString();
				followToggle2.isOn = commandData.follow;
				foreach (var item in commandData.imageComps) {
					item.color = new Color(1, 0.51f, 0);
				}
				typeColorImage.color = new Color(1, 0.51f, 0);
				#endregion
				break;
			case CommandType.ChangeColor:
				#region ChangeColor
				typeDropDown.value = 2;
				ReloadTabs(2);
				switch (commandData.colorDataList) {
					case ColorDataList.Back:
						dirDropDown2.value = 0;
						break;
					case ColorDataList.Level:
						dirDropDown2.value = 1;
						break;
					case ColorDataList.Enemy:
						dirDropDown2.value = 2;
						break;
				}
				rInput.text = commandData.r.ToString();
				gInput.text = commandData.g.ToString();
				bInput.text = commandData.b.ToString();
				timeInput2.text = commandData.time.ToString();
				switch (commandData.lerpType) {
					case LerpType.None:
						modeDropDown1.value = 0;
						break;
					case LerpType.EaseIn:
						modeDropDown1.value = 1;
						break;
					case LerpType.EaseOut:
						modeDropDown1.value = 2;
						break;
					case LerpType.SmoothStep:
						modeDropDown1.value = 3;
						break;
					case LerpType.InvSmoothStep:
						modeDropDown1.value = 4;
						break;
					case LerpType.Bounce:
						modeDropDown1.value = 5;
						break;
					case LerpType.TriWave:
						modeDropDown1.value = 6;
						break;
					case LerpType.SinWave:
						modeDropDown1.value = 7;
						break;
					case LerpType.SqrWave:
						modeDropDown1.value = 8;
						break;
					case LerpType.SawWave:
						modeDropDown1.value = 9;
						break;
				}
				levelInput1.text = commandData.level.ToString();
				foreach (var item in commandData.imageComps) {
					item.color = new Color(1, 0.9f, 0);
				}
				typeColorImage.color = new Color(1, 0.9f, 0);
				#endregion
				break;
			case CommandType.ResizeLevel:
				#region ResizeLevel
				typeDropDown.value = 3;
				ReloadTabs(3);
				widthInput.text = commandData.width.ToString();
				heightInput.text = commandData.height.ToString();
				foreach (var item in commandData.imageComps) {
					item.color = new Color(0.25f, 0.8f, 0.45f);
				}
				typeColorImage.color = new Color(0.25f, 0.8f, 0.45f);
				#endregion
				break;
			case CommandType.RotateLevel:
				#region RotateLevel
				typeDropDown.value = 4;
				ReloadTabs(4);
				angleInput.text = commandData.angle.ToString();
				timeInput3.text = commandData.time.ToString();
				switch (commandData.lerpType) {
					case LerpType.None:
						modeDropDown2.value = 0;
						break;
					case LerpType.EaseIn:
						modeDropDown2.value = 1;
						break;
					case LerpType.EaseOut:
						modeDropDown2.value = 2;
						break;
					case LerpType.SmoothStep:
						modeDropDown2.value = 3;
						break;
					case LerpType.InvSmoothStep:
						modeDropDown2.value = 4;
						break;
					case LerpType.Bounce:
						modeDropDown2.value = 5;
						break;
					case LerpType.TriWave:
						modeDropDown2.value = 6;
						break;
					case LerpType.SinWave:
						modeDropDown2.value = 7;
						break;
					case LerpType.SqrWave:
						modeDropDown2.value = 8;
						break;
					case LerpType.SawWave:
						modeDropDown2.value = 9;
						break;
				}
				levelInput2.text = commandData.level.ToString();
				foreach (var item in commandData.imageComps) {
					item.color = new Color(0, 0.5f, 0.15f);
				}
				typeColorImage.color = new Color(0, 0.5f, 0.15f);
				#endregion
				break;
			case CommandType.EnlargeLevel:
				#region EnlargeLevel
				typeDropDown.value = 5;
				ReloadTabs(5);
				rateInput.text = commandData.rate.ToString();
				timeInput4.text = commandData.time.ToString();
				switch (commandData.lerpType) {
					case LerpType.None:
						modeDropDown3.value = 0;
						break;
					case LerpType.EaseIn:
						modeDropDown3.value = 1;
						break;
					case LerpType.EaseOut:
						modeDropDown3.value = 2;
						break;
					case LerpType.SmoothStep:
						modeDropDown3.value = 3;
						break;
					case LerpType.InvSmoothStep:
						modeDropDown3.value = 4;
						break;
					case LerpType.Bounce:
						modeDropDown3.value = 5;
						break;
					case LerpType.TriWave:
						modeDropDown3.value = 6;
						break;
					case LerpType.SinWave:
						modeDropDown3.value = 7;
						break;
					case LerpType.SqrWave:
						modeDropDown3.value = 8;
						break;
					case LerpType.SawWave:
						modeDropDown3.value = 9;
						break;
				}
				levelInput3.text = commandData.level.ToString();
				foreach (var item in commandData.imageComps) {
					item.color = new Color(0, 0.6f, 0.75f);
				}
				typeColorImage.color = new Color(0, 0.6f, 0.75f);
				#endregion
				break;
			case CommandType.MoveLevel:
				#region MoveLevel
				typeDropDown.value = 6;
				ReloadTabs(6);
				xInput.text = commandData.x.ToString();
				yInput.text = commandData.y.ToString();
				timeInput5.text = commandData.time.ToString();
				switch (commandData.lerpType) {
					case LerpType.None:
						modeDropDown4.value = 0;
						break;
					case LerpType.EaseIn:
						modeDropDown4.value = 1;
						break;
					case LerpType.EaseOut:
						modeDropDown4.value = 2;
						break;
					case LerpType.SmoothStep:
						modeDropDown4.value = 3;
						break;
					case LerpType.InvSmoothStep:
						modeDropDown4.value = 4;
						break;
					case LerpType.Bounce:
						modeDropDown4.value = 5;
						break;
					case LerpType.TriWave:
						modeDropDown4.value = 6;
						break;
					case LerpType.SinWave:
						modeDropDown4.value = 7;
						break;
					case LerpType.SqrWave:
						modeDropDown4.value = 8;
						break;
					case LerpType.SawWave:
						modeDropDown4.value = 9;
						break;
				}
				levelInput4.text = commandData.level.ToString();
				foreach (var item in commandData.imageComps) {
					item.color = new Color(0, 0.32f, 0.65f);
				}
				typeColorImage.color = new Color(0, 0.32f, 0.65f);
				#endregion
				break;
			case CommandType.ReplacePlayer:
				#region ReplacePlayer
				typeDropDown.value = 7;
				ReloadTabs(7);
				playerXInput.text = commandData.playerX.ToString();
				playerYInput.text = commandData.playerY.ToString();
				foreach (var item in commandData.imageComps) {
					item.color = new Color(0, 0.15f, 0.58f);
				}
				typeColorImage.color = new Color(0, 0.15f, 0.58f);
				#endregion
				break;
			case CommandType.PlayerVisible:
				#region PlayerVisible
				typeDropDown.value = 8;
				ReloadTabs(8);
				visibleToggle.isOn = commandData.visible;
				foreach (var item in commandData.imageComps) {
					item.color = new Color(0.6f, 0.07f, 0.55f);
				}
				typeColorImage.color = new Color(0.6f, 0.07f, 0.55f);
				#endregion
				break;
			case CommandType.Kill:
				#region Kill
				typeDropDown.value = 9;
				ReloadTabs(9);
				switch (commandData.entityTag) {
					case "Enemy":
						tagDropDown.value = 0;
						break;
					case "Laser":
						tagDropDown.value = 1;
						break;
					case "Heart":
						tagDropDown.value = 2;
						break;
					default:
						tagDropDown.value = 0;
						break;
				}
				foreach (var item in commandData.imageComps) {
					item.color = new Color(1f, 0.01f, 0.41f);
				}
				typeColorImage.color = new Color(1f, 0.01f, 0.41f);
				#endregion
				break;
		}
	}

	public void UpdateActiveTime(string activetime) {
		bool canParse = float.TryParse(activetime, out commandData.activeTime);
		if (canParse) {
			if (commandData.activeTime <= EditManageScript.Instance.musicLength) {
				if (commandData.activeTime > 0) {
					commandObj.transform.localPosition = new Vector3(((commandData.activeTime / EditManageScript.Instance.musicLength) * 1024.0f) - 512, 0, 0);
				}
				else {
					commandData.activeTime = 0;
					activeTimeInput.text = commandData.activeTime.ToString();
					commandObj.transform.localPosition = new Vector3(-512, 0, 0);
				}
			}
			else {
				commandData.activeTime = EditManageScript.Instance.musicLength;
				activeTimeInput.text = commandData.activeTime.ToString();
				commandObj.transform.localPosition = new Vector3(512, 0, 0);
			}
		}
		else {
			activeTimeInput.text = commandData.activeTime.ToString();
			Debug.LogWarning("Can't Parse " + activetime + " to demical number");
		}
	}

	public void UpdateType(int type) {
		try {
			commandData.type = (CommandType)Enum.Parse(typeof(CommandType), type.ToString());
			Load();
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + type + " to CommandType");
		}
	}

	public void UpdateDirection(int dir) {
		try {
			commandData.spawnDir = (Direction)Enum.Parse(typeof(Direction), dir.ToString());
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + dir + " to Direction");
		}
	}

	public void UpdateSpawnPos(string spawnPos) {
		try {
			commandData.spawnPos = int.Parse(spawnPos);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + spawnPos + " to integer number");
		}
	}

	public void UpdateSpeed(string speed) {
		try {
			commandData.speed = float.Parse(speed);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + speed + " to demial number");
		}
	}

	public void UpdateVelo(string velo) {
		try {
			commandData.velo = float.Parse(velo);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + velo + " to demial number");
		}
	}

	public void UpdateIsHeart(bool isHeart) {
		commandData.isHeart = isHeart;
	}

	public void UpdateFollow(bool follow) {
		commandData.follow = follow;
	}

	public void UpdateTime(string time) {
		try {
			commandData.time = float.Parse(time);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + time + " to demial number");
		}
	}

	public void UpdateColorType(int colortype) {
		try {
			commandData.colorDataList = (ColorDataList)Enum.Parse(typeof(ColorDataList), colortype.ToString());
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + colortype + " to ColorDataList");
		}
	}

	public void UpdateR(string r) {
		try {
			commandData.r = float.Parse(r);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + r + " to demial number");
		}
	}

	public void UpdateG(string g) {
		try {
			commandData.g = float.Parse(g);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + g + " to demial number");
		}
	}

	public void UpdateB(string b) {
		try {
			commandData.b = float.Parse(b);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + b + " to demial number");
		}
	}

	public void UpdateLerpType(int lerptype) {
		try {
			commandData.lerpType = (LerpType)Enum.Parse(typeof(LerpType), lerptype.ToString());
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + lerptype + " to LerpType");
		}
	}

	public void UpdateLevel(string level) {
		try {
			commandData.level = int.Parse(level);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + level + " to integer number");
		}
	}

	public void UpdateWidth(string width) {
		try {
			commandData.width = int.Parse(width);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + width + " to integer number");
		}
	}

	public void UpdateHeight(string height) {
		try {
			commandData.height = int.Parse(height);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + height + " to integer number");
		}
	}

	public void UpdateAngle(string angle) {
		try {
			commandData.angle = float.Parse(angle);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + angle + " to demial number");
		}
	}

	public void UpdateRate(string rate) {
		try {
			commandData.rate = float.Parse(rate);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + rate + " to demial number");
		}
	}

	public void UpdateX(string x) {
		try {
			commandData.x = float.Parse(x);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + x + " to demial number");
		}
	}

	public void UpdateY(string y) {
		try {
			commandData.y = float.Parse(y);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + y + " to demial number");
		}
	}

	public void UpdatePlayerX(string x) {
		try {
			commandData.playerX = int.Parse(x);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + x + " to integer number");
		}
	}

	public void UpdatePlayerY(string y) {
		try {
			commandData.playerY = int.Parse(y);
		}
		catch (Exception) {
			Debug.LogWarning("Can't Parse " + y + " to integer number");
		}
	}

	public void UpdateVisible(bool visible) {
		commandData.visible = visible;
	}

	public void UpdateTag(int tag) {
		switch (tag) {
			case 0:
				commandData.entityTag = "Enemy";
				break;
			case 1:
				commandData.entityTag = "Laser";
				break;
			case 2:
				commandData.entityTag = "Heart";
				break;
			default:
				commandData.entityTag = "Enemy";
				Debug.LogWarning("Unexpected Tag Index : " + tag);
				break;
		}
	}

	public void Copy() {
		switch (commandData.type) {
			case CommandType.SpawnEnemy:
				copyData.type = CommandType.SpawnEnemy;
				copyData.spawnDir = commandData.spawnDir;
				copyData.spawnPos = commandData.spawnPos;
				copyData.speed = commandData.speed;
				copyData.velo = commandData.velo;
				copyData.isHeart = commandData.isHeart;
				copyData.follow = commandData.follow;
				break;
			case CommandType.SpawnLaser:
				copyData.type = CommandType.SpawnLaser;
				copyData.spawnDir = commandData.spawnDir;
				copyData.spawnPos = commandData.spawnPos;
				copyData.time = commandData.time;
				copyData.follow = commandData.follow;
				break;
			case CommandType.ChangeColor:
				copyData.type = CommandType.ChangeColor;
				copyData.r = commandData.r;
				copyData.g = commandData.g;
				copyData.b = commandData.b;
				copyData.time = commandData.time;
				copyData.lerpType = commandData.lerpType;
				copyData.level = commandData.level;
				break;
			case CommandType.ResizeLevel:
				copyData.type = CommandType.ResizeLevel;
				copyData.width = commandData.width;
				copyData.height = commandData.height;
				break;
			case CommandType.RotateLevel:
				copyData.type = CommandType.RotateLevel;
				copyData.angle = commandData.angle;
				copyData.time = commandData.time;
				copyData.lerpType = commandData.lerpType;
				copyData.level = commandData.level;
				break;
			case CommandType.EnlargeLevel:
				copyData.type = CommandType.EnlargeLevel;
				copyData.rate = commandData.rate;
				copyData.time = commandData.time;
				copyData.lerpType = commandData.lerpType;
				copyData.level = commandData.level;
				break;
			case CommandType.MoveLevel:
				copyData.type = CommandType.MoveLevel;
				copyData.x = commandData.x;
				copyData.y = commandData.y;
				copyData.time = commandData.time;
				copyData.lerpType = commandData.lerpType;
				copyData.level = commandData.level;
				break;
			case CommandType.ReplacePlayer:
				copyData.type = CommandType.ReplacePlayer;
				copyData.playerX = commandData.playerX;
				copyData.playerY = commandData.playerY;
				break;
			case CommandType.PlayerVisible:
				copyData.type = CommandType.PlayerVisible;
				copyData.visible = commandData.visible;
				break;
			case CommandType.Kill:
				copyData.type = CommandType.Kill;
				copyData.entityTag = commandData.entityTag;
				break;
		}
	}

	public void Paste() {
		switch (copyData.type) {
			case CommandType.SpawnEnemy:
				commandData.type = CommandType.SpawnEnemy;
				commandData.spawnDir = copyData.spawnDir;
				commandData.spawnPos = copyData.spawnPos;
				commandData.speed = copyData.speed;
				commandData.velo = copyData.velo;
				commandData.isHeart = copyData.isHeart;
				commandData.follow = copyData.follow;
				break;
			case CommandType.SpawnLaser:
				commandData.type = CommandType.SpawnLaser;
				commandData.spawnDir = copyData.spawnDir;
				commandData.spawnPos = copyData.spawnPos;
				commandData.time = copyData.time;
				commandData.follow = copyData.follow;
				break;
			case CommandType.ChangeColor:
				commandData.type = CommandType.ChangeColor;
				commandData.r = copyData.r;
				commandData.g = copyData.g;
				commandData.b = copyData.b;
				commandData.time = copyData.time;
				commandData.lerpType = copyData.lerpType;
				commandData.level = copyData.level;
				break;
			case CommandType.ResizeLevel:
				commandData.type = CommandType.ResizeLevel;
				commandData.width = copyData.width;
				commandData.height = copyData.height;
				break;
			case CommandType.RotateLevel:
				commandData.type = CommandType.RotateLevel;
				commandData.angle = copyData.angle;
				commandData.time = copyData.time;
				commandData.lerpType = copyData.lerpType;
				commandData.level = copyData.level;
				break;
			case CommandType.EnlargeLevel:
				commandData.type = CommandType.EnlargeLevel;
				commandData.rate = copyData.rate;
				commandData.time = copyData.time;
				commandData.lerpType = copyData.lerpType;
				commandData.level = copyData.level;
				break;
			case CommandType.MoveLevel:
				commandData.type = CommandType.MoveLevel;
				commandData.x = copyData.x;
				commandData.y = copyData.y;
				commandData.time = copyData.time;
				commandData.lerpType = copyData.lerpType;
				commandData.level = copyData.level;
				break;
			case CommandType.ReplacePlayer:
				commandData.type = CommandType.ReplacePlayer;
				commandData.playerX = copyData.playerX;
				commandData.playerY = copyData.playerY;
				break;
			case CommandType.PlayerVisible:
				commandData.type = CommandType.PlayerVisible;
				commandData.visible = copyData.visible;
				break;
			case CommandType.Kill:
				commandData.type = CommandType.Kill;
				commandData.entityTag = copyData.entityTag;
				break;
		}
		commandData.OpenSetting();
	}

	public void Delete() {
		Destroy(commandObj);
		commandData = null;
		gameObject.SetActive(false);
	}

	public void OK() {
		gameObject.SetActive(false);
	}

	public void UpdateTip() {
		if (preCommandObj != commandObj) {
			if (tips.Count > 0) {
				tipText.text = tips[UnityEngine.Random.Range(0, tips.Count)];
				tipText.text = tipText.text.Replace("\\n", "\n");
			}
			else {
				tipText.text = string.Empty;
			}
		}
	}
}
