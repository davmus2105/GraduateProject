using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace TES
{
    [CreateAssetMenu(fileName = "SceneChangeTrigger", menuName = "TES/Components/SceneChanger")]
    public class SceneChangeTrigger : TriggerEventComponent
    {
        public int requiredamount;
        public string dialogue_name;
        public int scene_id;
        public override void ExecuteEvent(Collider collider = null, TriggerEventContainer container = null)
        {
            int result = ResultManager.Instance.GetResult(dialogue_name);
            if (result >= requiredamount)
                ChangeScene();
            else
            {
                HUD_Controller.Instance.ShowInfoMessage("Ви не відповили правильно на всі питання");
            }
        }
        void ChangeScene()
        {
            QuestSystem.QuestManager.Instance.CompleteQuest(1);
            // TODO:
            // The way that scene will be changed
            SceneManager.LoadScene(scene_id);
            Debug.Log("Your way is finished");
        }
    }
}