namespace KursovaWorkBLL.Services.AdditionalServices
{
    /// <summary>
    /// Class providing a template for creating HTML email bodies.
    /// </summary>
    public static class EmailBodyHelper
    {
        /// <summary>
        /// Creates an HTML email body using the provided data.
        /// </summary>
        /// <param name="FirstName">Recipient's first name.</param>
        /// <param name="LastName">Recipient's last name.</param>
        /// <param name="verificationCode">Verification code.</param>
        /// <param name="purpose">Purpose of verification.</param>
        /// <returns>HTML email body.</returns>
        public static string BodyTemp(string FirstName, string LastName, int verificationCode, string purpose)
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
                    <h2>Dear {FirstName} {LastName},</h2>
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
        /// <param name="Make">Car make.</param>
        /// <param name="Model">Car model.</param>
        /// <param name="Year">Car year.</param>
        /// <returns>HTML email body.</returns>
        public static string OrderBodyTemp(string userName, string Make, string Model, int Year)
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
                    <p>You have purchased a new car: {Make} {Model}, {Year}.</p>
                    <p>Details of your order:</p>
                    <ul>
                        <li>Make: {Make}</li>
                        <li>Model: {Model}</li>
                        <li>Year: {Year}</li>
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
}
