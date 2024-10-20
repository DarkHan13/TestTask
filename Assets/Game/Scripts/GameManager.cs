using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TrunkChecker trunkChecker;
    [SerializeField] private int numberObjectToWin = 2;
    [SerializeField] private TextMeshProUGUI tmp;

    private int _number;

    private int Number
    {
        get => _number;
        set
        {
            _number = value;
            if (!_isGameOver) tmp.text = $"Осталось {_number} из {numberObjectToWin}";
        }
    }
    
    private bool _isGameOver;


    private void Start()
    {
        tmp.text = "Нажмите на Tab";

        var objectGrabber = FindAnyObjectByType<ObjectGrabber>();
        void OnTabPressed()
        {
            tmp.text = "Возьмите объекты (лкм) и положите в кузов";
            objectGrabber.OnDebugMode -= OnTabPressed;
        }

        objectGrabber.OnDebugMode += OnTabPressed;
    }


    private void OnEnable()
    {
        trunkChecker.OnObjectEnter += IncrementObject;
        trunkChecker.OnObjectExit += DecrementObject;
    }

    private void OnDisable()
    {
        trunkChecker.OnObjectEnter -= IncrementObject;
        trunkChecker.OnObjectExit -= DecrementObject;
    }

    private void IncrementObject()
    {
        Number++;
        if (!_isGameOver && _number == numberObjectToWin)
        {
            _isGameOver = true;
            tmp.text = "Победа";
        }
    }

    private void DecrementObject()
    {
        Number--;
    }
}
