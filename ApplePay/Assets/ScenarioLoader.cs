using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScenarioLoader : MonoBehaviour
{
    public RoomSpawner Spawner;
    public Transform[] Portable;
    private void Start() => InitLoader();
    private void InitLoader()
    {
        if(IsRandom) CurrentScenario = (byte)Random.Range(0, Scenarios.Count);
        this.LoadNext();
        TargetScenariosCount = (byte)Random.Range(minScenarios, maxScenarios);
    }
    public bool IsRandom;
    [SerializeField] private byte minScenarios = 1;
    [SerializeField] private byte maxScenarios = 1;
    public byte TargetScenariosCount;
    [HideInInspector] public byte CurrentScenario;
    [HideInInspector] public byte InitializedScenariosCount;
    public System.Collections.Generic.List<FloorScenario> Scenarios = new System.Collections.Generic.List<FloorScenario>();
}
public static class ScenarioExtension
{
    private static void InitScenario(this ScenarioLoader loader, byte index)
    {
        FloorScenario floorScenario = loader.Scenarios[index];
        floorScenario.TargetNestScenariosCount = Random.Range(floorScenario.MinScenarios, floorScenario.MaxScenarios);
        Scene loadScene = SceneManager.CreateScene("main" + loader.InitializedScenariosCount);
        floorScenario.IsInitialized = true;
        loader.InitializedScenariosCount++;
        LoadScene(loadScene, loader.Portable);
        loader.GenerateFloorLevel(floorScenario.Main);
    }
    ///<summary>
    ///Loads next scenario according to the specified loader.
    ///</summary>

    public static void LoadNext(this ScenarioLoader loader)
    {
        FloorScenario currentScenario = loader.Scenarios[loader.CurrentScenario];
        if(currentScenario.IsInitialized == false)
        {
            loader.InitScenario(loader.CurrentScenario);
        }
        else if(currentScenario.ScenarioLoaded < currentScenario.TargetNestScenariosCount)
        {
            Scene loadScene = SceneManager.CreateScene("floor" + (loader.InitializedScenariosCount -1) + (currentScenario.ScenarioLoaded + 1));
            LoadScene(loadScene, loader.Portable);
            byte loadFloorIndex = 0;
            if(currentScenario.Random == true) loadFloorIndex = (byte)Random.Range(0, currentScenario.NestScenarios.Count);
            else loadFloorIndex = (byte)Mathf.Repeat(currentScenario.ScenarioLoaded + 1, currentScenario.NestScenarios.Count);

            if(currentScenario.NestScenarios[loadFloorIndex].Repeatable == false) currentScenario.NestScenarios.RemoveAt(loadFloorIndex);
            loader.Scenarios[loader.CurrentScenario].ScenarioLoaded++;
            loader.GenerateFloorLevel(currentScenario.NestScenarios[loadFloorIndex].Scenario);
        }
        else
        {
            loader.Scenarios.RemoveAt(loader.CurrentScenario);
            if(loader.Scenarios.Count == 0 || loader.InitializedScenariosCount >= loader.TargetScenariosCount)
            {
                Debug.Log("Scenario is over!");
                return;
            }
            if(loader.IsRandom) loader.CurrentScenario = (byte)Random.Range(0, loader.Scenarios.Count - 1);
            else loader.CurrentScenario = (byte)Mathf.Repeat(loader.CurrentScenario, loader.Scenarios.Count - 1);
            loader.InitScenario(loader.CurrentScenario);
        }
    }
    private static void GenerateFloorLevel(this ScenarioLoader loader, FloorLevelScenario levelScenario)
    {
        loader.Spawner.StartObjects = loader.Portable.ToList();
        loader.Spawner.Scenario = levelScenario;
        loader.Spawner.GenerateLevel();
    }
    private static void LoadScene(Scene scene, Transform[] portable)
    {
        Scene[] scenes = new Scene[SceneManager.sceneCount];
        for(int i = 0; i < SceneManager.sceneCount; i++) scenes[i] = SceneManager.GetSceneAt(i);
        
        UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(scene.name ,new LoadSceneParameters(LoadSceneMode.Additive));
        
        foreach(Transform gameObject in portable)
        {
            SceneManager.MoveGameObjectToScene(gameObject.gameObject, scene);
        }
        foreach(Scene _scene in scenes)
        {
            if(_scene != scene)
            {
                SceneManager.UnloadSceneAsync(_scene);
            }
        }
    }
}
[System.Serializable]

public class FloorScenario
{
    public FloorLevelScenario Main;
    [HideInInspector] public bool IsInitialized;
    public bool Random;
    public byte MinScenarios;
    public byte MaxScenarios;
    internal int TargetNestScenariosCount;
    internal byte ScenarioLoaded;
    public System.Collections.Generic.List<NestFloorLevel> NestScenarios = new System.Collections.Generic.List<NestFloorLevel>();
}

[System.Serializable]

public struct NestFloorLevel
{
    public bool Repeatable;
    public FloorLevelScenario Scenario;
}