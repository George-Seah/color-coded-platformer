using UnityEngine;
using Esper.ESave;
public class ESave : MonoBehaviour
{
    private SaveFile saveFile;
    void Awake()
    {
        saveFile = GetComponent<SaveFileSetup>().GetSaveFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveStar(string starIndex, bool collected){
        saveFile.AddOrUpdateData(starIndex, collected);
        saveFile.Save();
        Debug.Log($"starIndex: {starIndex}, collected: {collected}");
    }
    public bool LoadStarData(string starIndex){
        if(starIndex == null){
            Debug.Log($"starIndex is null.");
        }
        else{
            Debug.Log($"starIndex {starIndex} has been collected: {saveFile.GetData<bool>(starIndex)}.");
        }
        return saveFile.GetData<bool>(starIndex);
    }
    public void ResetStar(string starIndex){
        if(saveFile == null){
            saveFile = GetComponent<SaveFileSetup>().GetSaveFile();
        }
        saveFile.AddOrUpdateData(starIndex, false);
        saveFile.Save();
        Debug.Log($"starIndex: {starIndex}, collected: {saveFile.GetData<bool>(starIndex)}");
    }
}
