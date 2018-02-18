# RhythmGame

## 소개
본 게임은 [Soundodger+](soundodger.com/), [Orbit or Beat](http://www.zuzunza.com/myflash/game_detail.html?game_id=562091) 등 여러 게임들에서 영향을 받아 제작되고 있는 리듬 탄막 게임입니다. 흘러나오는 배경음악에 맞추어 작은 빨간색 사각형을 조종해 사방에서 나오는 장애물을 피하는 것이 게임의 주 내용이며, 현재는 기본적인 기능과 레이아웃 정도만 구현되어 있는 상태입니다.

[![Image](http://img.youtube.com/vi/mapm9Q-u6v0/0.jpg)](https://www.youtube.com/watch?v=mapm9Q-u6v0)

## 조작법

방향키로 조작하며, 부드럽게 움직이는 것이 아니라 한번에 한 칸씩 바로 이동합니다. 또한 나중에 수정될 부분이지만 현재 버전에서는 ESC키를 누름과 동시에 게임이 종료되도록 설정되어 있습니다.

## 설명

### 게임 플레이

+ **블럭 장애물 생성**

블럭 형태의 직진하는 장애물을 생성합니다. 생성된 장애물은 게임판의 너비와 높이에 맞추어 자동으로 위치가 변경됩니다.

+ **레이저 장애물 생성**

레이저 형태의 장애물을 생성합니다. 생성된 장애물은 게임판의 너비와 높이에 맞추어 자동으로 위치가 변경됩니다. 점점 투명해지며 일정 시간이 지나면 사라집니다.

+ **하트 생성**

하트를 생성합니다. 아직 스프라이트가 준비되어 있지 않아 블럭 장애물과 같은 모양을 띄고 있지만 피격 시 남은 생명이 1 증가합니다. 생성된 하트는 게임판의 너비와 높이에 맞추어 자동으로 위치가 변경됩니다.

+ **게임판 크기 변경**

게임판의 너비와 높이를 변경합니다. 게임 특성상 게임판 자체의 크기가 변하는 것이 아닌 장애물과 플레이어 사각형의 크기가 변화합니다. 너비와 높이가 동일하도록 변경할 경우 플레이어와 장애물이 정사각형 모양으로 변하고, 너비와 높이가 서로 다르게 변경할 경우 플레이어와 장애물이 직사각형 모양으로 변하게 됩니다. 모든 엔티티는 게임판 크기에 맞추어 스스로 위치와 크기를 변화시킵니다.

+ **배경 & 장애물 & 외벽의 색 변경**

배경과 장애물, 그리고 외벽의 색은 각각 다른 색으로 변경할 수 있습니다. 색이 변화하는 시간은 유니티에서 제공하는 Lerp 기능을 통해 원하는 대로 조절할 수 있도록 구현했고, 색은 RGBA로 표현할 수 있는 모든 색을 사용할 수 있습니다. 만약 색이 변화하는 도중에 다른 색으로 변경을 시도한다면, 그 전에 진행 중이던 색 변화는 취소되고 새로운 명령이 실행됩니다.

+ **카메라 이동**

원하는 위치로 카메라를 이동시킵니다. 카메라를 이동시킴으로써 보다 다양한 연출을 만들 수 있고, 시각적인 즐거움 또한 증가시킬 수 있습니다.

+ **카메라 회전**

레벨 디자이너는 원하는 만큼 카메라를 회전할 수 있습니다. 극적인 연출이 필요할 때는 사용을 추천하지만, 카메라 이동과 함께 사용되면 게임판이 공전하는 형태가 되기 때문에 원하는 연출이 나오지 않을 수 있습니다.

+ **카메라 확대 & 축소**

원하는 만큼 카메라를 확대시키거나 축소시킵니다. 이 기능을 통해 보다 유동적이거나 역동적인 레벨을 만들 수 있습니다.

### 모드
+ **No Damage Mode**

플레이어가 장애물과 닿아도 생명이 감소하지 않습니다. 레벨의 전체적인 모습을 보는 데에 효과적입니다.

+ **Normal Mode**

기본 체력이 10으로 시작하며, 장애물과 닿으면 생명이 1씩 감소합니다.

+ **Challenge Mode**

기본 체력이 1로 시작하며, 장애물과 닿으면 바로 게임 오버가 됩니다. 자신의 실력을 검증해보고 싶은 사람들에게 추천합니다.

## 향후 계획

현재 버전은 계획 중인 여러 기능들과 대부분의 디자인이 포함되어 있지 않은 버전입니다. 3월 이후부터는 본격적으로 개발에 착수해 레벨 에디터, 메인 화면 디자인, 추가 레벨들 등을 완성할 예정입니다.
