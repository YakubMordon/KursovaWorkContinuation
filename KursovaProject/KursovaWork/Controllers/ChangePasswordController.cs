using KursovaWorkDAL.Entity.Entities;
using KursovaWork.Models;
using KursovaWorkBLL.Services.AdditionalServices;
using KursovaWorkBLL.Services.MainServices.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Контролер, що відповідає за зміну пароля користувача.
    /// </summary>
    public class ChangePasswordController : Controller
    {
        /// <summary>
        /// Сервіс для роботи з користувачем
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Об'єкт класу ILogger для логування подій 
        /// </summary>
        private readonly ILogger<ChangePasswordController> _logger;

        /// <summary>
        /// Об'єкт класу User? (nullable), який вказує на поточного користувача
        /// </summary>
        private static User? _curUser;

        /// <summary>
        /// Змінна, яка містить в собі поточний верифікаційний код
        /// </summary>
        private static int _verificationCode;

        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="ChangePasswordController"/>.
        /// </summary>
        /// <param name="userService">Сервіс для роботи з користувачем.</param>
        /// <param name="logger">Логгер для запису логів.</param>
        public ChangePasswordController(IUserService userService, ILogger<ChangePasswordController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Отримує сторінку для введення пошти користувача.
        /// </summary>
        /// <returns>Сторінка введення пошти користувача.</returns>
        public IActionResult UserFinder()
        {
            _logger.LogInformation("Перехід на сторінку введення пошти");
            return View("~/Views/ForgotPassword/UserFinder.cshtml");
        }

        /// <summary>
        /// Обробляє введену пошту користувача для відправки верифікаційного коду.
        /// </summary>
        /// <param name="model">Модель, що містить введену пошту користувача.</param>
        /// <returns>Сторінка введення верифікаційного коду або сторінка введення пошти з повідомленням про помилку.</returns>
        public IActionResult ForgotPassword(EmailViewModel model)
        {
            _logger.LogInformation("Вхід у метод верифікації електронної пошти");
            if(ModelState.IsValid)
            {
                _curUser = _userService.GetUserByEmail(model.Email);    
                if (_curUser != null)
                {
                    _logger.LogInformation("Пошту знайдено");
                    SendCode();
                    return Json(new { success = true });
                }
                _logger.LogInformation("Електронна пошта не є зареєстрованою");

                return Json(new {success = false, error = "Така електронна пошта не є зареєстрованою" });
            }

            string error = ModelState[nameof(EmailViewModel.Email)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Дані не пройшли валідацію");
            return Json(new { success = false, error });
        }

        /// <summary>
        /// Обробляє введений верифікаційний код для переходу до сторінки зміни пароля користувача.
        /// </summary>
        /// <param name="model">Модель, що містить введений користувачем верифікаційний код.</param>
        /// <returns>Сторінка зміни пароля або сторінка введення верифікаційного коду з повідомленням про помилку.</returns>
        public IActionResult ChangePassword(VerificationViewModel model)
        {
            _logger.LogInformation("Вхід у метод верифікації коду");
            if (ModelState.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                _logger.LogInformation("Переходимо в цикл утворення цілісного рядка з 4 підрядків");

                foreach (var digit in model.VerificationDigits)
                {
                    if (string.IsNullOrEmpty(digit))
                    {
                        _logger.LogInformation("Не введено всіх цифр");
                        return Json(new { success = false, error = "Не введено всіх цифр" });
                    }
                    stringBuilder.Append(digit);
                }

                string temp = stringBuilder.ToString();

                if (int.Parse(temp) != _verificationCode)
                {
                    _logger.LogInformation("Неправильний код підтвердження");

                    return Json(new { success = false, error = "Неправильний код підтвердження" });
                }

                _logger.LogInformation("Успішно підтверджено можливість для користувача на зміну паролю");

                return Json(new { success = true });
            }

            string error = ModelState[nameof(VerificationViewModel.VerificationDigits)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Дані не пройшли валідацію");
            return Json(new { success = false, error });
        }

        /// <summary>
        /// Метод переходу на сторінку зміни паролю
        /// </summary>
        /// <returns>Сторінка зміни паролю</returns>
        public IActionResult UpdatePassword()
        {
            _logger.LogInformation("Перехід на сторінку зміни паролю");
            return View("~/Views/ForgotPassword/ChangePassword.cshtml");
        }

        /// <summary>
        /// Обробляє введений новий пароль користувача.
        /// </summary>
        /// <param name="model">Модель, що містить введений користувачем новий пароль.</param>
        /// <returns>Сторінка зміни пароля або сторінка введення нового пароля з повідомленням про помилку.</returns>
        [HttpPost]
        public IActionResult SubmitChange(ChangePasswordViewModel model)
        {
            _logger.LogInformation("Вхід у метод зміни паролю");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Дані пройшли валідацію");

                _userService.UpdatePasswordOfUser(model.Password, model.ConfirmPassword, _curUser);

                _logger.LogInformation("Успішно змінено пароль, переходимо на головну сторінку");

                return Json(new {success = true });

            }

            var errors = new
            {
                passwordError = ModelState[nameof(ChangePasswordViewModel.Password)].Errors.FirstOrDefault()?.ErrorMessage ?? "",
                confirmPasswordError = ModelState[nameof(ChangePasswordViewModel.ConfirmPassword)].Errors.FirstOrDefault()?.ErrorMessage ?? ""
            };

            _logger.LogInformation("Дані не пройшли валідацію");

            return Json(new {success = false, errors });
        }

        /// <summary>
        /// Відправляє електронний лист з верифікаційним кодом.
        /// </summary>
        /// <returns>Сторінка введення верифікаційного коду.</returns>
        public IActionResult SendVerificationCode()
        {
            _logger.LogInformation("Переходимо на сторінку з введенням коду");

            return View("~/Views/ForgotPassword/ForgotPassword.cshtml");
        }

        /// <summary>
        /// Відправляє заново електронний лист з новим верифікаційним кодом.
        /// </summary>
        /// <returns>Повідомлення про успішне надіслання коду</returns>
        public IActionResult ReSendVerificationCode()
        {
            SendCode();
            return Json(new { message = "Успішно надіслано код" });
        }

        /// <summary>
        /// Метод генерування коду та відправки листа
        /// </summary>
        public void SendCode()
        {
            _logger.LogInformation("Вхід у метод надсилання верифікаційного коду");

            _verificationCode = new Random().Next(1000, 9999);

            string subject = "Код підтвердження";

            string body = EmailBodyTemplate.BodyTemp(_curUser.FirstName, _curUser.LastName, _verificationCode, "зміни паролю");

            _logger.LogInformation("Надсилаємо повідомлення на електронну пошту користувача");

            EmailSender.SendEmail(_curUser.Email, subject, body);
        }
    }
}
