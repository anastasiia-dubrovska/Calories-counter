using Xunit;
using System.Collections.Generic;

namespace WpfAppCaloriesCounter.BLL.Tests
{
    public class CalorieCalculatorTests
    {
        [Fact]
        public void CalculateTotalCalories_ShouldReturnCorrectSum()
        {
            // Arrange
            var calculator = new CalorieCalculator();
            var foodEntries = new List<FoodEntry>
            {
                new FoodEntry { Calories = 200 },
                new FoodEntry { Calories = 150 },
                new FoodEntry { Calories = 100 }
            };

            // Act
            var result = calculator.CalculateTotalCalories(foodEntries);

            // Assert
            Assert.Equal(450, result);
        }
    }
}
