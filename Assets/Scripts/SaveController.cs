using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
	private string saveFile;

	private void Awake()
	{
		saveFile = Application.persistentDataPath + "/game_data.json";
	}

    public void SaveFactory(List<Transform> animalListOnFloor, List<Transform> foodListOnFloor, Camera cam, int _N, float speedAnimal, float timeScale)
    {
        JSONClassList _listJSON = new JSONClassList();
        _listJSON.ClassList = new List<JSONClassSingle>();

        foreach (var item in animalListOnFloor)
        {
            JSONClassSingle json = FillJSONClass(item,0, item.GetComponent<Animal>().FoodToEat.gameObject.GetHashCode());
            _listJSON.ClassList.Add(json);
        }

        foreach (var item in foodListOnFloor)
        {
            JSONClassSingle json = FillJSONClass(item, 1);
            _listJSON.ClassList.Add(json);
        }

        _listJSON.CameraPosition = Camera.main.transform.position;
        _listJSON.CameraRotation = Camera.main.transform.rotation.eulerAngles;
        _listJSON.DefaultSpeedAnimal = speedAnimal;
        _listJSON._N = _N;
        _listJSON.TimeScaleValue = timeScale;


        string stringJSON = JsonUtility.ToJson(_listJSON);
        File.WriteAllText(saveFile, stringJSON);
    }

    public JSONClassList LoadFactory()
    {
        var json = File.ReadAllText(saveFile);
        return JsonUtility.FromJson<JSONClassList>(json);
    }

    private JSONClassSingle FillJSONClass(Transform classT, int classType, int childHashCode = 0)
    {
        JSONClassSingle classJSON = new JSONClassSingle();

        classJSON.ClassType = classType; // 0 - animal, // 1 - food

        if (classType == 0)
        {
            classJSON.ChildHashCode = childHashCode;
        }

        classJSON.HashCode = classT.gameObject.GetHashCode();

        classJSON.Position = classT.transform.position;

        classJSON.QuaternionX = classT.transform.rotation.x;
        classJSON.QuaternionY = classT.transform.rotation.y;
        classJSON.QuaternionZ = classT.transform.rotation.z;
        classJSON.QuaternionW = classT.transform.rotation.w;

        return classJSON;
    }
}
