using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        //maintab methods
        static string? selectedLevel;
        static string? selectedUniversity;
        static string? selectedProgram;
        static string? firstname;
        private async void BotStartBtn_Click(object sender, RoutedEventArgs e)
        {
            LiveLogOutput.Clear();

            using CancellationTokenSource OnBotLoadCts = await OnBotLoadMsg();

            // отправка запроса отмены для остановки
            OnBotLoadCts.Cancel();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            await Task.Factory.StartNew(() => TgBot.botClient.StartReceiving(updateHandler: HandleUpdateAsync,
                                                                            pollingErrorHandler: HandleErrorAsync,
                                                                            cancellationToken: TgBot.MainBotCts.Token, receiverOptions: receiverOptions)); //ok


            #region old Update method
            //async Task<Task> HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            //{
            //    Message? message = update.Message;
            //    if (message is null)
            //    {
            //        MessageBox.Show("update.Message is null", "Error");
            //        return Task.CompletedTask;
            //    }
            //    long? chatId = message.Chat.Id;
            //    string? fixedForDbMessageText = null; //правка текста для экранирования запроса

            //    //заготовленные реакции бота на определенные типы сообщений
            //    #region bot's reactions on incoming message types
            //    string reaction_recievedAudio = "Good audio, but I don't have an ears";
            //    string reaction_recievedContact = "Would you like us to contact you later?";
            //    string reaction_recievedDocument = "Delete this document.";
            //    string reaction_recievedPhoto = "Nice photo, but send me a text.";
            //    string reaction_recievedSticker = "Answers with stickers do not count as answers. Respect your and other's time.";
            //    string reaction_recievedVideo = "Is it a video?";
            //    string reaction_recievedVoice = "Nice moan.";
            //    //string reaction_recievedText = "Welcome!";
            //    #endregion

            //    #region PrepairedMessageSending
            //    if (message.Audio is not null)
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedAudio, message.Chat.Id, cancellationToken);

            //    }

            //    if (message.Contact is not null)
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedContact, message.Chat.Id, cancellationToken);
            //    }

            //    if (message.Document is not null)
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedDocument, message.Chat.Id, cancellationToken);
            //    }

            //    if (message.Photo is not null) // Telegram.Bot.Types.PhotoSize[4]} IT IS TRUE
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedPhoto, message.Chat.Id, cancellationToken);

            //        LiveLogger(message); // живой лог
            //        FileLogger(message, Convert.ToString(message.Photo.Length), message.Chat.Id, Settings.logPath); // логгирование в файл
            //        DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedPhotoMessageToDbQuery);
            //    }

            //    if (message.Sticker is not null)
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedSticker, message.Chat.Id, cancellationToken);
            //    }

            //    if (message.Video is not null)
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedVideo, message.Chat.Id, cancellationToken);
            //    }

            //    if (message.Voice is not null)
            //    {
            //        await PrepairedMessageSender(botClient, reaction_recievedVoice, message.Chat.Id, cancellationToken);
            //    }
            //    #endregion

            //    if (message.Text is not null)
            //    {
            //        LiveLogger(message); // живой лог
            //        FileLogger(message, message.Text, message.Chat.Id, Settings.logPath); // логгирование в файл

            //        //checking SQL-problem symbols in message before writing it in db
            //        if (message.Text is not null && message.Text.Contains('\''))
            //        {
            //            string convertedMessageText = message.Text.Replace("'", "\\'");

            //            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedUnacceptableMessageToDbQuery);
            //        }

            //        else
            //        {
            //            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedMessageToDbQuery);
            //        }

            //        await ParrotedMessageSender(botClient, message, chatId, cancellationToken);
            //    }
            //    return Task.CompletedTask;
            //}
            #endregion

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                Message message = update.Message;
                if (message is null) { goto Eight; }

                #region sqlQueries править запросы на запись в бд
                //запись принятых сообщений в бд
                string recievedMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Text}')";

                //запись приныятых фотографий в бд
                string recievedPhotoMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Photo}')";
                #endregion

                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message && message.Text is null) //suggestion if recieved not text message
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Пожалуйста, выберите один из доступных вариантов:", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                    LiveLogger_message(message); // живой лог
                    FileLogger_message(message, message.Text, message.Chat.Id, Settings.fileLoggerPath); // логгирование в файл
                    return;

                }

                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message && message.Text.ToLower() == "/start") //if recieved Message update type
                {
                    firstname = update.Message.Chat.FirstName;

                    if (message.Text.ToLower() == "/start") //if recieved this text
                    {
                        LiveLogger_message(message); // живой лог
                        FileLogger_message(message, message.Text, message.Chat.Id, Settings.fileLoggerPath); // логгирование в файл
                        DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedMessageToDbQuery);

                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Добро пожаловать, {firstname}!", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                        return;
                    }
                    else if (message.Text is not null && message.Text.ToLower() is not "/start")
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Пожалуйста, выберите один из доступных вариантов:", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                        LiveLogger_message(message); // живой лог
                        FileLogger_message(message, message.Text, message.Chat.Id, Settings.fileLoggerPath); // логгирование в файл
                        DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedMessageToDbQuery);
                        return;
                    }

                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Добро пожаловать, {firstname}!", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                }

            Eight:
                ///главное меню (выбор программы) - 
                ///выбор уровня (фиксированный ответ)
                ///выбор вуза (парсятся)
                ///выбор направления (доступные ответы генерятся на основе предыдущих записанных callbackData)

                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    if (update.CallbackQuery.Data is "toHome") //действия при нажатии На главную
                    {
                        string telegramMessage = $"Добро пожаловать, {firstname}!";
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.mainMenuKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                    }

                    //ОТПРАВКА КЛАВЫ ВЫБОРА УРОВНЯ
                    if (update.CallbackQuery.Data is "programChoose") //если ответ был programChoose, то изменить сообщение на следующее...
                    {
                        string telegramMessage = "Выберите желаемый уровень подготовки:";
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.levelChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);

                    }

                    //ОТПРАВКА КЛАВЫ ВЫБОРА УНИВЕРА
                    if (update.CallbackQuery.Data.Contains("_level")) //если ответ содержал в себе level, то изменить сообщение на следующее...
                    {
                        selectedLevel = update.CallbackQuery.Data.Replace("_level", string.Empty) as string;
                        //UniversityEntryFreq.universitiesFreqList;
                        //generation vuvs:
                        List<InlineKeyboardButton> parsedUniversitiesButtons = new(); //

                        foreach (var item in UniversityEntryFreq.universitiesFreqList)
                        {
                            //if (item.Level == choisedLevel && item.UniversityName == choisedUniversity)
                              parsedUniversitiesButtons.Add(InlineKeyboardButton.WithCallbackData(text: item.UniversityName, callbackData: Convert.ToString(item.UniversityName) + "_university"));
                        }
                        parsedUniversitiesButtons.Add(InlineKeyboardButton.WithCallbackData(text: "🏠", callbackData: "toHome"));
                        var dynamicUniversityChoosingKeypad = new InlineKeyboardMarkup(parsedUniversitiesButtons);


                        string telegramMessage = "Пожалуйста, выберите необходимый университет:";
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: dynamicUniversityChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);

                    }

                    //ОТПРАВКА КЛАВЫ ВЫБОРА ПРОГРАММЫ
                    if (update.CallbackQuery.Data.Contains("_university"))
                    {
                        
                        selectedUniversity = update.CallbackQuery.Data.Replace("_university", string.Empty) as string;
                        string telegramMessage = "Подобранные программы обучения:\n\n";
                        // фильтрация карточек на основании выборов абитуриента
                        List<Card> filteredCardsByEnrollee = new();

                        foreach (var item in cardsView) //переписать цикл на фор для нормальной нумерации направлений
                        {
                            if (selectedLevel == item.Level && selectedUniversity == item.UniversityName)
                                filteredCardsByEnrollee.Add(item);
                        }

                        foreach (var item in filteredCardsByEnrollee)
                        {
                            telegramMessage += $"{item.Id}:\t{item.ProgramName}\n";
                        }

                        //генерация кнопок на основе отфильтрованных карточек
                        List<InlineKeyboardButton> filteredUniversitiesButtons = new(); //

                        foreach (var item in filteredCardsByEnrollee)
                        {
                            if (item.Level == selectedLevel && item.UniversityName == selectedUniversity)
                                filteredUniversitiesButtons.Add(InlineKeyboardButton.WithCallbackData(text: Convert.ToString(item.Id), callbackData: Convert.ToString(item.Id)));
                        }
                        filteredUniversitiesButtons.Add(InlineKeyboardButton.WithCallbackData(text: "🏠", callbackData: "toHome"));

                        var dynamicProgramChoosingKeypad = new InlineKeyboardMarkup(filteredUniversitiesButtons);

                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: dynamicProgramChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                    }

                    //отправка финального сообщения с данными о выбранном направлении
                    if (int.TryParse(update.CallbackQuery.Data, out int isNumericValue) is true)
                    {
                        selectedProgram = update.CallbackQuery.Data;

                        Card finalSelectedCard = cardsView[Convert.ToInt32(selectedProgram)];

                        string telegramMessage = $"Мы подобрали для вас следующее направление:\n" +
                                                $"Университет:\t{finalSelectedCard.UniversityName}\n" +
                                                $"Программа:\t{finalSelectedCard.ProgramName}\n" +
                                                $"Код программы:\t{finalSelectedCard.ProgramCode}\n" +
                                                $"Уровень образования:\t{finalSelectedCard.Level}\n" +
                                                $"Форма обучения:\t{finalSelectedCard.StudyForm}\n" +
                                                $"Длительность обучения:\t{finalSelectedCard.Duration}\n" +
                                                $"Язык обучения:\t{finalSelectedCard.StudyLang}\n" +
                                                $"Куратор:\t{finalSelectedCard.Curator}\n" +
                                                $"Номер телефона:\t{finalSelectedCard.PhoneNumber}\n" +
                                                $"Почта:\t{finalSelectedCard.Email}\n" +
                                                $"Стоимость обучения:\t{finalSelectedCard.Cost}";

                        InlineKeyboardMarkup lastButtonsKeypad = new(
                        new[]
                        {
                            // first row
                            new[]
                            {
                                InlineKeyboardButton.WithUrl(text: "Связаться", url: $"mailto:{finalSelectedCard.Email}"),
                            },
                            // second row
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "🏠", callbackData: "toHome"),
                            },

                        });

                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: lastButtonsKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                        
                        LiveLogger_callBack(update.CallbackQuery, finalSelectedCard);
                        ChoicesToDb(update.CallbackQuery, finalSelectedCard);
                        FileLogger_callBack(update.CallbackQuery, Settings.callbackLoggerPath, finalSelectedCard);

                    }

                }
            }

            Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) //обработчик ошибок API
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}\nTimestamp: {DateTime.Now}",
                    _ => exception.ToString()
                };

                Dispatcher.Invoke(() =>
                {
                    return LiveLogOutput.Text += $"{ErrorMessage}\n" + "-------------------------------------------------------------------------------------------------------------\n";
                });
                return Task.CompletedTask;
            }

        }

        private void StopBotBtn_Click(object sender, RoutedEventArgs e)
        {
            Stats.ShutdownTimeFixator();
            Stats.UpTimeWriter();
            TgBot.MainBotCts.Cancel();
            LiveLogOutput.Clear();
            LiveLogOutput.Text = "Бот был остановлен.";
        }

        private void StopExitBotBtn_Click(object sender, RoutedEventArgs e)
        {
            Stats.ShutdownTimeFixator();
            Stats.UpTimeWriter();
            TgBot.MainBotCts.Cancel();
            Environment.Exit(0);
        }

        private void CmdOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => ConsoleWorker.CardOutputter());            
        }

        #region other right stackpanel buttons
        private void OutputPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");

        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");

        }

        private void OutputClrBtn_Click(object sender, RoutedEventArgs e)
        {
            LiveLogOutput.Clear();
        }
        #endregion

        public async Task<CancellationTokenSource> OnBotLoadMsg()
        {
            CancellationTokenSource OnBotLoadCts = new();
            User me = await TgBot.botClient.GetMeAsync();
            LiveLogOutput.Text += $"Начато прослушивание бота @{me.Username} с именем {me.FirstName} в {DateTime.Now}\n";
            LiveLogOutput.Text += "-----------------------------------------------------------------------------------------------------------\n";
            return OnBotLoadCts;
        }

        public void LiveLogger_message(Message? message)
        {
            Dispatcher.Invoke(() =>
            {
                return LiveLogOutput.Text += $"Получено сообщение '{message.Text}' от пользователя @{message.Chat.Username} так же известного, как {message.Chat.FirstName} {message.Chat.LastName} в чате {message.Chat.Id} в {DateTime.Now}.\n" +
                            "-----------------------------------------------------------------------------------------------------------\n";
            });
        }

        public void LiveLogger_callBack(CallbackQuery callbackQuery, Card card)
        {
            Dispatcher.Invoke(() =>
            {
                return LiveLogOutput.Text += $"Пользователь @{callbackQuery.From.Username}, так же известный, как {callbackQuery.From.FirstName} {callbackQuery.From.LastName} выбрал уровень {selectedLevel}, университет {selectedUniversity} и программу {card.ProgramName} в {DateTime.Now}\n" +
                                            "-----------------------------------------------------------------------------------------------------------\n";
            });
        }

        public static async void FileLogger_message(Message message, string messageText, long chatId, string logPath) //логгирование полученных сообщений в файл
        {
            using StreamWriter logWriter = new(logPath, true); //инициализация экземпляра Streamwriter

            await logWriter.WriteLineAsync($"Получено сообщение '{messageText}' от пользователя @{message.Chat.Username}, так же известного, как {message.Chat.FirstName} {message.Chat.LastName} в чате {chatId} в {DateTime.Now}."); //эхо
            await logWriter.WriteLineAsync("-----------------------------------------------------------------------------------------------------------");
        }

        public static async void FileLogger_callBack(CallbackQuery callbackQuery, string logPath, Card card) //логгирование callback в файл
        {
            using StreamWriter logWriter = new(logPath, true); //инициализация экземпляра Streamwriter

            await logWriter.WriteLineAsync($"Пользователь @{callbackQuery.From.Username}, так же известный, как {callbackQuery.From.FirstName} {callbackQuery.From.LastName} выбрал уровень {selectedLevel}, университет {selectedUniversity} и программу {card.ProgramName} в {DateTime.Now}");
            await logWriter.WriteLineAsync("-----------------------------------------------------------------------------------------------------------");

        }

        public void ChoicesToDb(CallbackQuery callbackQuery, Card card) 
        {
            string query = $"INSERT INTO Fixated_choices (username, fname, lname, choisedLevel, choisedUniversity, choisedProgram, timestamp) " +
                $"VALUES ('@{callbackQuery.From.Username}', '{callbackQuery.From.FirstName}', '{callbackQuery.From.LastName}', '{selectedLevel}', '{selectedUniversity}', '{card.ProgramName}', '{DateTime.Now}')";
            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query); //запись истории паролей

        }

        public static async Task PrepairedMessageSender(ITelegramBotClient botClient, string sendingMessage, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId: chatId,
                                                    text: sendingMessage,
                                                    cancellationToken: cancellationToken);
        }

        public static async Task<Task> ParrotedMessageSender(ITelegramBotClient botClient, Message? message, long? chatId, CancellationToken cancellationToken) //отправка пользователю текста его сообщения
        {
            if (message is not null)
            {
                await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: $"I received the following message:\n{message.Text}",
               cancellationToken: cancellationToken);
            }

            else await ErrorToChatSender(botClient, chatId, cancellationToken);
            return Task.CompletedTask;
        }

        public static async Task ErrorToChatSender(ITelegramBotClient botClient, long? chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"botserver_standard error. message.Text is null?",
            cancellationToken: cancellationToken);
        }
    }
}
