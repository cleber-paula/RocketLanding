using System.Collections;

namespace RocketLanding.Tests;

[TestFixture]
public class RocketLandingTests
{
    private IArea _landingAreaTestOutOfPlatform;
    private IArea _landingAreaTestOkForLanding;

    [SetUp]
    public void Setup()
    {
        _landingAreaTestOutOfPlatform = new Area(3, 3, 1, 1, 1, 1);
        _landingAreaTestOutOfPlatform = new Area(3, 3, 1, 1, 1, 1);


        _landingAreaTestOkForLanding = new Area(11, 11, 1, 1, 9, 9);

        int rocketId = 0;

        for (int i = 3; i <= 7; i++) {
            _landingAreaTestOkForLanding.CheckLanding(++rocketId, i, 3);
            _landingAreaTestOkForLanding.CheckLanding(++rocketId, i, 7);
            if(i != 3 && i != 7)
            {
                _landingAreaTestOkForLanding.CheckLanding(++rocketId, 3, i);
                _landingAreaTestOkForLanding.CheckLanding(++rocketId, 7, i);
            }
        }

    }

    [Test]
    [TestCase(10, 10, 10, 5, 1, 1)]
    [TestCase(10, 10, 5, 10, 1, 1)]
    [TestCase(10, 10, 9, 9, 2, 1)]
    [TestCase(10, 10, 9, 9, 1, 2)]
    [TestCase(10, 10, -1, 9, 2, 2)]
    [TestCase(10, 10, 9, -1, 2, 2)]
    public void TestInvalidPlatformDefinition(int rows, int columns, int platformFirstLine, int platformFirstColumn, int platformRows, int platformColumns)
    {
        Assert.Throws(
            Is.TypeOf<Exception>().And.Message.EqualTo("Platform out of boundaries"),
            () => new Area(rows, columns, platformFirstLine, platformFirstColumn, platformRows, platformColumns));
    }

    [Test]
    [TestCase(0, 10, 10, 5, 1, 1)]
    [TestCase(10, 0, 5, 10, 1, 1)]
    public void TestInvalidAreaSize(int rows, int columns, int platformFirstLine, int platformFirstColumn, int platformRows, int platformColumns)
    {
        Assert.Throws(
            Is.TypeOf<Exception>().And.Message.EqualTo("Invalid area size"),
            () => new Area(rows, columns, platformFirstLine, platformFirstColumn, platformRows, platformColumns));
    }

    [Test]
    [TestCase(10, 10, 10, 5, 0, 1)]
    [TestCase(10, 10, 5, 10, 1, 0)]
    public void TestInvalidPlatformSize(int rows, int columns, int platformFirstLine, int platformFirstColumn, int platformRows, int platformColumns)
    {
        Assert.Throws(
            Is.TypeOf<Exception>().And.Message.EqualTo("Invalid platform size"),
            () => new Area(rows, columns, platformFirstLine, platformFirstColumn, platformRows, platformColumns));
    }

    [Test]
    [TestCase(0, 0), TestCase(0, 1), TestCase(0, 2)]
    [TestCase(1, 0), TestCase(1, 2)]
    [TestCase(2, 0), TestCase(2, 1), TestCase(2, 2)]
    [TestCase(-1, -1), TestCase(3, 3)]
    public void TestOutOfPlatform(int row, int column)
    {
        string result = _landingAreaTestOutOfPlatform.CheckLanding(1, row, column);
        Assert.That(result, Is.EqualTo("out of platform"));
    }

    [TestCaseSource(typeof(RocketLandingDataTests), nameof(RocketLandingDataTests.OkForLandingTests))]
    public void TestOkForLanding(int rocketId, int row, int column)
    {
        string result = _landingAreaTestOkForLanding.CheckLanding(rocketId, row, column);
        Assert.That(result, Is.EqualTo("ok for landing"));
    }

    [TestCaseSource(typeof(RocketLandingDataTests), nameof(RocketLandingDataTests.ClashTests))]
    public void TestClash(int usedRow, int usedColumn, int row, int column)
    {
        IArea landingArea = new Area(5, 5, 1, 1, 3, 3);
        string result = landingArea.CheckLanding(1, usedRow, usedColumn);
        Assert.That(result, Is.EqualTo("ok for landing"));

        result = landingArea.CheckLanding(2, row, column);
        Assert.That(result, Is.EqualTo("clash"));
    }

    [Test]
    public void TestLastCheck()
    {
        IArea landingArea = new Area(5, 5, 1, 1, 3, 3);
        string result;
        result = landingArea.CheckLanding(1, 1, 1);
        Assert.That(result, Is.EqualTo("ok for landing"));

        result = landingArea.CheckLanding(2, 2, 2);
        Assert.That(result, Is.EqualTo("clash"));

        result = landingArea.CheckLanding(1, 0, 0);
        Assert.That(result, Is.EqualTo("out of platform"));

        result = landingArea.CheckLanding(2, 2, 2);
        Assert.That(result, Is.EqualTo("ok for landing"));
    }

}

public class RocketLandingDataTests
{
    public static IEnumerable ClashTests
    {
        get
        {
            for(int usedRow = 1; usedRow <= 3; usedRow++) 
                for(int usedColumn = 1; usedColumn <= 3; usedColumn++) 
                    for(int row = Math.Max(usedRow - 1, 1); row <= Math.Min(usedRow + 1, 3); row++)
                        for(int column = Math.Max(usedColumn - 1, 1); column <= Math.Min(usedColumn + 1, 3); column++) 
                            yield return new TestCaseData(usedRow, usedColumn, row, column);
                        
        }
    }

    public static IEnumerable OkForLandingTests
    {
        get
        {
            for (int i = 1; i <= 9; i++)
            {
                yield return new TestCaseData(999, i, 1);
                yield return new TestCaseData(999, i, 9);

                if (i != 1 && i != 9)
                {
                    yield return new TestCaseData(999, 1, i);
                    yield return new TestCaseData(999, 9, i);
                }
            }
            yield return new TestCaseData(999, 5, 5);
        }
    }

}
