using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

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
        UpdateCardState(card1Sprite, inv.card1);
        UpdateCardState(card2Sprite, inv.card2);
        UpdateCardState(card3Sprite, inv.card3);
    }

    public void UpdateKeys(PlayerInventory inv)
    {
        UpdateKeyState(key1Sprite, inv.key1);
        UpdateKeyState(key2Sprite, inv.key2);
        UpdateKeyState(key3Sprite, inv.key3);
       
    }

    private void UpdateCardState(Image cardImage, bool isCollected)
    {
        if (cardImage != null)
        {
            SetSpriteOpacity(cardImage, isCollected ? activeOpacity : inactiveOpacity);
        }
    }

    private void UpdateKeyState(Image keyImage, bool isCollected)
    {
        if (keyImage != null)
        {
            SetSpriteOpacity(keyImage, isCollected ? activeOpacity : inactiveOpacity);
        }
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
}