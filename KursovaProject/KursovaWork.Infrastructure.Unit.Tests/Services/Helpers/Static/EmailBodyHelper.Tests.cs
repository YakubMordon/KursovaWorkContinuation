using FluentAssertions;
using KursovaWork.Infrastructure.Services.Helpers.Static;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.Helpers.Static;

public class EmailBodyHelperTests
{
    [Theory]
    [InlineData("Jack", "Hopkins", 1111, "Purpose")]
    [InlineData("John", "Johnson", 2222, "New Purpose")]
    [InlineData("Terry", "Harrison", 3333, "Entirely New Purpose")]
    public void BodyTemp_ShouldReturnHtmlEmailVerificationBody(string firstName, string lastName, int verificationCode, string purpose)
    {
        // Arrange

        var expected = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            font-size: 14px;
                        }}
                    </style>
                </head>
                <body>
                    <h2>Dear {firstName} {lastName},</h2>
                    <p>Your verification code: <strong>{verificationCode}</strong></p>
                    <p>Please use this code to verify your {purpose}.</p>
                    <p>If you have any questions or need additional information, please contact our support service.</p>
                    <p>Thank you for your trust!</p>
                    <p>Best regards,</p>
                    <p>VAG Dealer</p>
                </body>
                </html>";

        // Act

        var actual = EmailBodyHelper.BodyTemp(firstName, lastName, verificationCode, purpose);

        // Assert

        actual.Should().Be(expected);
    }    
    
    [Theory]
    [InlineData("Jack", "BMW", "M5", 2012)]
    [InlineData("John", "Audi", "Q8", 2020)]
    [InlineData("Terry", "Volkswagen", "Golf", 2018)]
    public void OrderBodyTemp_ShouldReturnHtmlEmailOrderBody(string userName, string make, string model, int year)
    {
        // Arrange

        var expected = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            font-size: 14px;
                        }}
                    </style>
                </head>
                <body>
                    <h2>Dear {userName},</h2>
                    <p>Thank you for your purchase!</p>
                    <p>You have purchased a new car: {make} {model}, {year}.</p>
                    <p>Details of your order:</p>
                    <ul>
                        <li>Make: {make}</li>
                        <li>Model: {model}</li>
                        <li>Year: {year}</li>
                    </ul>
                    <p>Additional order information is available in your personal account on our website.</p>
                    <p>If you have any questions or need further information, please contact our support service.</p>
                    <p>Thank you for your trust!</p>
                    <p>Best regards,</p>
                    <p>VAG Dealer</p>
                </body>
                </html>";

        // Act

        var actual = EmailBodyHelper.OrderBodyTemp(userName, make, model, year);

        // Assert

        actual.Should().Be(expected);
    }
}
