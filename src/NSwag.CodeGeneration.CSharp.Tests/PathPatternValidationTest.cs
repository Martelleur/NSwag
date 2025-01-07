using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace NSwag.CodeGeneration.CSharp.Tests
{
    public class PathPatternValidationTest
    {
        private static string generateSpec(bool withPattern)
        {
            return $@"{{
  ""openapi"": ""3.0.1"",
  ""info"": {{
    ""title"": ""NSwager Test Server API"",
    ""description"": ""An api used to test NSwager."",
    ""version"": ""2.0""
  }},
  ""paths"": {{
    ""/api/v2/test/{{PathVariable}}/ping"": {{
      ""get"": {{
        ""tags"": [
          ""TestControllerVersionTwo""
        ],
        ""summary"": ""Used to ping a valid user name."",
        ""operationId"": ""PingpathVariableV2"",
        ""parameters"": [
          {{
            ""name"": ""pathVariable"",
            ""in"": ""path"",
            ""description"": ""A System.String"",
            ""required"": true,
            ""schema"": {{
              {(withPattern ? @"""pattern"": ""^[a-zA-Z0-9_]+$"", " : "")}
              ""type"": ""string""
            }}
          }}
        ],
        ""responses"": {{
          ""200"": {{
            ""description"": ""With the user name"",
            ""content"": {{
              ""application/json"": {{
                ""schema"": {{
                  ""$ref"": ""#/components/schemas/PathVariableDTO""
                }}
              }}
            }}
          }}
        }}
      }}
    }}
  }},
  ""components"": {{
    ""schemas"": {{
      ""PathVariableDTO"": {{
        ""required"": [
          ""pathVariable""
        ],
        ""type"": ""object"",
        ""properties"": {{
          ""pathVariable"": {{
            ""type"": ""string"",
            ""description"": ""The value of the path variable"",
            ""nullable"": true
          }}
        }},
        ""additionalProperties"": false,
        ""description"": ""A DTO containing a path variable""
      }}
    }}
  }}
}}";
        }

        //This string if statement is exactly the same as the if statement in ValidatePatternValueMock method.
        private string generatedCode =
@"if (!System.Text.RegularExpressions.Regex.IsMatch(pathVariable, ""^[a-zA-Z0-9_]+$""))
    throw new System.ArgumentException(""Parameter 'pathVariable' does not match the required pattern '^[a-zA-Z0-9_]+$'."");";


        private static void ValidatePathPatternMock(string pathVariable, string regexPattern)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(pathVariable, regexPattern))
                throw new System.ArgumentException(
                    $"Parameter 'pathVariable' does not match the required pattern '{regexPattern}'."
                );
        }

        [Fact]
        public async Task When_path_parameter_have_pattern_field()
        {
            // Arrange
            var document = await OpenApiDocument.FromJsonAsync(generateSpec(withPattern: true));
            var generator = new CSharpClientGenerator(
                document,
                new CSharpClientGeneratorSettings()
            );

            // Act
            var code = generator.GenerateFile();
            
            // Assert
            Assert.Contains(NormalizeWhitespace(generatedCode), NormalizeWhitespace(code));
        }

        [Fact]
        public async Task When_path_parameter_not_have_pattern_field()
        {
            // Arrange
            var document = await OpenApiDocument.FromJsonAsync(generateSpec(withPattern: false));
            var generator = new CSharpClientGenerator(
                document,
                new CSharpClientGeneratorSettings()
            );

            // Act
            var code = generator.GenerateFile();

            // Assert
            Assert.DoesNotContain(generatedCode, code);
        }

        [Theory]
        [InlineData("User123", "^[a-zA-Z0-9_]+$")] // Alphanumeric and underscores
        [InlineData("User-123", "^[a-zA-Z0-9_-]+$")] // Alphanumeric, underscores, and dashes
        [InlineData("User.Name", "^[a-zA-Z0-9._]+$")] // Alphanumeric, dots, and underscores
        [InlineData("123456", "^\\d+$")] // Digits only
        public void ValidatePathVariable_ValidPathVariables_DoesNotThrow(
            string pathVariable,
            string regexPattern
        )
        {
            // Arrange & Act & Assert
            var exception = Record.Exception(
                () => ValidatePathPatternMock(pathVariable, regexPattern)
            );
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("User@123", "^[a-zA-Z0-9_]+$")] // Alphanumeric and underscores
        [InlineData("User Name", "^[a-zA-Z0-9_-]+$")] // Alphanumeric, underscores, and dashes
        [InlineData("User!Name", "^[a-zA-Z0-9._]+$")] // Alphanumeric, dots, and underscores
        [InlineData("Name123", "^\\d+$")] // Digits only
        public void ValidatePathVariable_InvalidPathVariables_ThrowsArgumentException(
            string pathVariable,
            string regexPattern
        )
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => ValidatePathPatternMock(pathVariable, regexPattern)
            );
            Assert.Contains(
                $"Parameter 'pathVariable' does not match the required pattern",
                exception.Message
            );
        }

        private static string NormalizeWhitespace(string input)
        {
            char[] separators = ['\r', '\n', '\t', ' '];
            return string.Join(
                " ",
                input.Split(separators, StringSplitOptions.RemoveEmptyEntries)
            );
        }
    }
}
