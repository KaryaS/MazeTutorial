using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This allows Unity to save our custom struct
[System.Serializable]
public struct IntVector2
{

    //Allow manipulation of Vectors containing integers
    public int x, z;
    //Constructor method to allow us to define our methods
    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    //Providing an overload function on the addition operator as Vectors are 
    //added differently than single integer numbers
    public static IntVector2 operator +(IntVector2 a_VecA, IntVector2 a_VecB)
    {
        IntVector2 vTemp;
        //Adds the X values of the vectors together
        vTemp.x = a_VecA.x + a_VecB.x;
        //Adds the Z values of the vectors together
        vTemp.z = a_VecA.z + a_VecB.z;
        return vTemp;
    }
}