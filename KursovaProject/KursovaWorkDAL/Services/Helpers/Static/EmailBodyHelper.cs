namespace KursovaWork.Infrastructure.Services.Helpers.Static;

/// <summary>
/// Class providing a template for creating HTML email bodies.
/// </summary>
public static class EmailBodyHelper
{
    /// <summary>
    /// Creates an HTML email body using the provided data.
    /// </summary>
    /// <param name="firstName">Recipient's first name.</param>
    /// <param name="lastName">Recipient's last name.</param>
    /// <param name="verificationCode">Verification code.</param>
    /// <param name="purpose">Purpose of verification.</param>
    /// <returns>HTML email body.</returns>
    public static string BodyTemp(string firstName, string lastName, int verificationCode, string purpose)
    {
        return $@"
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
    }

    /// <summary>
    /// Creates an HTML email body for an order using the provided data.
    /// </summary>
    /// <param name="userName">User's name.</param>
    /// <param name="make">Car make.</param>
    /// <param name="model">Car model.</param>
    /// <param name="year">Car year.</param>
    /// <returns>HTML email body.</returns>
    public static string OrderBodyTemp(string userName, string make, string model, int year)
    {
        return $@"
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
    }
}