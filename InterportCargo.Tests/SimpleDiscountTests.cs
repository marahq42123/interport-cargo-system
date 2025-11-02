using Xunit;
using InterportCargo.Web.Pages.Officer;
using InterportCargo.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;

   

namespace InterportCargo.Tests
{
     // Verify that discount calculation logic works correctly based on
    // container quantity and service requirements (quarantine and fumigation).

    public class SimpleDiscountTests
    {
        private PrepareQuotationModel CreateModel()
        {
            var options = new DbContextOptionsBuilder<InterportContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new InterportContext(options);
            return new PrepareQuotationModel(context);
        }


        // Test Case 1:
        // Ensures a 10% discount is applied when all conditions are met:
        // more than 10 containers, quarantine required, and fumigation required

        [Fact]
        public void GetDiscountRate_ShouldReturn10_WhenAllConditionsMet()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.GetDiscountRate(12, true, true);

            // Assert
            Assert.Equal(10m, result);
        }

         // Test Case 2:
        // Ensures no discount is applied when none of the conditions are met:
        // less than or equal to 5 containers, no quarantine, and no fumigation.
        
        [Fact]
        public void GetDiscountRate_ShouldReturn0_WhenNoConditionsMet()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.GetDiscountRate(2, false, false);

            // Assert
            Assert.Equal(0m, result);
        }
    }
}
