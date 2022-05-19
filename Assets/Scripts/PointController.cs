using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour
{
   [SerializeField] private int points = 0;
   public TextMeshProUGUI pointTextComponent;

   public int Points
   {
      get => points;
      set
      {
         points = value;
         UpdateView();   
      }
   }


   private void UpdateView()
   {
      pointTextComponent.text = $"points: {points}";
   }
}
