using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCinematic : Cinematic
{
    public override CinematicScene[] Scenes { get; set; } = new CinematicScene[]
    {
        new Scene1(),
        new Scene2()
    };
}
