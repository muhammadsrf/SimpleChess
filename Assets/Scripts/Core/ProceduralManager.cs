using System.Collections.Generic;
using Pathfinding;
using SyarifRee.AudioNMusic;
using SyarifRee.Data;
using SyarifRee.Item;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SyarifRee.Core
{
    /// <summary>
    /// Membuat dan mengupdate bidak di dalam box (grid)
    /// </summary>
    public class ProceduralManager : MonoBehaviour
    {
        [Header("GamePlay")]
        public CountBidak countBidak;
        public bool gameOver = false;
        public int randomBidak = 0;
        public float _timerRunning;

        [Header("Setup")]
        public float _timer;
        [SerializeField] private PionData _pionData;
        [SerializeField] GameObject _emptyPrefabs;
        [SerializeField] GameObject _bidakPrefabs;
        [SerializeField] GameObject _attackZonePrefabs;
        [SerializeField] GameObject _textAppearPrefabs;
        [SerializeField] SpriteRenderer _nextBoxIcon;
        [SerializeField] Text _scoreText;
        [SerializeField] private AstarPath _pathfinder;
        [SerializeField] private Animator _animatorWindowLose;

        private GridGraph _gridGraph;
        private int _score;
        private const int width = 9;
        private const int height = 6;

        private void Start()
        {
            gameOver = true;

            InitialisationScore();
            InitialisationBox();
            StartTimer();
        }

        private void StartTimer()
        {
            _timerRunning = _timer;
            gameOver = false;
        }

        private void FixedUpdate()
        {
            if (gameOver == false)
            {
                _timerRunning -= Time.deltaTime;

                if (_timerRunning <= 0)
                {
                    GameOver();
                }
            }
        }

        private void InitialisationScore()
        {
            // intial score to 0
            _score = 0;
            _scoreText.text = _score.ToString();
        }

        private void InitialisationBox()
        {
            // processing to generate empty box
            _gridGraph = AstarPath.active.data.gridGraph; // get grid graph, reference from a * pathfinder

            // processing to instantiate new box empty
            foreach (var node in _gridGraph.nodes)
            {
                var newBox = Instantiate(_emptyPrefabs,
                              (Vector3)node.position,
                              Quaternion.identity,
                              transform
                              );

                var itemController = newBox.GetComponent<ItemController>();
                itemController.SetNode(node);
                itemController.pionName = _pionData.pions[randomBidak].id;
                itemController.proceduralManager = this;
            }

            // update next box icon
            randomBidak = Random.Range(0, _pionData.pions.Count);
            _nextBoxIcon.sprite = _pionData.pions[randomBidak].icon;
        }

        /// <summary>
        /// Update score text setelah berhasil drop.
        /// </summary>
        public void UpdateScore(Transform bidakTransform)
        {
            _score = _score + _pionData.pions[randomBidak].value;
            _scoreText.text = _score.ToString();

            // effect text appearing
            var newText = Instantiate(_textAppearPrefabs);

            newText.GetComponent<ScoreAppear>().SetText(_pionData.pions[randomBidak].value, bidakTransform.position);
        }

        /// <summary>
        /// Update next box setelah digunakan.
        /// </summary>
        public void UpdateNextBox()
        {
            randomBidak = Random.Range(0, _pionData.pions.Count);

            // update next box icon
            _nextBoxIcon.sprite = _pionData.pions[randomBidak].icon;
        }

        public GameObject CreateBidak(Vector3 position, Transform parent, GridNode node)
        {
            // checking before spawn new Block
            // rules for every Block
            switch (_pionData.pions[randomBidak].id)
            {
                case "Rock":
                    GridNodeBase currentNode = node;

                    // atas direction
                    for (int i = 0; i < height; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(2);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);

                                return null;
                            }
                        }
                    }

                    currentNode = node;
                    // bawah direction
                    for (int i = 0; i < height; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(0);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);
                                return null;
                            }
                        }
                    }

                    // kanan direction
                    currentNode = node;
                    for (int i = 0; i < width; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(1);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);
                                return null;
                            }
                        }
                    }

                    // kiri direction
                    currentNode = node;
                    for (int i = 0; i < width; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(3);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);
                                return null;
                            }
                        }
                    }

                    break;

                case "Bishop":
                    currentNode = node;

                    // atas-kanan direction
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(5);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);

                                return null;
                            }
                        }
                    }

                    currentNode = node;
                    // bawah-kanan direction
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(4);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);

                                return null;
                            }
                        }
                    }

                    // bawah kiri direction
                    currentNode = node;
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(7);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);

                                return null;
                            }
                        }
                    }

                    // atas kiri direction
                    currentNode = node;
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(6);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (!currentNode.Walkable)
                            {
                                GameOver(_pionData.pions[randomBidak].id, node);

                                return null;
                            }
                        }
                    }

                    break;

                case "Knight":
                    currentNode = node;
                    // L1 - dimulai dari L atas kanan atas
                    if (
                        currentNode.GetNeighbourAlongDirection(2) != null
                        && currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(5) != null
                        && !currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(5).Walkable
                        )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    // L2
                    if (
                            currentNode.GetNeighbourAlongDirection(1) != null
                            && currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(5) != null
                            && !currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(5).Walkable
                            )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    // L3
                    if (
                        currentNode.GetNeighbourAlongDirection(1) != null
                        && currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(4) != null
                        && !currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(4).Walkable)
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    // L4
                    if (
                        currentNode.GetNeighbourAlongDirection(0) != null
                        && currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(4) != null
                        && !currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(4).Walkable
                        )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    // L5
                    if (
                        currentNode.GetNeighbourAlongDirection(0) != null
                        && currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(7) != null
                        && !currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(7).Walkable)
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    // L6
                    if (
                        currentNode.GetNeighbourAlongDirection(7) != null
                        && currentNode.GetNeighbourAlongDirection(7).GetNeighbourAlongDirection(3) != null
                        && !currentNode.GetNeighbourAlongDirection(7).GetNeighbourAlongDirection(3).Walkable
                        )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    // L7
                    if (
                        currentNode.GetNeighbourAlongDirection(3) != null
                        && currentNode.GetNeighbourAlongDirection(3).GetNeighbourAlongDirection(6) != null
                        && !currentNode.GetNeighbourAlongDirection(3).GetNeighbourAlongDirection(6).Walkable)
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    // L8
                    if (
                        currentNode.GetNeighbourAlongDirection(2) != null
                        && currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(6) != null
                        && !currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(6).Walkable)
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }

                    break;

                case "Dragon":
                    currentNode = node;

                    if (
                        currentNode.GetNeighbourAlongDirection(6) != null
                        && !currentNode.GetNeighbourAlongDirection(6).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(2) != null
                        && !currentNode.GetNeighbourAlongDirection(2).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(5) != null
                        && !currentNode.GetNeighbourAlongDirection(5).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(1) != null
                        && !currentNode.GetNeighbourAlongDirection(1).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(4) != null
                        && !currentNode.GetNeighbourAlongDirection(4).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(0) != null
                        && !currentNode.GetNeighbourAlongDirection(0).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(7) != null
                        && !currentNode.GetNeighbourAlongDirection(7).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    else if (
                        currentNode.GetNeighbourAlongDirection(3) != null
                        && !currentNode.GetNeighbourAlongDirection(3).Walkable
                    )
                    {
                        GameOver(_pionData.pions[randomBidak].id, node);
                        return null;
                    }
                    break;
            }

            var newBidak = Instantiate(_bidakPrefabs, position, Quaternion.identity, parent);

            node.Walkable = false;

            newBidak.GetComponent<SpriteRenderer>().sprite = _pionData.pions[randomBidak].icon;

            CounterNewBidak(newBidak);

            // renew timer
            _timerRunning = _timer;

            return newBidak;
        }

        private void CounterNewBidak(GameObject newBidak)
        {
            // add number counter bidak created
            switch (_pionData.pions[randomBidak].id)
            {
                case "Rock":
                    countBidak.rock += 1;
                    countBidak.rockList.Add(newBidak);
                    if (countBidak.rock == 3)
                    {
                        // hapus semua bidak rock
                        foreach (var bidak in countBidak.rockList)
                        {
                            bidak.transform.parent.GetComponent<ItemController>().bidak = null;
                            bidak.transform.parent.GetComponent<ItemController>().GetNode().Walkable = true;
                            bidak.GetComponent<Disappear>().DisappearNow();

                            // play a sound effect
                            AudioManager.instance.PlayClip(AudioID.same3);
                        }

                        // reset bidak
                        countBidak.rockList.Clear();
                        countBidak.rock = 0;
                    }
                    break;
                case "Bishop":
                    countBidak.bishop += 1;
                    countBidak.bishopList.Add(newBidak);
                    if (countBidak.bishop == 3)
                    {
                        // hapus semua bidak bishop
                        foreach (var bidak in countBidak.bishopList)
                        {
                            bidak.transform.parent.GetComponent<ItemController>().bidak = null;
                            bidak.transform.parent.GetComponent<ItemController>().GetNode().Walkable = true;
                            bidak.GetComponent<Disappear>().DisappearNow();

                            // play a sound effect
                            AudioManager.instance.PlayClip(AudioID.same3);
                        }

                        // reset bidak
                        countBidak.bishopList.Clear();
                        countBidak.bishop = 0;
                    }
                    break;

                case "Knight":
                    countBidak.knight += 1;
                    countBidak.knightList.Add(newBidak);
                    if (countBidak.knight == 3)
                    {
                        // hapus semua bidak knight
                        foreach (var bidak in countBidak.knightList)
                        {
                            bidak.transform.parent.GetComponent<ItemController>().bidak = null;
                            bidak.transform.parent.GetComponent<ItemController>().GetNode().Walkable = true;
                            bidak.GetComponent<Disappear>().DisappearNow();

                            // play a sound effect
                            AudioManager.instance.PlayClip(AudioID.same3);
                        }

                        // reset bidak
                        countBidak.knightList.Clear();
                        countBidak.knight = 0;
                    }
                    break;
                case "Dragon":
                    countBidak.dragon += 1;
                    countBidak.dragonList.Add(newBidak);
                    if (countBidak.dragon == 3)
                    {
                        // hapus semua bidak dragon
                        foreach (var bidak in countBidak.dragonList)
                        {
                            bidak.transform.parent.GetComponent<ItemController>().bidak = null;
                            bidak.transform.parent.GetComponent<ItemController>().GetNode().Walkable = true;
                            bidak.GetComponent<Disappear>().DisappearNow();

                            // play a sound effect
                            AudioManager.instance.PlayClip(AudioID.same3);
                        }

                        // reset bidak
                        countBidak.dragonList.Clear();
                        countBidak.dragon = 0;
                    }
                    break;
            }
        }

        public void GameOver()
        {
            WrongSound();
            _animatorWindowLose.Play("pop-up", 0);
            gameOver = true;
        }

        private void GameOver(string blockName, GridNodeBase node)
        {
            WrongSound();
            LocatingAttackZone(blockName, node);

            // locating transparent Block on the forbidden box
            var transBlock = Instantiate(_bidakPrefabs, (Vector3)node.position, Quaternion.identity, transform);
            transBlock.GetComponent<SpriteRenderer>().sprite = _pionData.pions[randomBidak].icon;
            transBlock.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            transBlock.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);

            _animatorWindowLose.Play("pop-up", 0);
            gameOver = true;
        }

        /// <summary>
        /// This is for locating box of attack zone on Block triggered
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="node"></param>
        private void LocatingAttackZone(string blockName, GridNodeBase node)
        {
            switch (blockName)
            {
                case "Rock":
                    GridNodeBase currentNode = node;

                    // atas direction
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(2);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }

                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }

                    currentNode = node;
                    // bawah direction
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(0);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }

                    // kanan direction
                    currentNode = node;
                    for (int i = 0; i < width - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(1);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }

                    // kiri direction
                    currentNode = node;
                    for (int i = 0; i < width - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(3);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }
                    break;

                case "Bishop":
                    currentNode = node;

                    // atas-kanan direction
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(5);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }

                    currentNode = node;
                    // bawah-kanan direction
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(4);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }

                    // bawah kiri direction
                    currentNode = node;
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(7);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }

                    // atas kiri direction
                    currentNode = node;
                    for (int i = 0; i < height - 1; i++)
                    {
                        currentNode = currentNode.GetNeighbourAlongDirection(6);

                        if (currentNode == null)
                        {
                            break;
                        }
                        else if (currentNode != null)
                        {
                            if (currentNode.Walkable) { continue; }
                            var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.position, Quaternion.identity, transform);
                        }
                    }
                    break;

                case "Knight":
                    currentNode = node;
                    //L1
                    if (
                        currentNode.GetNeighbourAlongDirection(2) != null
                        && currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(5) != null
                        && !currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(5).Walkable
                        )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(5).position, Quaternion.identity, transform);
                    }

                    // L2
                    if (
                        currentNode.GetNeighbourAlongDirection(1) != null
                        && currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(5) != null
                        && !currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(5).Walkable
                        )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(5).position, Quaternion.identity, transform);
                    }

                    // L3
                    if (
                        currentNode.GetNeighbourAlongDirection(1) != null
                        && currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(4) != null
                        && !currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(4).Walkable)
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(1).GetNeighbourAlongDirection(4).position, Quaternion.identity, transform);
                    }

                    // L4
                    if (
                        currentNode.GetNeighbourAlongDirection(0) != null
                        && currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(4) != null
                        && !currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(4).Walkable
                        )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(4).position, Quaternion.identity, transform);
                    }

                    // L5
                    if (
                        currentNode.GetNeighbourAlongDirection(0) != null
                        && currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(7) != null
                        && !currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(7).Walkable)
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(0).GetNeighbourAlongDirection(7).position, Quaternion.identity, transform);
                    }

                    // L6
                    if (
                        currentNode.GetNeighbourAlongDirection(7) != null
                        && currentNode.GetNeighbourAlongDirection(7).GetNeighbourAlongDirection(3) != null
                        && !currentNode.GetNeighbourAlongDirection(7).GetNeighbourAlongDirection(3).Walkable
                        )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(7).GetNeighbourAlongDirection(3).position, Quaternion.identity, transform);
                    }

                    // L7
                    if (
                        currentNode.GetNeighbourAlongDirection(3) != null
                        && currentNode.GetNeighbourAlongDirection(3).GetNeighbourAlongDirection(6) != null
                        && !currentNode.GetNeighbourAlongDirection(3).GetNeighbourAlongDirection(6).Walkable)
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(3).GetNeighbourAlongDirection(6).position, Quaternion.identity, transform);
                    }

                    // L8
                    if (
                        currentNode.GetNeighbourAlongDirection(2) != null
                        && currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(6) != null
                        && !currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(6).Walkable)
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(2).GetNeighbourAlongDirection(6).position, Quaternion.identity, transform);
                    }
                    break;

                case "Dragon":
                    currentNode = node;

                    if (
                        currentNode.GetNeighbourAlongDirection(6) != null
                        && !currentNode.GetNeighbourAlongDirection(6).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(6).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(2) != null
                        && !currentNode.GetNeighbourAlongDirection(2).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(2).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(5) != null
                        && !currentNode.GetNeighbourAlongDirection(5).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(5).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(1) != null
                        && !currentNode.GetNeighbourAlongDirection(1).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(1).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(4) != null
                        && !currentNode.GetNeighbourAlongDirection(4).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(4).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(0) != null
                        && !currentNode.GetNeighbourAlongDirection(0).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(0).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(7) != null
                        && !currentNode.GetNeighbourAlongDirection(7).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(7).position, Quaternion.identity, transform);
                    }

                    if (
                        currentNode.GetNeighbourAlongDirection(3) != null
                        && !currentNode.GetNeighbourAlongDirection(3).Walkable
                    )
                    {
                        var attackZone = Instantiate(_attackZonePrefabs, (Vector3)currentNode.GetNeighbourAlongDirection(3).position, Quaternion.identity, transform);
                    }
                    break;
            }
        }

        private void WrongSound()
        {
            AudioManager.instance.PlayClip(AudioID.wrong);
        }

        public void RetryButton()
        {
            SceneManager.LoadScene("GamePlay");
        }
    }


    /// <summary>
    /// Class untuk menghitung bidak di dalam box
    /// </summary>
    [System.Serializable]
    public class CountBidak
    {
        public int rock;
        public List<GameObject> rockList = new List<GameObject>();
        public int bishop;
        public List<GameObject> bishopList = new List<GameObject>();
        public int knight;
        public List<GameObject> knightList = new List<GameObject>();
        public int dragon;
        public List<GameObject> dragonList = new List<GameObject>();
    }
}