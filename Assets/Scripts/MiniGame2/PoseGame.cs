using System;
using System.Collections;
using Core;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGame2
{
    public class PoseGame : MiniGameCore
    {
        [Header("Pose Data")] 
        [SerializeField] private int pose;
        [SerializeField] private TextMeshProUGUI testPoseIndicator;
        [SerializeField] private TextMeshProUGUI testYourPoseIndicator;
        private int _poseCheck;
        private Coroutine _poseChecker;
        private float _localTimeModifier;
        private bool _hasWon;
        
        private void GetPose()
        {
            var vertical = Input.GetAxisRaw("Vertical");
            var horizontal = Input.GetAxisRaw("Horizontal");
            pose = vertical switch {
                1 => 1,
                0 => horizontal switch {
                    1 => 2,
                    0 => 0,
                    -1 => 3,
                    _ => pose
                },
                -1 => 4,
                _ => pose
            };
        }

        private void SetPoseIndicator(int nextPose, TextMeshProUGUI poseIndicator) =>
            poseIndicator.text = nextPose switch {
                0 => "IDLE",
                1 => "UP",
                2 => "RIGHT",
                3 => "LEFT",
                4 => "DOWN",
                _ => poseIndicator.text
            };

        private void Update()
        {
            if (GameHasEnded) return;
            SetPoseIndicator(pose, testYourPoseIndicator);
            GetPose();
            _poseChecker ??= StartCoroutine(nameof(PoseCheckerCoroutine));
        }

        private IEnumerator PoseCheckerCoroutine() {
            _poseCheck = Random.Range(1, 5);
            SetPoseIndicator(_poseCheck, testPoseIndicator);
            Debug.Log($"Incoming pose: {_poseCheck}!");
            yield return new WaitForSeconds(1.5f - GameManager.Instance.timeModifier - _localTimeModifier);
            if (pose == _poseCheck) {
                Debug.Log("Well done! Next!");
                _localTimeModifier += 0.01f;
                testPoseIndicator.text = "NEXT!";
                yield return new WaitForSeconds(0.2f);
                _hasWon = true;
                _poseChecker = null;
            } else {
                _hasWon = false;
                Debug.Log("So bad... Disgusting...");
                EndGame();
            }
        }

        public override void EndGame()
        {
            if (GameHasEnded) return;
            if (!_hasWon) GameManager.Instance.lives -= 1;
            Debug.Log(_hasWon ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
