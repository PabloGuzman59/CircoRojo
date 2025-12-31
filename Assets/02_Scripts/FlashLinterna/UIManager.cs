using UnityEngine;
using UnityEngine.UI;
using TMPro; // Agrega esta línea
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("TextMesh Pro para mensajes")]
    public TMP_Text messageText; // Referencia al TextMesh Pro UI

    [Header("Card Sprites")]
    public Image card1Sprite;
    public Image card2Sprite;
    public Image card3Sprite;

    [Header("Key Sprites")]
    public Image key1Sprite;
    public Image key2Sprite;
    public Image key3Sprite;

    [Header("Sprites para cada elemento")]
    public Sprite card1SpriteImage;
    public Sprite card2SpriteImage;
    public Sprite card3SpriteImage;
    public Sprite key1SpriteImage;
    public Sprite key2SpriteImage;
    public Sprite key3SpriteImage;

    [Header("Opacity Settings")]
    [Range(0, 1)]
    public float activeOpacity = 1f;
    [Range(0, 1)]
    public float inactiveOpacity = 0.3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Inicializar todos los sprites
        InitializeUI();

        // Ocultar mensaje al inicio
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    private void InitializeUI()
    {
        // Asignar sprites iniciales a las tarjetas
        if (card1Sprite != null && card1SpriteImage != null)
        {
            card1Sprite.sprite = card1SpriteImage;
            SetSpriteOpacity(card1Sprite, inactiveOpacity);
        }

        if (card2Sprite != null && card2SpriteImage != null)
        {
            card2Sprite.sprite = card2SpriteImage;
            SetSpriteOpacity(card2Sprite, inactiveOpacity);
        }

        if (card3Sprite != null && card3SpriteImage != null)
        {
            card3Sprite.sprite = card3SpriteImage;
            SetSpriteOpacity(card3Sprite, inactiveOpacity);
        }

        // Asignar sprites iniciales a las llaves
        if (key1Sprite != null && key1SpriteImage != null)
        {
            key1Sprite.sprite = key1SpriteImage;
            SetSpriteOpacity(key1Sprite, inactiveOpacity);
        }

        if (key2Sprite != null && key2SpriteImage != null)
        {
            key2Sprite.sprite = key2SpriteImage;
            SetSpriteOpacity(key2Sprite, inactiveOpacity);
        }

        if (key3Sprite != null && key3SpriteImage != null)
        {
            key3Sprite.sprite = key3SpriteImage;
            SetSpriteOpacity(key3Sprite, inactiveOpacity);
        }
    }

    public void UpdateCards(PlayerInventory inv)
    {
        bool card1Collected = UpdateCardState(card1Sprite, inv.card1, "card1");
        bool card2Collected = UpdateCardState(card2Sprite, inv.card2, "card2");
        bool card3Collected = UpdateCardState(card3Sprite, inv.card3, "card3");

        // Mostrar mensaje para tarjetas
        if (card1Collected) ShowMessage("¡Conseguiste la tarjeta ROJA!");
        if (card2Collected) ShowMessage("¡Conseguiste la tarjeta MORADA!");
        if (card3Collected) ShowMessage("¡Conseguiste la tarjeta VERDE!");
    }

    public void UpdateKeys(PlayerInventory inv)
    {
        bool key1Collected = UpdateKeyState(key1Sprite, inv.key1, "key1");
        bool key2Collected = UpdateKeyState(key2Sprite, inv.key2, "key2");
        bool key3Collected = UpdateKeyState(key3Sprite, inv.key3, "key3");

        // Mostrar mensaje para llaves
        if (key1Collected) ShowMessage("¡Conseguiste la llave ROJA!\nBusca la puerta del mismo color.");
        if (key2Collected) ShowMessage("¡Conseguiste la llave MORADA!\nBusca la puerta del mismo color.");
        if (key3Collected) ShowMessage("¡Conseguiste la llave VERDE!\nBusca la puerta del mismo color.");
    }

    private bool UpdateCardState(Image cardImage, bool isCollected, string cardName)
    {
        bool wasAlreadyCollected = false;

        if (cardImage != null)
        {
            // Verificar si ya estaba recolectada
            wasAlreadyCollected = cardImage.color.a == activeOpacity;

            SetSpriteOpacity(cardImage, isCollected ? activeOpacity : inactiveOpacity);
        }

        // Retornar si acaba de ser recolectada
        return isCollected && !wasAlreadyCollected;
    }

    private bool UpdateKeyState(Image keyImage, bool isCollected, string keyName)
    {
        bool wasAlreadyCollected = false;

        if (keyImage != null)
        {
            // Verificar si ya estaba recolectada
            wasAlreadyCollected = keyImage.color.a == activeOpacity;

            SetSpriteOpacity(keyImage, isCollected ? activeOpacity : inactiveOpacity);
        }

        // Retornar si acaba de ser recolectada
        return isCollected && !wasAlreadyCollected;
    }

    private void SetSpriteOpacity(Image image, float opacity)
    {
        if (image != null)
        {
            Color currentColor = image.color;
            currentColor.a = opacity;
            image.color = currentColor;
        }
    }

    // Método para mostrar mensajes
    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;

            // Opcional: Ocultar el mensaje después de 2 segundos
            Invoke("ClearMessage", 2f);
        }
    }

    // Método para limpiar el mensaje
    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }
}