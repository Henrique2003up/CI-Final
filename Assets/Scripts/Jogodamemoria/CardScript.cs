using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Certifica-te que isto está presente para usar Image

public class CardScript : MonoBehaviour
{
    public Image Below; // A imagem que mostra a cor ou o sprite da carta
    public Image Cover; // A imagem que cobre a carta
    public int CardID;  // O ID que identifica o par de cartas

    public void Awake()
    {
        // Garante que a imagem de baixo e a capa estão ativas no início.
        // A capa deve estar ativa para que a carta comece virada para baixo.
        Below.gameObject.SetActive(true);
        Cover.gameObject.SetActive(true);
    }

    // Define a cor da imagem de baixo
    public void SetBelowColor(Color newColor)
    {
        Below.color = newColor;
        // Se a cor for o critério de match, podes definir o CardID com base na cor
        // (embora comparar as cores diretamente já funcione para este caso)
    }

    // Define o sprite da imagem de baixo
    public void SetBelowImage(Sprite newImage)
    {
        Below.color = Color.white; // Assegura que a cor é branca para o sprite aparecer sem tintura
        Below.sprite = newImage;
    }

    // Define o ID da carta (usado para comparar pares de sprites)
    public void SetCardID(int id)
    {
        CardID = id;
    }

    // Desativa a capa, revelando a imagem de baixo
    public void DisableCover()
    {
        Cover.gameObject.SetActive(false);
    }

    // Ativa a capa, escondendo a imagem de baixo
    public void EnableCover()
    {
        Cover.gameObject.SetActive(true);
    }
}
