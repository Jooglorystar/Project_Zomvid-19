# Project Zombid 19

Unity 3D Zombie Survival Project
---


## 📖 목차

1. [프로젝트 소개](#프로젝트-소개)
2. [팀원구성](#팀원구성)
3. [게임구조](#게임구조)
4. [주요기능](#주요기능)
5. [개발기간](#개발기간)
6. [기술스택](#기술스택)
7. [Trouble Shooting](#trouble-shooting)

---
    
## 프로젝트 소개

- 추위와 좀비가 가득한 세상에서 자원을 수집하고 홀로 생존하는 고독한 서바이벌 게임
- Secne/Final/MainScene

---

## 팀원구성

- 손형민(팀장) - 인벤토리, 크래프팅시스템, 좀비AI조정, 플레이어 및 아이템 관련 시스템
- 이지혜(팀원) - 좀비, 토끼, 스폰존 제작
- 성요셉(팀원) - Terrain 맵 제작, 날씨 및 환경, 환경 자원, 건축 시스템
- 주창규(팀원) - 

---

## 주요기능

- 기능 1. 기본 이동 및 점프
  
WASD와 마우스를 통해 움직일 수 있고, Space를 눌러 점프할 수 있습니다. Shift를 누르면 달립니다.

<br>

- 기능 2. 컨디션
  
  - 체력과 스태미나, 배고픔, 목마름, 체온이 있다.
  - 

<br>

- 기능 3. 자원 수집

  - 

<br>

- 기능 4. 크래프팅 시스템
![Crafting](https://github.com/user-attachments/assets/90f6fbde-4c12-4854-a1e1-94e7960e284a)
  - Tab을 눌러 인벤토리 창을 연 후 상단의 Crafing 탭에서 사용 가능


- 기능 5. 건축
![Build_1](https://github.com/user-attachments/assets/486cdef7-e884-4a9b-b963-0b6d75b1a5cf)
![Build_2](https://github.com/user-attachments/assets/26acd56b-896b-4e84-a463-eeee80ceba6d)
![Build_3](https://github.com/user-attachments/assets/14a25b44-65d5-413a-b78d-82ad954cbd19)
  - 건축용 아이템을 선택하고 Build 버튼을 누르면 설치 가능 상태가 되며 E 버튼을 눌러 설치할 수 있다.
<br>

- 기능 6. 전투
![image](https://github.com/user-attachments/assets/07b17f8b-24c2-49ee-bf8f-7a1fa5513c3a)
![image](https://github.com/user-attachments/assets/e592a4ff-7474-4bcf-8f8a-0725b99577d0)
  - 좀비가 플레이어를 공격할 수 있다. 플레이어는 마우스 좌클릭을 통해 좀비를 공격할 수 있다.
  - 토끼는 플레이어 감지 시 도망간다. 플레이어는 마우스 좌클릭을 통해 토끼를 공격할 수 있다.

<br>

- 기능 7. 날씨 및 환경
![weather_transition_1](https://github.com/user-attachments/assets/bcc9b684-1dae-4607-a31d-eb4cdf6e13e8)
![weather_transition_2](https://github.com/user-attachments/assets/f03a63a0-1d95-48d8-b429-b2f92c6a72a9)
![weather_transition_3](https://github.com/user-attachments/assets/15077111-5e01-4c1c-bb1a-1ac2ea32eea5)
  - 부드러운 배경 전환
<br>

---

## 개발기간

- 2024.10.31(수) ~ 2024.11.7(목)   

---

## 기술스택

- 유니티 2022.3.17f LTS   
- Microsoft Visual Studio 2022   
- GitHub   

---

## Trouble Shooting

<details>
  <summary>손형민 : </summary>
    <div markdown="1">
      <ul>
        <li></li>
        <li></li>
      </ul>
    </div>
</details>

<details>
  <summary>이지혜 : </summary>
    <div markdown="1">
      <ul>
          <li>좀비가 플레이어 발견 시 움직임이 지나치게 빨라졌습니다.</li>
        <li>원래는 걷기 모션만 이용하기위해 anget스피드에 맞춰 움직임속도를 조정해줬었습니다.</li>
        <li>달리기 모션을 추가하면서 조정해주는 코드를 지워주지 않아 생겼던 문제였습니다.</li>
      </ul>
    </div>
</details>

<details>
  <summary>성요셉 : </summary>
    <div markdown="1">
      <ul>
        <li>날씨 전환을 표현하기 위해 스카이박스에 트랜지션이 필요했는데 이를 유니티에서 지원하지 않았다.</li>
        <li>이를 직접 구현하기 위해 사용자 지정 셰이더를 만들어 해당 셰이더를 사용하는 머티리얼을 스카이박스로 사용했습니다.</li>
      </ul>
    </div>
</details>

<details>
  <summary>주창규 : </summary>
    <div markdown="1">
      <ul>
        <li></li>
      </ul>
    </div>
</details>
