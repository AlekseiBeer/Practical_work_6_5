using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [System.Serializable]
    public class Instrument
    {
        public string name;
        public int[] instrumentValue;
        public TextMeshProUGUI _textInstruments;
    }
    [SerializeField] private Instrument[] instruments;

    [SerializeField] private int[] _startPins = new int[3];
    [SerializeField] private int[] _targetPins = new int[3];
    [SerializeField] private TextMeshProUGUI[] _textPins = new TextMeshProUGUI[3];
    [SerializeField] private float _timerTime = 30.0f;
    [SerializeField] private GameObject _resultWindow;
    [SerializeField] private TextMeshProUGUI _timerText, _textResult;
    private readonly int[] _pins = new int[3];
    private bool _gameOver = false;
    private float _timer;

    void Start()
    {
        ResetGame();
        InitInstrumentsText();
    }

    void Update()
    {
        if (_gameOver) return;

        UpdateTimer();

        if (CheckLockOpened())
            EndGame(true);
        else if (_timer <= 0)
            EndGame(false);
    }

    void UpdateTimer()
    {
        _timer = Math.Max(0, _timer - Time.deltaTime);
        _timerText.text = _timer.ToString("F2");
    }

    public void ResetGame()
    {
        _gameOver = false;
        _resultWindow.SetActive(false);
        _timer = _timerTime;
        for (int i = 0; i < _pins.Length; i++) 
            _pins[i] = _startPins[i];

        PrintPinsValue(); 
    }

    public void ChangePinsByInstrument(int instrumentIndex)
    {
        if (_gameOver) return;

        for (int i = 0; i < instruments[instrumentIndex].instrumentValue.Length; i++)
            _pins[i] = Mathf.Clamp(_pins[i] + instruments[instrumentIndex].instrumentValue[i], 0, 10);

        PrintPinsValue();
    }

    void PrintPinsValue()
    {
        for (int i = 0; i < _textPins.Length; i++)
            _textPins[i].text = _pins[i].ToString();
    }

    void InitInstrumentsText()
    {
        for (int i = 0; i < instruments.Length; i++)
            instruments[i]._textInstruments.text = $"{instruments[i].instrumentValue[0]}  |  {instruments[i].instrumentValue[1]}  |  {instruments[i].instrumentValue[2]}\n{instruments[i].name}";
    }

    private bool CheckLockOpened()
    {
        return _pins.SequenceEqual(_targetPins);
    }

    void EndGame(bool isWin)
    {
        _gameOver = true;
        _resultWindow.SetActive(true);
        _textResult.text = isWin ? "Вы выиграли" : "Вы проиграли";
    }
}