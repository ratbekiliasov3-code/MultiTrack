using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MultiTrack.Models
{
    public static class LanguageHelper
    {
        public static readonly string DefaultLang = "TR";
        public static readonly string[] SupportedLanguages = { "TR", "RU", "EN" };

        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            { "SiteName", new() { { "TR", "MultiTrack" }, { "EN", "MultiTrack" }, { "RU", "MultiTrack" } } },
            { "HomePageTitle", new() { { "TR", "MultiTrack | Giriş & Kayıt" }, { "EN", "MultiTrack | Login & Register" }, { "RU", "MultiTrack | Вход и регистрация" } } },
            { "LoginTab", new() { { "TR", "Giriş Yap" }, { "EN", "Login" }, { "RU", "Войти" } } },
            { "RegisterTab", new() { { "TR", "Kayıt Ol" }, { "EN", "Register" }, { "RU", "Регистрация" } } },
            { "Email", new() { { "TR", "E-posta" }, { "EN", "Email" }, { "RU", "Электронная почта" } } },
            { "Password", new() { { "TR", "Şifre" }, { "EN", "Password" }, { "RU", "Пароль" } } },
            { "ConfirmPassword", new() { { "TR", "Şifre Tekrar" }, { "EN", "Confirm Password" }, { "RU", "Повторите пароль" } } },
            { "EmailAddress", new() { { "TR", "E-posta Adresi" }, { "EN", "Email Address" }, { "RU", "Адрес электронной почты" } } },
            { "LoginButton", new() { { "TR", "Sisteme Gir" }, { "EN", "Sign In" }, { "RU", "Войти" } } },
            { "CreateAccountButton", new() { { "TR", "Hesap Oluştur" }, { "EN", "Create Account" }, { "RU", "Создать аккаунт" } } },
            { "IntroTitle", new() { { "TR", "Tüm Hayatını Tek Yerden Takip Et." }, { "EN", "Track Your Whole Life from One Place." }, { "RU", "Следите за всей своей жизнью в одном месте." } } },
            { "IntroDescription", new() { { "TR", "Kitapların, spor programın ve harcamaların artık kontrolün altında." }, { "EN", "Books, sports plans, and expenses are now under your control." }, { "RU", "Книги, спортивные планы и расходы теперь под вашим контролем." } } },
            { "FeatureBookTracking", new() { { "TR", "Kitap Takibi" }, { "EN", "Book Tracking" }, { "RU", "Отслеживание книг" } } },
            { "FeatureBookTrackingDesc", new() { { "TR", "Okuma alışkanlığı kazan" }, { "EN", "Build a reading habit" }, { "RU", "Вырабатывайте привычку к чтению" } } },
            { "FeatureSportsProgram", new() { { "TR", "Spor Programı" }, { "EN", "Sports Program" }, { "RU", "Спортивная программа" } } },
            { "FeatureSportsProgramDesc", new() { { "TR", "Antrenmanlarını yönet" }, { "EN", "Manage your workouts" }, { "RU", "Управляйте тренировками" } } },
            { "FeatureExpenseAnalysis", new() { { "TR", "Finans Analizi" }, { "EN", "Expense Analysis" }, { "RU", "Финансовый анализ" } } },
            { "FeatureExpenseAnalysisDesc", new() { { "TR", "Harcamalarını takip et" }, { "EN", "Track your expenses" }, { "RU", "Отслеживайте расходы" } } },
            { "FeatureWaterTracking", new() { { "TR", "Su Takibi" }, { "EN", "Water Tracking" }, { "RU", "Отслеживание воды" } } },
            { "FeatureWaterTrackingDesc", new() { { "TR", "Günlük su hedefine ulaş" }, { "EN", "Reach your daily water goal" }, { "RU", "Достигайте своей суточной нормы воды" } } },
            { "RegisterSuccess", new() { { "TR", "Kayıt işlemi başarılı! Şimdi kendi bilgilerinizle giriş yapabilirsiniz." }, { "EN", "Registration successful! You can now sign in with your credentials." }, { "RU", "Регистрация прошла успешно! Теперь вы можете войти в систему." } } },
            { "ErrorFillAll", new() { { "TR", "Lütfen tüm alanları doldurun." }, { "EN", "Please fill in all fields." }, { "RU", "Пожалуйста, заполните все поля." } } },
            { "ErrorPasswordsMismatch", new() { { "TR", "Şifreler birbiriyle uyuşmuyor!" }, { "EN", "Passwords do not match!" }, { "RU", "Пароли не совпадают!" } } },
            { "ErrorEmailExists", new() { { "TR", "Bu e-posta adresiyle zaten bir hesap var!" }, { "EN", "An account already exists with this email!" }, { "RU", "Аккаунт с этим адресом уже существует!" } } },
            { "ErrorEmailPassword", new() { { "TR", "E-posta ve şifre alanları boş bırakılamaz." }, { "EN", "Email and password cannot be empty." }, { "RU", "Электронная почта и пароль не могут быть пустыми." } } },
            { "ErrorInvalidLogin", new() { { "TR", "Hatalı e-posta veya şifre girdiniz!" }, { "EN", "Invalid email or password." }, { "RU", "Неправильный адрес электронной почты или пароль." } } },
            { "GoBack", new() { { "TR", "Geri Dön" }, { "EN", "Go Back" }, { "RU", "Назад" } } },
            { "WelcomeLabel", new() { { "TR", "Hoş geldin," }, { "EN", "Welcome," }, { "RU", "Добро пожаловать," } } },
            { "DashboardTitle", new() { { "TR", "Kontrol Paneli" }, { "EN", "Dashboard" }, { "RU", "Панель" } } },
            { "WaterCardTitle", new() { { "TR", "Günlük Su Takibi" }, { "EN", "Daily Water Tracking" }, { "RU", "Ежедневное отслеживание воды" } } },
            { "WaterCardGoal", new() { { "TR", "Hedef: 3L" }, { "EN", "Goal: 3L" }, { "RU", "Цель: 3л" } } },
            { "WaterCardLink", new() { { "TR", "Takvim ve Detaylar" }, { "EN", "Calendar & Details" }, { "RU", "Календарь и детали" } } },
            { "StatsCardTitle", new() { { "TR", "İstatistik" }, { "EN", "Statistics" }, { "RU", "Статистика" } } },
            { "StatsCardDesc", new() { { "TR", "Tüm ilerlemelerini tek yerde gör" }, { "EN", "See all your progress in one place" }, { "RU", "Смотрите весь прогресс в одном месте" } } },
            { "StatsCardLink", new() { { "TR", "İstatistikleri Gör" }, { "EN", "View Statistics" }, { "RU", "Посмотреть статистику" } } },
            { "PlanCardTitle", new() { { "TR", "Günlük Planım" }, { "EN", "Daily Plan" }, { "RU", "Ежедневный план" } } },
            { "PlanCardEmpty", new() { { "TR", "Henüz plan yok" }, { "EN", "No plans yet" }, { "RU", "Пока нет планов" } } },
            { "PlanCardLink", new() { { "TR", "Plan Ekle" }, { "EN", "Add Plan" }, { "RU", "Добавить план" } } },
            { "BookCardTitle", new() { { "TR", "Kitabım" }, { "EN", "My Book" }, { "RU", "Моя книга" } } },
            { "BookCardLink", new() { { "TR", "İlerlemeyi Kaydet" }, { "EN", "Save Progress" }, { "RU", "Сохранить прогресс" } } },
            { "WorkoutCardTitle", new() { { "TR", "Günlük Antrenman" }, { "EN", "Daily Workout" }, { "RU", "Ежедневная тренировка" } } },
            { "WorkoutCardMeta", new() { { "TR", "0 Hareket" }, { "EN", "0 Moves" }, { "RU", "0 упражнений" } } },
            { "WorkoutCardEmpty", new() { { "TR", "Bugün antrenman yok" }, { "EN", "No training today" }, { "RU", "Сегодня нет тренировки" } } },
            { "WorkoutCardLink", new() { { "TR", "Antrenmanı Başlat" }, { "EN", "Start Workout" }, { "RU", "Начать тренировку" } } },
            { "ExpenseCardTitle", new() { { "TR", "Günlük Harcamalar" }, { "EN", "Daily Expenses" }, { "RU", "Ежедневные расходы" } } },
            { "ExpenseCardTotalToday", new() { { "TR", "Bugünkü Toplam:" }, { "EN", "Today's Total:" }, { "RU", "Сегодня всего:" } } },
            { "ExpenseCardTotalMonth", new() { { "TR", "Bu Ayki Toplam:" }, { "EN", "This Month Total:" }, { "RU", "За текущий месяц:" } } },
            { "ExpenseCardLink", new() { { "TR", "Harcama Ekle / Detaylar" }, { "EN", "Add Expense / Details" }, { "RU", "Добавить расход / детали" } } },
            { "BookTrackingPageTitle", new() { { "TR", "Kitap Takibi" }, { "EN", "Book Tracking" }, { "RU", "Отслеживание книги" } } },
            { "BookTrackingQuestion", new() { { "TR", "Kaçıncı Sayfadasın?" }, { "EN", "What page are you on?" }, { "RU", "На какой вы странице?" } } },
            { "RemainingLabel", new() { { "TR", "Kalan:" }, { "EN", "Remaining:" }, { "RU", "Оставшиеся:" } } },
            { "CompletedLabel", new() { { "TR", "Tamamlanan:" }, { "EN", "Completed:" }, { "RU", "Пройдено:" } } },
            { "SaveInfoLabel", new() { { "TR", "Sayfa İlerlemesi" }, { "EN", "Page Progress" }, { "RU", "Прогресс страниц" } } },
            { "UpdateInfoButton", new() { { "TR", "Bilgileri Kaydet" }, { "EN", "Save Info" }, { "RU", "Сохранить" } } },
            { "ExpenseTrackingTitle", new() { { "TR", "Harcama Takip" }, { "EN", "Expense Tracking" }, { "RU", "Отслеживание расходов" } } },
            { "ExpenseTrackingSubtitle", new() { { "TR", "Bugünün harcama detaylarını ekleyin ve aylık toplamınızı takip edin." }, { "EN", "Add today's expense details and track your monthly total." }, { "RU", "Добавляйте расходы за сегодня и отслеживайте месячный итог." } } },
            { "TodayLabel", new() { { "TR", "Bugün" }, { "EN", "Today" }, { "RU", "Сегодня" } } },
            { "ThisMonthLabel", new() { { "TR", "Bu Ay" }, { "EN", "This Month" }, { "RU", "Этот месяц" } } },
            { "TotalExpensesLabel", new() { { "TR", "Toplam Harcama" }, { "EN", "Total Expense" }, { "RU", "Общие расходы" } } },
            { "ThisListShowsAllExpenses", new() { { "TR", "Bu liste tüm harcamalarınızı gösterir." }, { "EN", "This list shows all your expenses." }, { "RU", "Этот список показывает все ваши расходы." } } },
            { "NewExpense", new() { { "TR", "Yeni Harcama Ekle" }, { "EN", "Add New Expense" }, { "RU", "Добавить новый расход" } } },
            { "ExpenseDescription", new() { { "TR", "Açıklama" }, { "EN", "Description" }, { "RU", "Описание" } } },
            { "ExpenseAmount", new() { { "TR", "Tutar (₺)" }, { "EN", "Amount (₺)" }, { "RU", "Сумма (₺)" } } },
            { "AddButton", new() { { "TR", "Ekle" }, { "EN", "Add" }, { "RU", "Добавить" } } },
            { "ExpenseDetails", new() { { "TR", "Harcama Detayları" }, { "EN", "Expense Details" }, { "RU", "Детали расходов" } } },
            { "RecentExpenses", new() { { "TR", "Son eklenen harcamalarınız" }, { "EN", "Recently added expenses" }, { "RU", "Недавно добавленные расходы" } } },
            { "NoExpensesMessage", new() { { "TR", "Henüz kayıtlı harcamanız yok." }, { "EN", "No expenses recorded yet." }, { "RU", "Записи о расходах еще нет." } } },
            { "DescriptionHeader", new() { { "TR", "Açıklama" }, { "EN", "Description" }, { "RU", "Описание" } } },
            { "DateHeader", new() { { "TR", "Tarih" }, { "EN", "Date" }, { "RU", "Дата" } } },
            { "AmountHeader", new() { { "TR", "Tutar" }, { "EN", "Amount" }, { "RU", "Сумма" } } },
            { "DeleteButton", new() { { "TR", "Sil" }, { "EN", "Delete" }, { "RU", "Удалить" } } },
            { "BookBackButton", new() { { "TR", "Geri Dön" }, { "EN", "Back" }, { "RU", "Назад" } } },
            { "WaterTrackingTitle", new() { { "TR", "Su Takip" }, { "EN", "Water Tracking" }, { "RU", "Отслеживание воды" } } },
            { "TodayWaterText", new() { { "TR", "Bugün" }, { "EN", "Today" }, { "RU", "Сегодня" } } },
            { "GoalText", new() { { "TR", "Hedef" }, { "EN", "Goal" }, { "RU", "Цель" } } },
            { "AddWaterButton", new() { { "TR", "+250 ml" }, { "EN", "+250 ml" }, { "RU", "+250 мл" } } },
            { "SubtractWaterButton", new() { { "TR", "-250 ml" }, { "EN", "-250 ml" }, { "RU", "-250 мл" } } },
            { "AddWater500", new() { { "TR", "+500 ml" }, { "EN", "+500 ml" }, { "RU", "+500 мл" } } },
            { "SubtractWater500", new() { { "TR", "-500 ml" }, { "EN", "-500 ml" }, { "RU", "-500 мл" } } },
            { "ResetWaterButton", new() { { "TR", "Temizle" }, { "EN", "Reset" }, { "RU", "Сброс" } } },
            { "SaveWaterError", new() { { "TR", "Su verisi kaydedilemedi. Lütfen sayfayı yenileyip tekrar deneyin." }, { "EN", "Unable to save water data. Please refresh and try again." }, { "RU", "Не удалось сохранить данные о воде. Пожалуйста, обновите страницу и попробуйте снова." } } },
            { "ResetWaterError", new() { { "TR", "Su verisi sıfırlanamadı. Lütfen sayfayı yenileyip tekrar deneyin." }, { "EN", "Unable to reset water data. Please refresh and try again." }, { "RU", "Не удалось сбросить данные о воде. Пожалуйста, обновите страницу и попробуйте снова." } } },
            { "WorkoutTodayLabel", new() { { "TR", "Bugün:" }, { "EN", "Today:" }, { "RU", "Сегодня:" } } },
            { "NoWorkoutPlanMessage", new() { { "TR", "Bugün için bir planın yok, dinlenme günü mü?" }, { "EN", "No plan for today, rest day?" }, { "RU", "Нет плана на сегодня, день отдыха?" } } },
            { "WeeklyCompletionTitle", new() { { "TR", "Haftalık Tamamlanma Oranı" }, { "EN", "Weekly Completion Rate" }, { "RU", "Процент выполнения за неделю" } } },
            { "TotalWorkoutsLabel", new() { { "TR", "Toplam Antrenman" }, { "EN", "Total Workouts" }, { "RU", "Всего тренировок" } } },
            { "CompletedWorkoutsLabel", new() { { "TR", "Tamamlanan Antrenman" }, { "EN", "Completed Workouts" }, { "RU", "Завершенные тренировки" } } },
            { "WorkoutCompleteButton", new() { { "TR", "✓" }, { "EN", "✓" }, { "RU", "✓" } } },
            { "WorkoutDoneButton", new() { { "TR", "✓ Yapıldı" }, { "EN", "Done" }, { "RU", "Выполнено" } } },
            { "WorkoutCompleteAction", new() { { "TR", "Tamamla" }, { "EN", "Complete" }, { "RU", "Завершить" } } },
            { "DailyPlanTitle", new() { { "TR", "Günlük Plan" }, { "EN", "Daily Plan" }, { "RU", "Ежедневный план" } } },
            { "GoToTodayButton", new() { { "TR", "Bugüne Git" }, { "EN", "Go to Today" }, { "RU", "Перейти к сегодня" } } },
            { "MonthlyTotalConsumption", new() { { "TR", "Aylık Toplam Tüketim" }, { "EN", "Monthly Total Consumption" }, { "RU", "Ежемесячное потребление" } } },
            { "AddPlanPlaceholder", new() { { "TR", "Plan ekle..." }, { "EN", "Add a plan..." }, { "RU", "Добавить план..." } } },
            { "AddPlanButton", new() { { "TR", "Ekle" }, { "EN", "Add" }, { "RU", "Добавить" } } },
            { "TodayPlansHeader", new() { { "TR", "Bugünün Planları" }, { "EN", "Today's Plans" }, { "RU", "Планы на сегодня" } } },
            { "NoPlansMessage", new() { { "TR", "Bugün plan yok" }, { "EN", "No plans today" }, { "RU", "Нет планов на сегодня" } } },
            { "CalendarMonthFormat", new() { { "TR", "MMMM yyyy" }, { "EN", "MMMM yyyy" }, { "RU", "MMMM yyyy" } } },
            { "PlanCountLabel", new() { { "TR", "plan" }, { "EN", "plan" }, { "RU", "план" } } },
            { "StatisticsTitle", new() { { "TR", "İstatistikler" }, { "EN", "Statistics" }, { "RU", "Статистика" } } },
            { "BackButton", new() { { "TR", "Geri" }, { "EN", "Back" }, { "RU", "Назад" } } },
            { "WaterChartLabel", new() { { "TR", "Litre" }, { "EN", "Liters" }, { "RU", "Литры" } } },
            { "SportsThisMonthTitle", new() { { "TR", "Spor (Bu Ay)" }, { "EN", "Sports (This Month)" }, { "RU", "Спорт (этот месяц)" } } },
            { "ExpenseThisMonthTitle", new() { { "TR", "Harcama (Bu Ay)" }, { "EN", "Expense (This Month)" }, { "RU", "Расходы (этот месяц)" } } },
            { "ReadingProgressTitle", new() { { "TR", "Kitap Okuma" }, { "EN", "Book Reading" }, { "RU", "Чтение книги" } } },
            { "TasksTitle", new() { { "TR", "Görevler (Bu Ay)" }, { "EN", "Tasks (This Month)" }, { "RU", "Задачи (этот месяц)" } } },
            { "SummaryTitle", new() { { "TR", "Özet" }, { "EN", "Summary" }, { "RU", "Сводка" } } },
            { "SummaryWaterTotal", new() { { "TR", "Su Toplamı (7 gün):" }, { "EN", "Water Total (7 days):" }, { "RU", "Общий объем воды (7 дней):" } } },
            { "SummaryWorkouts", new() { { "TR", "Antrenmanlar (ay):" }, { "EN", "Workouts (month):" }, { "RU", "Тренировки (месяц):" } } },
            { "SummaryExpense", new() { { "TR", "Harcama (ay):" }, { "EN", "Expense (month):" }, { "RU", "Расходы (месяц):" } } },
            { "TodoPageTitle", new() { { "TR", "Todo Listesi" }, { "EN", "Todo List" }, { "RU", "Список дел" } } },
            { "NewTodoPlaceholder", new() { { "TR", "Yeni görev yaz..." }, { "EN", "Write a new task..." }, { "RU", "Напишите новую задачу..." } } },
            { "LanguageSelectLabel", new() { { "TR", "Dil" }, { "EN", "Lang" }, { "RU", "Язык" } } },
            { "LanguageTR", new() { { "TR", "TR" }, { "EN", "TR" }, { "RU", "TR" } } },
            { "LanguageRU", new() { { "TR", "RU" }, { "EN", "RU" }, { "RU", "RU" } } },
            { "LanguageEN", new() { { "TR", "EN" }, { "EN", "EN" }, { "RU", "EN" } } },
            { "ExpenseTrackingPageTitle", new() { { "TR", "Harcama Takip" }, { "EN", "Expense Tracking" }, { "RU", "Отслеживание расходов" } } },
            { "DateLabel", new() { { "TR", "Tarih" }, { "EN", "Date" }, { "RU", "Дата" } } },
            { "DescriptionLabel", new() { { "TR", "Açıklama" }, { "EN", "Description" }, { "RU", "Описание" } } },
            { "AmountLabel", new() { { "TR", "Tutar (₺)" }, { "EN", "Amount (₺)" }, { "RU", "Сумма (₺)" } } },
            { "AddExpenseButton", new() { { "TR", "Ekle" }, { "EN", "Add" }, { "RU", "Добавить" } } },
            { "ExpenseDetailsText", new() { { "TR", "Harcama Detayları" }, { "EN", "Expense Details" }, { "RU", "Детали расходов" } } },
            { "NoExpenses", new() { { "TR", "Henüz kayıtlı harcamanız yok." }, { "EN", "No expenses recorded yet." }, { "RU", "Записей о расходах еще нет." } } },
            { "SaveProgressButton", new() { { "TR", "İlerlemeyi Kaydet" }, { "EN", "Save Progress" }, { "RU", "Сохранить прогресс" } } },
            { "SaveInfoButton", new() { { "TR", "Bilgileri Kaydet" }, { "EN", "Save Info" }, { "RU", "Сохранить информацию" } } },
            { "WorkoutPageTitle", new() { { "TR", "Spor Takibi" }, { "EN", "Workout Tracking" }, { "RU", "Отслеживание тренировок" } } },
            { "AddWorkout", new() { { "TR", "Ekle" }, { "EN", "Add" }, { "RU", "Добавить" } } },
            { "CompleteWorkout", new() { { "TR", "✔" }, { "EN", "✔" }, { "RU", "✔" } } },
            { "Last7Days", new() { { "TR", "Son 7 Gün" }, { "EN", "Last 7 Days" }, { "RU", "Последние 7 дней" } } },
            { "ThisMonth", new() { { "TR", "Bu Ay" }, { "EN", "This Month" }, { "RU", "Этот месяц" } } },
            { "AverageDailyExpense", new() { { "TR", "Günlük Ortalama" }, { "EN", "Daily Average" }, { "RU", "Среднее ежедневное" } } },
            { "DailyPlanPageTitle", new() { { "TR", "Günlük Plan" }, { "EN", "Daily Plan" }, { "RU", "Ежедневный план" } } },
            { "AddPlan", new() { { "TR", "Ekle" }, { "EN", "Add" }, { "RU", "Добавить" } } },
            { "GoToToday", new() { { "TR", "Bugüne Git" }, { "EN", "Go to Today" }, { "RU", "Перейти к сегодня" } } },
            { "Done", new() { { "TR", "✓ Yapıldı" }, { "EN", "✓ Done" }, { "RU", "✓ Выполнено" } } },
            { "Complete", new() { { "TR", "Tamamla" }, { "EN", "Complete" }, { "RU", "Завершить" } } },
            { "TodayAt", new() { { "TR", "Bugün:" }, { "EN", "Today:" }, { "RU", "Сегодня:" } } }
        };

        public static string GetCurrentLanguage(HttpContext? context)
        {
            if (context?.Request?.Cookies?.TryGetValue("lang", out var lang) == true)
            {
                lang = lang.ToUpperInvariant();
                if (SupportedLanguages.Contains(lang)) return lang;
            }
            return DefaultLang;
        }

        public static string GetCultureCode(HttpContext? context)
        {
            return GetCurrentLanguage(context) switch
            {
                "EN" => "en-US",
                "RU" => "ru-RU",
                _ => "tr-TR"
            };
        }

        public static string T(HttpContext? context, string key)
        {
            var lang = GetCurrentLanguage(context);
            return GetText(lang, key);
        }

        public static string GetText(string language, string key)
        {
            if (!SupportedLanguages.Contains(language)) language = DefaultLang;
            if (Translations.TryGetValue(key, out var localized) && localized.TryGetValue(language, out var value))
            {
                return value;
            }
            return key;
        }

        public static IEnumerable<(string Code, string Label)> GetLanguageOptions()
        {
            return SupportedLanguages.Select(code => (code, GetText(code, "Language" + code)));
        }
    }
}
