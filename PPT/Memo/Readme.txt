# Adventure
First 3D Project

## 📖 목차

1. [프로젝트 소개](#프로젝트-소개)
3. [게임구조](#게임구조)
4. [주요기능](#주요기능)
5. [개발기간](#개발기간)
6. [기술스택](#기술스택)
7. [Trouble Shooting](#trouble-shooting)

---
    
## 프로젝트 소개

- 다양한 장애물을 피해 회피하고 최고 지점에 올라가는 온리업류 게임의 주요 기능들을 만듬

---

## 주요기능

- 기능 1. 기본 이동 및 점프
![CharacterMove](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/Base.PNG)
WASD와 마우스를 통해 움직일 수 있고, Space를 눌러 점프할 수 있습니다. Shift를 누르면 달립니다.

<br>

- 기능 2. 체력 바
  - 체력과 마나, 스태미나를 가지고있다.
  - ConditionBar와 UICondition에 의해 수치가 변한다.
  - 플레이어는 PlayerCondition에서 각각 스탯에 해당하는 Condition을 UICondition에서 가져온다.
<br>

- 기능 3. 오브젝트 이름 표시
![Interaction](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/FPInteractionWithC.PNG)
  - Entities/Player - Interaction
	  - Search함수를 지속적으로 사용하여 3인칭일때에는 정면을, 1인칭일때에는 화면 중앙을 조사한다.
	  - 조사당한 물체의 IInteractable을 가져와서 해당 물체의 이름과 설명을 화면에 띄어준다.
    - E키를 눌러 획득하거나, 버튼을 누를 수 있다.

<br>

- 기능 4. 아이템 데이터 관리 및 아이템 사용
![Inventory](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/Inventory.PNG)
  - ScriptableObjects/Scripts, Item
  - 3가지 스탯을 회복시켜주는 포션종류 3가지와 장착아이템 2가지를 획득 할 수 있다.
  - 장착을하면 캐릭터 손에 장착아이템이 위치하게 된다.
  - 장착 시 해당 슬롯은 빨간 외곽선이 생긴다.


- 기능 5. 점프대
![JumpPad](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/JumpPad.PNG)
  - 점프패드를 밟을 시 위 방향으로 AddForce의 Impulse모드가 작동된다.
<br>

- 기능 6. 3인칭과 1인칭 변경
![FP](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/pressV.PNG)
  - V를 누르면 3인칭과 1인칭을 바꿀 수 있다.
  - 시네머신을 이용
<br>

- 기능 7. 움직이는 플랫폼, 대포 그리고 레이저트랩
![Cannon](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/Cannon.PNG)
  - 대포에 들어가 버튼을 누르면 3초뒤 AddForce.Impulse모드로 날라지만 포물선을 그리지 못하는 문제가 존재
![CannonButtones](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/CannonButtons.PNG)
  - 위, 아래버튼(파란색)을 눌러(E) 포신의 방향을 조절할 수 있다.
  - 발사버튼(빨간색)을 눌러(E) 발사 할 수 있다.
![RazorTrap](https://github.com/Zzondy-Unity/Adventure/blob/main/Captures/RazorTrap.PNG)
  - 위 장면은 방향설정을 잘못한 장면이긴하다.
  - 레이저는 특정지점을 패트롤하며 레이저에 닿은 물체는 데미지를 입는다.
<br>


---

## 개발기간

- 2024.10.23(월) ~ 2024.10.30(화)   

---

## 기술스택

- 유니티 2022.3.17f LTS   
- Microsoft Visual Studio 2022   
- GitHub   

---

## Trouble Shooting

<details>
  <summary>자연스러운 애니메이션</summary>
    <div markdown="1">
      <ul>
        <li>4방향 애니메이션과 달리는 애니메이션을 적용중 계속해서 애니메이션이 끊기는 현상을 발견.</li>
        <li>AnyState에서 해서 이동중에도 또 이동모션이 추가로 들어가서 생기는 문제. Idle에서 움직임을 담당하는 BlendTree를 연결하는것으로 해결</li>
      </ul>
    </div>
</details>

<details>
  <summary>대포의 포물선운동</summary>
    <div markdown="1">
      <ul>
        <li>AddForce.Impulse를 제대로된 방향으로 지정해도 위로만 캐릭터가 움직이는 문제 발견.</li>
        <li>포물선 운동의 경우 공식이 나와있으므로 해당 공식을 이용하면 문제 해결 가능.</li>
        <li>다만 시간이 부족해서 이번엔 적용하지 못햇다.</li>
      </ul>
    </div>
</details>

<details>
  <summary>인벤토리</summary>
    <div markdown="1">
      <ul>
        <li>인벤토리를 만드는데 강의를 보면서해도 시간이 오래걸리고, 난이도가 높았음</li>
      </ul>
    </div>
</details>