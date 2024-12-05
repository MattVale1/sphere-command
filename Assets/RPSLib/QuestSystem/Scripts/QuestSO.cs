/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using UnityEngine;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests/New Quest")]
    public class QuestSO : ScriptableObject {

        [HideInInspector]
        public QuestData data = new();

        [Header("DETAILS")]
        public string QUEST_ID;
        public bool IsObjective;
        public string sourceOfQuest;
        public string title;
        [TextArea] public string description;

        [Header("OBJECTIVES")]
        public List<QuestSO> Objectives;

        [Header("REWARDS")]
        public List<QuestReward> rewards = new();
        public List<QuestReward> bonusRewards; // Could be used for completing a task perfectly
        public bool bonusRewardsEarned;

        [Header("PROGRESS")]
        public int requiredQuestSteps;
        public int questProgress;

        [Header("STATES")]
        public bool isActive;
        public bool isCompleted;

        [Header("PREREQUISITE")]
        public List<QuestSO> mustHaveCompleted; // Used to determine if we need to have completed another quest to be able to do this one    

        [Header("POSTREQUISITE")]
        public List<QuestSO> questsToProgress; // Used to trigger another quests progress

        [Header("POSTREQUISITE")]
        public List<QuestSO> triggersNextQuest; // Used to trigger a new quest if required

    }

}