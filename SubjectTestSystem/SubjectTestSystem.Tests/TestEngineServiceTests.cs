using SubjectTestSystem.Shared.Models;
using SubjectTestSystem.Shared.Services;
using Xunit;

namespace SubjectTestSystem.Tests;

public class TestEngineServiceTests
{
    private readonly TestEngineService _sut = new();

    [Fact]
    public void GenerateTest_ShouldSelectCorrectDistribution()
    {
        // Arrange
        var pool = CreateMockPool(100, 100);

        // Act
        var result = _sut.GenerateTest(pool, professionalCount: 60, commonCount: 20).ToList();

        // Assert
        Assert.Equal(80, result.Count);
        Assert.Equal(60, result.Count(q => q.OriginalQuestion.Category == "專業科目"));
        Assert.Equal(20, result.Count(q => q.OriginalQuestion.Category == "共同科目"));
    }

    [Fact]
    public void GenerateTest_ShuffledOptions_ShouldMaintainCorrectAnswerMapping()
    {
        // Arrange
        var question = new QuestionModel(
            "專業科目", "Test", 1, "Which is correct?", 
            ["Option A", "Option B", "Option C", "Option D"], 
            2 // Correct is "Option B"
        );
        var pool = new[] { question };

        // Act
        // Run multiple times to ensure randomness doesn't hide bugs
        for (int i = 0; i < 10; i++)
        {
            var result = _sut.GenerateTest(pool, 1, 0).First();
            
            // Assert
            var expectedText = "Option B";
            var actualTextAtCorrectIndex = result.ShuffledOptions[result.CorrectAnswerIndex].Text;
            
            Assert.Equal(expectedText, actualTextAtCorrectIndex);
        }
    }

    [Fact]
    public void GenerateTest_ShouldFilterInvalidQuestions()
    {
        // Arrange
        var valid = new QuestionModel("專業科目", "T", 1, "Valid", ["A", "B"], 1);
        var noOptions = new QuestionModel("專業科目", "T", 2, "No Options", [], 1);
        var badIndex = new QuestionModel("專業科目", "T", 3, "Bad Index", ["A"], 5);
        
        var pool = new[] { valid, noOptions, badIndex };

        // Act
        var result = _sut.GenerateTest(pool, 10, 10).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Valid", result[0].OriginalQuestion.Question);
    }

    [Fact]
    public void CalculateScore_ShouldReturnCorrectPercentage()
    {
        // Arrange
        var items = new List<TestItem>
        {
            CreateMockTestItem(true),
            CreateMockTestItem(true),
            CreateMockTestItem(false),
            CreateMockTestItem(false)
        };

        // Act
        var score = _sut.CalculateScore(items);

        // Assert
        Assert.Equal(50.0, score);
    }

    private static List<QuestionModel> CreateMockPool(int profCount, int commonCount)
    {
        var list = new List<QuestionModel>();
        for (int i = 0; i < profCount; i++)
            list.Add(new QuestionModel("專業科目", "Item", i, $"Q {i}", ["A", "B"], 1));
        for (int i = 0; i < commonCount; i++)
            list.Add(new QuestionModel("共同科目", "Item", i, $"Q {i}", ["A", "B"], 1));
        return list;
    }

    private static TestItem CreateMockTestItem(bool isCorrect)
    {
        var q = new QuestionModel("T", "T", 1, "Q", ["A", "B"], 1);
        var item = new TestItem(q, [new OptionItem(0, "A"), new OptionItem(1, "B")], 0);
        if (isCorrect) item.SelectedOptionIndex = 0;
        else item.SelectedOptionIndex = 1;
        return item;
    }
}
