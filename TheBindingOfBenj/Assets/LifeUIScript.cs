using GameLibrary;
using UnityEngine;
using UnityEngine.UI;

public class LifeUIScript : MonoBehaviour
{
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;
    [SerializeField] private GameObject _heartPrefab;

    private void Awake()
    {
        EventManager.Instance.AddListener<OnPlayerHurted>(PlayerHurted);
        EventManager.Instance.AddListener<OnGameStarted>(GameStarted);
    }

    private void PlayerHurted(OnPlayerHurted e)
    {
        DisplayLife(e.MaxHealth, e.CurrentHealth);
    }

    private void GameStarted(OnGameStarted e)
    {
        DisplayLife(e.Status.MaxHealth, e.Status.CurrentHealth);
    }


    private void DisplayLife(float maxHealth, float currentHealth)
    {

        // Très dégueulasse, il y a probablement plus simple
        foreach (Transform child in transform)
            Destroy(child.gameObject);


        for (int i = 0; i < maxHealth; i++)
        {
            _heartPrefab.GetComponent<Image>().sprite = i >= currentHealth ? _emptyHeartSprite : _fullHeartSprite;
            Instantiate(_heartPrefab, transform);
        }
    }
}
