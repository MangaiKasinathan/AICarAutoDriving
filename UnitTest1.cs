using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


[TestClass]
public class AutoDrivingCarTests
{
    [TestMethod]
    public void TestCollision()
    {
        var cars = new List<Car>
        {
            new Car("A", 1, 2, 'N', "FFRFFFFRRL", 10, 10),
            new Car("B", 7, 8, 'W', "FFLFFFFFFF", 10, 10)
        };

        var result = AutoDrivingCar.CheckForCollisions(cars);

        Assert.IsNotNull(result);
        Assert.AreEqual("A", result.Item1);
        Assert.AreEqual("B", result.Item2);
        Assert.AreEqual(7, result.Item3);
    }

    [TestMethod]
    public void TestNoCollision()
    {
        var cars = new List<Car>
        {
            new Car("A", 1, 2, 'N', "FFRFFFFRRL", 10, 10),
            new Car("B", 7, 8, 'W', "FFLFFFFFFF", 10, 10)
        };

        var result = AutoDrivingCar.CheckForCollisions(cars);

        Assert.IsNull(result);
    }
}
