using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUi : MonoBehaviour {

    [SerializeField] private TMP_Text _labelScore;
    [SerializeField] private RectTransform _health;
    [SerializeField] private RectTransform[] _additHealth;
    [SerializeField] private GameObject countdown;

    private int _score = 0;

    private void Awake() {
        AddScore(0);
        UpdateHealth(3);
    }

    public GameObject GetCountdown()
    {
        return countdown;
    }

    public void AddScore(int s) {
        _score += s;
        _labelScore.text = _score.ToString();
    }

    public void UpdateHealth(int h) {
        for (int i = 0; i < _health.childCount; i++) {
            _health.GetChild(i).gameObject.SetActive(i < h);
        }
    }
}
