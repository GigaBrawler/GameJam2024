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
        private int _poseCheck;
        private Coroutine _poseChecker;
        private float _localTimeModifier;
        private bool _hasWon;

        [Header("Graphics")]
        [SerializeField] private SpriteRenderer strongman;
        [SerializeField] private SpriteRenderer mime;
        [SerializeField] private Sprite mime1;
        [SerializeField] private Sprite mime2;
        [SerializeField] private Sprite pose1;
        [SerializeField] private Sprite pose2;
        [SerializeField] private Sprite pose3;
        [SerializeField] private Sprite pose4;
        [SerializeField] private Sprite poseIdle;
        
        [Header("Sound")]
        [SerializeField] private AudioSource pos1;
        [SerializeField] private AudioSource pos2;
        [SerializeField] private AudioSource pos3;
        [SerializeField] private AudioSource pos4;
        
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
                1 => "\u2191",
                2 => "\u2192",
                3 => "\u2190",
                4 => "\u2193",
                _ => poseIndicator.text
            };

        private void SetPoseSprite(int nextPose)
        {
            switch (nextPose)
            {
                case 1:
                    pos1.Play();
                    strongman.sprite = pose1;
                    break;
                case 2:
                    pos2.Play();
                    strongman.sprite = pose2;
                    break;
                case 3:
                    pos3.Play();
                    strongman.sprite = pose3;
                    break;
                case 4:
                    pos4.Play();
                    strongman.sprite = pose4;
                    break;
                case 0:
                    strongman.sprite = poseIdle;
                    break;
            }
        }

        private void Update()
        {
            if (gameHasEnded) return;
            SetPoseSprite(pose);
            GetPose();
            _poseChecker ??= StartCoroutine(nameof(PoseCheckerCoroutine));
        }

        private IEnumerator PoseCheckerCoroutine() {
            _poseCheck = Random.Range(1, 5);
            SetPoseIndicator(_poseCheck, testPoseIndicator);
            Debug.Log($"Incoming pose: {_poseCheck}!");
            yield return new WaitForSeconds(1f - 
                (GameManager.Instance.timeModifier > 1f ? 0 : GameManager.Instance.timeModifier / 2f) - (_localTimeModifier) + 0.5f);
            if (pose == _poseCheck) {
                Debug.Log("Well done! Next!");
                _localTimeModifier += 0.01f;
                mime.sprite = mime2;
                testPoseIndicator.text = "";
                yield return new WaitForSeconds(0.2f);
                _hasWon = true;
                _poseChecker = null;
                mime.sprite = mime1;
            } else {
                _hasWon = false;
                Debug.Log("So bad... Disgusting...");
                EndGame();
            }
        }

        public override void EndGame()
        {
            if (gameHasEnded) return;
            StopAllCoroutines();
            if (!_hasWon)
            {
                GameManager.Instance.boo.Play();
                GameManager.Instance.lives -= 1;
            }
            else
            {
                GameManager.Instance.yay.Play();
                GameManager.Instance.gamesWon++;
            }
            Debug.Log(_hasWon ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
