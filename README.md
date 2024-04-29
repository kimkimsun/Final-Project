# 두 번째 개인프로젝트
> ### **개발 환경**

![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)
# 프로젝트 특징
#### 여태까지 알면서 사용하지 못하였던 기술들이나 기능들을 사용하고자 만든 프로젝트이며 원래는 완성을 하였으나,
#### 더욱 더 많이 공부하여 많은 기능들을 추가로 계획하고자 했던 생각을 가지게 되어 공부와 같이 진행중인 프로젝트 입니다.
# 게임 특징
![image](https://github.com/kimkimsun/Final-Project/assets/116052108/51e69b03-85db-4f19-bf30-abbcd6ee37a3) ![image](https://github.com/kimkimsun/Final-Project/assets/116052108/2c4bfa37-a868-4f28-a54c-ab72ab407366)
#### 스팀의 출시된 게임인 인사일런스와 그림자 복도에서 영감을 많이 받은 게임입니다.
#### 장르는 공포 3D 탈출 등등입니다.
# 게임 영상 (이미지 클릭)
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/Xsq0zKicf30/0.jpg)](https://youtu.be/Xsq0zKicf30)
# **핵심 기능**
## 첫 번째 기능
### 기능설명
1. 몬스터는 여러가지 상태를 가지고 있습니다.
2. 배회를 하게 되는 Idle 상태, 플레이어 추격을 하는 Run상태, 가만히 있게 되는 Stun상태 등이 있습니다.
3. 추격 상태로 변경되는 기준은 몬스터 탐지범위 안에 오브젝트가 파악이 되는지에 따라 결정됩니다.
4. 추격 상태는 추격 상태 안에서도 추격 우선순위를 나누기 위해 Behaviour Tree로 구현하였습니다.
5. 이는 새로운 상태가 추가되더라도 콘텍스트 코드가 받는 영향을 최소화 하기 위하여 디자인 패턴 중 하나인 상태 패턴으로 구현하였습니다.

### 다이어그램
![마지막 갠프 다이어그램](https://github.com/kimkimsun/Final-Project/assets/116052108/92d02242-a0df-4550-b5da-029a79d78410)
#### 위와 같은 구조로 이루어져있습니다.
#### Behaviour Tree로 구현하게 된 이유는 몬스터가 폭죽이나 플레이어의 뛰는소리를 들어도 해당 위치로 추격을 하게 하였습니다.
#### 그렇게 가게된 위치에 플레이어가 존재한다면 다른 조건들은 다 무시한채 플레이어를 쫓게끔 우선순위를 주기 위하였습니다.
#### 이 외에 상태들은 Behaviour Tree를 사용하지 않고 상태 패턴만을 사용하였습니다.
<details>
    <summary>코드</summary>
    
### 코드
```C#
public class HiRilState {}
public class HiRilIdleState {}
public class HiRilRunState {}
public class HiRilStunState {}
public class HiRilAttackState {}
public class HiRilFinalState {}
```

```C#
public class HiRil : Monster
{
    protected override void Start()
    {
        sm = new StateMachine<HiRil>();
        sm.owner = this;
        sm.AddState("Idle", new HiRilIdleState());
        sm.AddState("Run", new HiRilRunState());
        sm.AddState("Stun", new HiRilStunState());
        sm.AddState("Attack", new HiRilAttackState());
        sm.AddState("Final", new HiRilFinalState());
        sm.SetState("Idle");
    }
}
```
</details>

## 두 번째 기능
### 기능 설명
1. 게임이 시작 될 시에 아이템이 스폰 가능한 오브젝트에 랜덤으로 아이템이 드랍됩니다.
2. 아이템 리스트라는 것이 존재해야겠다고 생각했고, 이 리스트를 모든 아이템이 스폰 가능한 오브젝트가 다 가지기엔 데이터가 무거워진다 느꼈습니다.
3. 이를 해결하기 위해 스크립터블 오브젝트를 활용한 경량화 패턴을 사용하였습니다.
### 코드
```C#
public class ItemSpawnObject : MonoBehaviour
{
    public ItemSpawnList itemSpawnList;
    void Start()
    {
        int randomSpawn = Random.Range(0, 2);

        if(randomSpawn == 0)
        {
            Item spawnPrefab = itemSpawnList[Random.Range(0, itemSpawnList.Count)];
            Instantiate(spawnPrefab, transform.position, Quaternion.identity);
        }
    }
}
```
#### ItemSpawnObject라는 클래스는 ItemSpawnList를 가지고 있으며, ItemSpawnList는 스크립터블 오브젝트 입니다.
#### 랜덤하게 생성하기 위하여 Random.Range함수를 사용하여 경량 패턴을 적극 활용해 좀 더 가볍게 만들었습니다.
## 세 번째 기능
### 기능 설명
1. 플레이어가 세이브 가능한 아이템을 만나서 상호작용할 시에 세이브 파일에 세이브가 되게 끔 하였습니다.
2. 게임을 처음 시작할 때 만약 세이브 파일이 있다면, 로드를 할 수 있게 하였습니다.
3. 이는 JSON을 활용하였습니다.
4. ### 사진 설명
![Untitled (1)](https://github.com/kimkimsun/Final-Project/assets/116052108/82d25ae6-2e6e-4744-a697-70154e354a2c)
### 코드
``` C#
public void SaveData(int fileIndex)
{
    fileName = fileName.Insert(8,fileIndex.ToString());
    StreamWriter sw;
    if (File.Exists(path + fileName) == false)
    {
        sw = File.CreateText(path + fileName);
    }
    else
    {
        sw = new StreamWriter(path + fileName);
    }
    sw.Write(JsonUtility.ToJson(saveData, true));
    sw.Close();
}

public void LoadData(int fileIndex)
{
    hiril = GameManager.Instance.hiril;
    haiken = GameManager.Instance.haiken;
    player = GameManager.Instance.player;
    fileName = fileName.Insert(8, fileIndex.ToString());
    if (File.Exists(path + fileName))
    {
        StreamReader sr = new StreamReader(path + fileName);
        saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
        sr.Close();
        saveData.Load(player, hiril, haiken);
    }
}
```
#### 위와 같은 코드로 이루어져있습니다.
## 그 외의 기능 및 추가 계획중인 기능들
### 그 외의 기능
#### 그 외에도 발소리 오브젝트를 생성하는 기능은 최적화를 생각해 오브젝트 풀링을 사용하였습니다.
#### 탈출 부품과 아이템에는 전략 패턴을 사용하였습니다.
#### UI에 경우에는 자료구조 중 하나인 Stack을 활용하였고 이외에도 많은 자료구조를 사용하였습니다.
#### 탈출 준비가 완료되었을 때 긴박함을 주기 위해 몬스터가 다같이 플레이어를 쫓아오는 기능은 옵저버 패턴을 활용하였습니다.
#### 플레이어의 문을 열고 닫기, 아이템 줍기 등은 카메라에서 Ray가 나오게 하였고 상호작용은 Interface로 구현하였습니다.
#### 그 외에도 많은 기능들을 구현하였으나, 가장 강조하고 싶은 3가지를 보여드렸습니다.
### 추가 계획중인 기능들
#### 네트워크를 활용해 멀티모드를 구현할 계획에 있습니다.
#### 최적화에 대해 좀 더 공부를 한 뒤 더 많은 최적화 기법을 사용 계획에 있습니다.
#### 더 많은 디자인패턴을 활용하여 좀 더 확장성 있게 개발할 계획에 있습니다.
