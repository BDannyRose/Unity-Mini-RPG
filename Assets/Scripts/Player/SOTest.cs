using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITest
{
    public void Test(GameObject gameObject);
}

[CreateAssetMenu(fileName ="SOTest", menuName ="SOTest")]
public class SOTest : ScriptableObject, ITest
{
    public virtual void Test(GameObject gameObject) {
        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
    }
}

public class SOTestInheritor : SOTest
{
    public override void Test(GameObject gameObject)
    {
        Rigidbody2D d2 = gameObject.GetComponent<Rigidbody2D>();
    }
}
