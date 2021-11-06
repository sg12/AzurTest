using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalFactory : MonoBehaviour
{
    public static int _N = 2;
    public static int MaxQuantityAnimal = _N*_N/2;
    private static float MaxTimeSecToReachFood = 5f;
    private static float DefaultSpeedAnimal = 3f;

    [SerializeField] private GameObject animalPrefab;
    [SerializeField] private GameObject foodPrefab;
    [Space]
    [SerializeField] private Transform floor;

    private static List<Transform> animalListOnFloor = new List<Transform>();
    private static List<Transform> foodListOnFloor = new List<Transform>();

    public delegate void ReachFoodByAnimalHandler(Animal animal, Food food);
    public ReachFoodByAnimalHandler ReachFoodEvent;

    void Start()
    {
        SetScaleFloor(2);
        SetSpeedValue(1);
        Time.timeScale = 1;
        ReachFoodEvent = (animal, food) => ReachFoodByAnimal(animal, food);
        StartCoroutine(AnimalFactoryProcess());
    }

    private IEnumerator AnimalFactoryProcess()
    {
        while (true)
        {
            if (CanSpawNextAnimal())
            {
                float radius = floor.localScale.x / 2f - (animalPrefab.transform.localScale.x / 2f);
                Animal newAnimal = SpawBase.SpawObject(floor.position, radius, floor, animalPrefab.transform, animalListOnFloor).GetComponent<Animal>();
                animalListOnFloor.Add(newAnimal.transform);
                SpawFoodAndStartSearch(newAnimal);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawFoodAndStartSearch(Animal animal)
    {
        float radius = ComputeRadiusForFood(MaxTimeSecToReachFood, DefaultSpeedAnimal);
        var newFood = SpawBase.SpawObject(animal.transform.position, radius, floor, foodPrefab.transform, foodListOnFloor);
        foodListOnFloor.Add(newFood.transform);
        animal.StartMoveToEat(newFood.GetComponent<Food>(), ReachFoodEvent, DefaultSpeedAnimal);
    }

    private bool CanSpawNextAnimal()
    {
        if (animalListOnFloor.Count < MaxQuantityAnimal)
            return true;
        else
            return false;
    }

    private void ReachFoodByAnimal(Animal animal, Food food)
    {
        StartCoroutine(EatFoodProcess(animal, food));
    }

    private IEnumerator EatFoodProcess(Animal animal, Food food)
    {
        food.StartDestroy();
        yield return new WaitForSeconds(1f);
        foodListOnFloor.Remove(food.transform);
        Destroy(food.gameObject);
        SpawFoodAndStartSearch(animal);
    }

    public float ComputeRadiusForFood(float maxTimeToReach, float defaultSpeedAnimal)
    {
        return Mathf.Sqrt(Mathf.Pow(maxTimeToReach * defaultSpeedAnimal,2)/2f);
    }

    public void SetScaleFloor(int scaleValue)
    {
        _N = scaleValue;
        MaxQuantityAnimal = _N * _N / 2;
        floor.localScale = new Vector3(scaleValue, floor.localScale.y, scaleValue);
    }

    public void SetSpeedValue(float speedValue)
    {
        DefaultSpeedAnimal = speedValue;
        foreach (var item in animalListOnFloor)
        {
            item.GetComponent<Animal>().UpdateSpeed(DefaultSpeedAnimal);
        }
    }

    public void SetTimeScale(int timeScaleValue)
    {
        Time.timeScale = timeScaleValue;
    }

    public void SaveFactoryState()
    {
        SaveController saveCont = GetComponent<SaveController>();
        saveCont.SaveFactory(animalListOnFloor, foodListOnFloor, Camera.main, _N, DefaultSpeedAnimal, Time.timeScale);
    }

    public void LoadFactoryState()
    {
        StopAllCoroutines();
        JSONClassList jsonList = GetComponent<SaveController>().LoadFactory();
        RestoreFactoryFromJSON(jsonList);
    }

    private void RestoreFactoryFromJSON(JSONClassList jsonList)
    {
        foreach (var item in animalListOnFloor)
            Destroy(item.gameObject);

        foreach (var item in foodListOnFloor)
            Destroy(item.gameObject);

        animalListOnFloor.Clear();
        foodListOnFloor.Clear();
        foreach (var item in jsonList.ClassList)
        {
            if (item.ClassType == 0)
            {
                var rotation = new Quaternion(item.QuaternionX, item.QuaternionY, item.QuaternionZ, item.QuaternionW);
                var newAnimal = SpawBase.SpawRestoredObject(item.Position, rotation, animalPrefab.transform).GetComponent<Animal>();
                animalListOnFloor.Add(newAnimal.transform);
                var foodItem = jsonList.ClassList.Where(x => (x.HashCode == item.ChildHashCode)).ToList()[0];

                var rotation2 = new Quaternion(foodItem.QuaternionX, foodItem.QuaternionY, foodItem.QuaternionZ, foodItem.QuaternionW);
                var newFood = SpawBase.SpawRestoredObject(foodItem.Position, rotation2, foodPrefab.transform);
                foodListOnFloor.Add(newFood.transform);
                newAnimal.StartMoveToEat(newFood.GetComponent<Food>(), ReachFoodEvent, DefaultSpeedAnimal);
            }
        }
        SetScaleFloor(jsonList._N);
        SetSpeedValue(jsonList.DefaultSpeedAnimal);
        Time.timeScale = jsonList.TimeScaleValue;
        StartCoroutine(AnimalFactoryProcess());
        Camera.main.transform.position = jsonList.CameraPosition;
        Camera.main.transform.rotation = Quaternion.Euler(jsonList.CameraRotation);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        foreach (var item in animalListOnFloor)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in foodListOnFloor)
        {
            Destroy(item.gameObject);
        }
        animalListOnFloor.Clear();
        foodListOnFloor.Clear();

        SceneManager.LoadScene(0);
    }
}
