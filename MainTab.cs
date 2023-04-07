using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Windows.Controls;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Windows.Shapes;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        //maintab methods

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

            //    #region sqlQueries
            //    //запись принятых сообщений в бд
            //    string recievedMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
            //    $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Text}')";

            //    //запись принятых опасных сообщений в бд
            //    string recievedUnacceptableMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
            //    $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{fixedForDbMessageText}')";

            //    //запись приныятых фотографий в бд
            //    string recievedPhotoMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
            //    $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Photo}')";

            //    string returningAllUserToBotPrivateMessages = $"SELECT * FROM received_messages WHERE username = '{message.Chat.Username}'";
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
            {   Seven:
                Message message = update.Message;
                if (message is null) { goto Eight; }

                #region sqlQueries править запросы на запись в бд
                //запись принятых сообщений в бд
                string recievedMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Text}')";

                //запись приныятых фотографий в бд
                string recievedPhotoMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Photo}')";

                //string returningAllUserToBotPrivateMessages = $"SELECT * FROM received_messages WHERE username = '{message.Chat.Username}'"; //not using
                #endregion
                

                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message && message.Text.ToLower() == "/start") //if recieved Message update type
                {
                    var firstname = update.Message.Chat.FirstName;

                    if (message.Text.ToLower() == "/start") //if recieved this text
                    {
                        LiveLogger(message); // живой лог
                        FileLogger(message, message.Text, message.Chat.Id, Settings.logPath); // логгирование в файл
                        DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedMessageToDbQuery);

                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Добро пожаловать, {firstname}!", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                        return;
                    }
                    else if (message.Text is not null && message.Text.ToLower() is not "/start")
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Пожалуйста, выберите один из доступных вариантов:", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                        LiveLogger(message); // живой лог
                        FileLogger(message, message.Text, message.Chat.Id, Settings.logPath); // логгирование в файл
                        DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, recievedMessageToDbQuery);
                        return;
                    }


                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Добро пожаловать, {firstname}!", replyMarkup: TelegramBotKeypads.mainMenuKeypad, cancellationToken: cancellationToken);
                    //return;
                }

            Eight:
                ///главное меню (выбор программы) - 
                ///выбор уровня (фиксированный ответ)
                ///выбор вуза(фиксированный ответ)
                ///выбор направления(доступные ответы генерятся на основе предыдущих записанных callbackData)
                ///        public static string? choisedLevel;
        //public static string? choisedUniversity;
        //public static string? directionChoose;
                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) //if recieved CallbackQuery (button codes) update type
                {
                    //var updateCallback = update.CallbackQuery;
                    if (update.CallbackQuery.Data is "toHome") //действия при нажатии На главную
                    {
                        string telegramMessage = "Добро пожаловать!";
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.mainMenuKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                    }

                    //ОТПРАВКА КЛАВЫ ВЫБОРА УРОВНЯ
                    if (update.CallbackQuery.Data is "programChoose") //если ответ был programChoose, то изменить сообщение на следующее...
                    {
                        string telegramMessage = "Выберите желаемый уровень подготовки:";
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.levelChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);

                        //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.levelChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                    }

                    //ОТПРАВКА КЛАВЫ ВЫБОРА УНИВЕРА
                    if (update.CallbackQuery.Data.Contains("_level")) //если ответ содержал в себе level, то изменить сообщение на следующее...
                    {
                        TelegramBotKeypads.choisedLevel = update.CallbackQuery.Data.Replace("_level", string.Empty) as string;
                        //if(TelegramBotKeypads.choisedLevel is "first") { TelegramBotKeypads.choisedLevel = "бакалавриат"; }
                        //if (TelegramBotKeypads.choisedLevel is "second") { TelegramBotKeypads.choisedLevel = "магистратура"; }
                        //if (TelegramBotKeypads.choisedLevel is "third") { TelegramBotKeypads.choisedLevel = "специалитет"; }

                        string telegramMessage = "Пожалуйста, выберите необходимый университет:";
                        //Console.WriteLine(TelegramBotKeypads.choisedLevel); //ok

                        //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.universityChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: TelegramBotKeypads.universityChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);

                    }

                    //ОТПРАВКА КЛАВЫ ВЫБОРА ПРОГРАММЫ
                    if (update.CallbackQuery.Data.Contains("_university"))
                    {
                        string telegramMessage = "Подобранные программы обучения:";
                        TelegramBotKeypads.choisedUniversity = update.CallbackQuery.Data.Replace("_university", string.Empty) as string;
                        //Console.WriteLine(TelegramBotKeypads.choisedUniversity); //ok

                        // фильтрация карточек на основании выборов юзера
                        List<Card> filteredCardsByClient = new();

                        foreach (var item in cardsView)
                        {
                            if (TelegramBotKeypads.choisedLevel == item.Level && TelegramBotKeypads.choisedUniversity == item.UniversityName)
                                filteredCardsByClient.Add(item);
                        }

                        //генерация кнопок на основе отфильтрованных карточек
                        List<InlineKeyboardButton> filteredUniversitiesButtons = new(); //

                        foreach (var item in cardsView)
                        {
                            if (item.Level == TelegramBotKeypads.choisedLevel && item.UniversityName == TelegramBotKeypads.choisedUniversity)
                                filteredUniversitiesButtons.Add(InlineKeyboardButton.WithCallbackData(text: item.ProgramName, callbackData: Convert.ToString(item.Id)));
                        }
                        var dynamicProgramChoosingKeypad = new InlineKeyboardMarkup(filteredUniversitiesButtons);

                        //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: dynamicProgramChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: dynamicProgramChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                        //
                        Dispatcher.Invoke(() =>
                        {
                            LiveLogOutput.AppendText($"{update.CallbackQuery.Data.ToString()}\n");
                        });
                    }

                    //отправка сообщения с данными о выбранном направлении
                    int numericValue;
                    if(int.TryParse(update.CallbackQuery.Data, out numericValue) is true)
                    {
                        TelegramBotKeypads.choisedProgram = update.CallbackQuery.Data;

                        Card finalSelectedCard = cardsView[Convert.ToInt32(TelegramBotKeypads.choisedProgram)];
                        
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
                        //Console.WriteLine(TelegramBotKeypads.choisedUniversity); //ok

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
                                InlineKeyboardButton.WithCallbackData(text: "На главную", callbackData: "toHome"),
                            },

                        });

                        //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: dynamicProgramChoosingKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                        await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: lastButtonsKeypad, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                    }



                    //else
                    //{
                    //    goto Seven;
                    //}

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
            TgBot.MainBotCts.Cancel();
            LiveLogOutput.Clear();
            LiveLogOutput.Text = "Bot has been stopped";
        }

        private void StopExitBotBtn_Click(object sender, RoutedEventArgs e)
        {
            Stats.ShutdownTimeFixator();
            Stats.UpTimeWriter();
            TgBot.MainBotCts.Cancel();
            Environment.Exit(0);
        }

        private void ManualParserRunningBtn_Click(object sender, RoutedEventArgs e)
        {
            //new Thread(() => ParserGUI()).Start();            
        }

        private void CmdOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            ConsoleWorker.CardOutputter();
            
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
            LiveLogOutput.Text += $"Start listening bot @{me.Username} named as {me.FirstName}. Timestamp: {DateTime.Now}\n";
            LiveLogOutput.Text += "-----------------------------------------------------------------------------------------------------------\n";
            return OnBotLoadCts;
        }

        public void LiveLogger(Message? message)
        {
            Dispatcher.Invoke(() =>
            {
                return LiveLogOutput.Text += $"Received a '{message.Text}' message from @{message.Chat.Username} aka {message.Chat.FirstName} {message.Chat.LastName} in chat {message.Chat.Id} at {DateTime.Now}.\n" +
                            "-----------------------------------------------------------------------------------------------------------\n";
            });
        }

        public static async void FileLogger(Message message, string messageText, long chatId, string logPath) //логгирование полученных сообщений в файл
        {
            using StreamWriter logWriter = new(logPath, true); //инициализация экземпляра Streamwriter

            await logWriter.WriteLineAsync($"Received a '{messageText}' message from @{message.Chat.Username} aka {message.Chat.FirstName} {message.Chat.LastName} in chat {chatId} at {DateTime.Now}."); //эхо
            await logWriter.WriteLineAsync("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            await logWriter.WriteLineAsync($"Raw message: {Newtonsoft.Json.JsonConvert.SerializeObject(message)}");
            await logWriter.WriteLineAsync("--------------------------------------------------------------------------------------------------------------------");
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

        public static async Task<Task> ButtonedReplies(ITelegramBotClient botClient, Message? message, long? chatId, CancellationToken cancellationToken) //отправка пользователю текста его сообщения
        {
            if (message is not null)
            {
               // await botClient.SendTextMessageAsync(
               //chatId: chatId,
               //text: $"I received the following message:\n{message.Text}",
               //cancellationToken: cancellationToken);
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
