using System.Collections.Generic;
using UnityEngine;

public class AdjacencyConstraint : MonoBehaviour
{
    [Header("Exceptions aux règles d'adjacences par défaut:\nIci on ne peut pas imposer une proba \ncar le nombre de chunk peut changer. \n(Proba par defaut = 1/nombre de chunks)\nOn indique donc combien de fois un chunk est plus ou moins \nprobable d'apparaitre \n(2 fois plus, 1.5 fois plus, 0 fois plus ...)")]
    public List<Generation.Constraint> constraints = new();
}
