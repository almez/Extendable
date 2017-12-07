using System;
using Xunit;

namespace Extendable.Tests
{
    public class FieldFactoryTest
    {
        [Fact(DisplayName = "FieldFactory : Initializes Field.Id once created")]
        public void CreateField_ByFieldFactory_ShouldInitializeIdAsGuid()
        {
            //Arrange
            var field = FieldFactory.CreateField("User", "10", "MiddleName", "Alden");

            //Act
            var id = Guid.Parse(field.Id);

            //Assert
            Assert.NotNull(id);
        }
    }
}
