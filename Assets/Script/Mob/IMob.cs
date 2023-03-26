using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMob
{
    int lifePoint { get; set; }
    float speed { get; set; }
    float energy { get; set; }
    float thirst { get; set; }
    float hunger { get; set; }
    float fatigue { get; set; }
    float fovRange { get; set; }
}
