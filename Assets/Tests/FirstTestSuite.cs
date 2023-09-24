using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FirstTestSuite
{
    public void StartingStatusTest()
    {
        Assert.IsTrue(Status.Current == "starting");
    }

    [Test]
    public void IsGameManagerHasNotLoadedItemsOnStart()
    {
        Assert.IsTrue(Item.items.Count==0);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator IsGameManagerHasLoadedItemsAfterFrame()
    {
        Item.LoadItems();
        yield return null;
        Assert.IsFalse(Item.items.Count == 0);
    }
}
