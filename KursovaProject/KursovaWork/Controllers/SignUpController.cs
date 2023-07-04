using Microsoft.AspNetCore.Mvc;
using KursovaWork.Models;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkBLL.Services.AdditionalServices;
using System.Text;
using KursovaWorkBLL.Services.MainServices.UserService;

namespace KursovaWork.Controllers
{
    /// <summary>
    /// Контролер, що відповідає за реєстрацію нового користувача.
    /// </summary>
    public class SignUpController : Controller
    {
        /// <summary>
        /// Сервіс для роботи з користувачем
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Об'єкт класу ILogger для логування подій 
        /// </summary>
        private readonly ILogger<SignUpController> _logger;

        /// <summary>
        /// Об'єкт класу User? (nullable), який вказує на поточного користувача
        /// </summary>
        private static User? _curUser;

        /// <summary>
        /// Змінна, яка містить в собі поточний верифікаційний код
        /// </summary>
        private static int _verificationCode;

        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="SignUpController"/>.
        /// </summary>
        /// <param name="userService">Сервіс для роботи з користувачем</param>
        /// <param name="logger">Логгер для запису логів.</param>
        public SignUpController(IUserService userService, ILogger<SignUpController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Отримує сторінку реєстрації.
        /// </summary>
        /// <returns>Сторінка реєстрації.</returns>
        public IActionResult SignUp()
        {
            _logger.LogInformation("Перехід на сторінку реєстрації");
            return View();
        }

        /// <summary>
        /// Отримує сторінку входу.
        /// </summary>
        /// <returns>Сторінка входу.</returns>
        public IActionResult LogIn()
        {
            _logger.LogInformation("Перехід на сторінку входу");
            return View("~/Views/LogIn/LogIn.cshtml");
        }

        /// <summary>
        /// Обробляє введені користувачем дані реєстрації.
        /// </summary>
        /// <param name="model">Модель, що містить введені користувачем дані реєстрації.</param>
        /// <returns>Сторінка введення верифікаційного коду або сторінка реєстрації з повідомленням про помилку.</returns>
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            _logger.LogInformation("Вхід у метод верифікації даних реєстрації");

            var errors = new Dictionary<string, string>();

            if (ModelState.IsValid)
            {
                User user = _userService.GetUserByEmail(model.Email);

                if (user != null)
                {
                    errors["emailError"] = "Користувач з такою ж елекронною поштою існує";
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    _logger.LogInformation("Користувач з такою ж елекронною поштою існує");
                    return Json(new { success = false, errors });
                }
                _curUser = model.ToUser();

                _logger.LogInformation("Успішно перевірено чи є така ж електронна пошта");

                SendCode();

                return Json(new { success = true });

            }

            errors["firstNameError"] = ModelState[nameof(SignUpViewModel.FirstName)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["lastNameError"] = ModelState[nameof(SignUpViewModel.LastName)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["emailError"] = ModelState[nameof(SignUpViewModel.Email)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["passwordError"] = ModelState[nameof(SignUpViewModel.Password)].Errors.FirstOrDefault()?.ErrorMessage ?? "";
            errors["confirmPasswordError"] = ModelState[nameof(SignUpViewModel.ConfirmPassword)].Errors.FirstOrDefault()?.ErrorMessage ?? "";


            _logger.LogInformation("Дані не пройшли валідацію");
            return Json(new { success = false, errors });
        }

        /// <summary>
        /// Обробляє введений користувачем верифікаційний код та здійснює підтвердження реєстрації.
        /// </summary>
        /// <param name="verification">Модель, що містить введений користувачем верифікаційний код.</param>
        /// <returns>Сторінка привітання або сторінка введення верифікаційного коду з повідомленням про помилку.</returns>
        [HttpPost]
        public IActionResult Submit(VerificationViewModel verification)
        {
            _logger.LogInformation("Вхід у метод підтвердження реєстрації та перевірки валідаційного коду");

            if (ModelState.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                _logger.LogInformation("Переходимо в цикл утворення цілісного рядка з 4 підрядків");

                foreach (var digit in verification.VerificationDigits)
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

                _userService.AddUser(_curUser);
                _curUser = null;
                _logger.LogInformation("Успішно зареєстровано користувача, перехід на головну сторінку");

                return Json(new { success = true });
            }

            string error = ModelState[nameof(VerificationViewModel.VerificationDigits)].Errors.FirstOrDefault()?.ErrorMessage ?? "";

            _logger.LogInformation("Дані не пройшли валідацію");
            return Json(new { success = false, error });
        }

        /// <summary>
        /// Метод переходу на сторінку привітання
        /// </summary>
        /// <returns>Сторінка привітання</returns>
        [HttpGet]
        public IActionResult Congratulations()
        {
            return View("~/Views/SignUp/Congratulations.cshtml");
        }

        /// <summary>
        /// Відправляє електронний лист з верифікаційним кодом.
        /// </summary>
        /// <returns>Сторінка введення верифікаційного коду.</returns>
        public IActionResult SendVerificationCode()
        {
            _logger.LogInformation("Переходимо на сторінку з введенням коду");

            return View("~/Views/SignUp/Submit.cshtml");
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

            string body = EmailBodyTemplate.BodyTemp(_curUser.FirstName, _curUser.LastName, _verificationCode, "реєстрації");

            _logger.LogInformation("Надсилаємо повідомлення на електронну пошту користувача");

            EmailSender.SendEmail(_curUser.Email, subject, body);
        }
    }
}
