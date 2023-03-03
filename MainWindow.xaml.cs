using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Exceptions;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        #pragma warning disable CS8604
        static TelegramBotClient botClient = new(App.botToken); //инициализация клиента
        static SqliteConnection sqliteConn = new(App.connString); //инициализация подключения к бд
        private CancellationTokenSource MainBotCts = new();

        //импорт библиотек для запуска консоли
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public MainWindow()
        {
            InitializeComponent();
            SetPwdBox.MaxLength = 50;
            SetRepeatedPwdBox.MaxLength = 50;
        }

        private async void BotStartBtn_Click(object sender, RoutedEventArgs e)
        {
            //code for interact with textbox

            LiveLogOutput.Clear();

            using CancellationTokenSource OnBotLoadCts = await OnBotLoadMsg();

            // отправка запроса отмены для остановки
            OnBotLoadCts.Cancel();
            //Update update = new Update();
            new Thread(() => botClient.StartReceiving(updateHandler: HandleUpdateAsync,
                                                      pollingErrorHandler: HandleErrorAsync,
                                                      cancellationToken: MainBotCts.Token)).Start();
            
            async Task<Task> HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {

                Message? message = update.Message;
                if (message is null)
                {
                    MessageBox.Show("update.Message is null", "Error");
                    return Task.CompletedTask;
                }
                long? chatId = message.Chat.Id;
                string? fixedForDbMessageText = null; //правка текста для экранирования запроса

                //заготовленные реакции бота на определенные типы сообщений
                #region bot's reactions on incoming message types
                string reaction_recievedAudio = "Good audio, but I don't have an ears";
                string reaction_recievedContact = "Would you like us to contact you later?";
                string reaction_recievedDocument = "Delete this document.";
                string reaction_recievedPhoto = "Nice photo, but send me a text.";
                string reaction_recievedSticker = "Answers with stickers do not count as answers. Respect your and other's time.";
                string reaction_recievedVideo = "Is it a video?";
                string reaction_recievedVoice = "Nice moan.";
                //string reaction_recievedText = "Welcome!";
                #endregion

                #region sqlQueries
                //запись принятых сообщений в бд
                string recievedMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Text}')";

                //запись принятых опасных сообщений в бд
                string recievedUnacceptableMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{fixedForDbMessageText}')";

                //запись приныятых фотографий в бд
                string recievedPhotoMessageToDbQuery = $"INSERT INTO Received_messages(username, is_bot, first_name, last_name, language_code, chat_id, message_id, message_date, chat_type, message_content) " +
                $"VALUES('@{message.Chat.Username}', '0', '{message.Chat.FirstName}', '{message.Chat.LastName}', 'ru', '{message.Chat.Id}', '{message.MessageId}', '{DateTime.Now}', '{message.Chat.Type}', '{message.Photo}')";

                string returningAllUserToBotPrivateMessages = $"SELECT * FROM received_messages WHERE username = '{message.Chat.Username}'";
                #endregion

                #region PrepairedMessageSending
                if (message.Audio is not null)
                {
                    await PrepairedMessageSender(botClient, reaction_recievedAudio, message.Chat.Id, cancellationToken);

                }

                if (message.Contact is not null)
                {
                    await PrepairedMessageSender(botClient, reaction_recievedContact, message.Chat.Id, cancellationToken);
                }

                if (message.Document is not null)
                {
                    await PrepairedMessageSender(botClient, reaction_recievedDocument, message.Chat.Id, cancellationToken);
                }

                if (message.Photo is not null) // Telegram.Bot.Types.PhotoSize[4]} IT IS TRUE
                {
                    await PrepairedMessageSender(botClient, reaction_recievedPhoto, message.Chat.Id, cancellationToken);

                    LiveLogger(message); // живой лог
                    FileLogger(message, Convert.ToString(message.Photo.Length), message.Chat.Id, App.logPath); // логгирование в файл
                    DbWorker.DbQuerySilentSender(sqliteConn, recievedPhotoMessageToDbQuery);

                }

                if (message.Sticker is not null)
                {
                    await PrepairedMessageSender(botClient, reaction_recievedSticker, message.Chat.Id, cancellationToken);
                }

                if (message.Video is not null)
                {
                    await PrepairedMessageSender(botClient, reaction_recievedVideo, message.Chat.Id, cancellationToken);
                }

                if (message.Voice is not null)
                {
                    await PrepairedMessageSender(botClient, reaction_recievedVoice, message.Chat.Id, cancellationToken);
                }
                #endregion

                if (message.Text is not null)
                {
                    LiveLogger(message); // живой лог
                    FileLogger(message, message.Text, message.Chat.Id, App.logPath); // логгирование в файл

                    //checking SQL-problem symbols in message before writing it in db
                    if (message.Text is not null && message.Text.Contains('\''))
                    {
                        string convertedMessageText = message.Text.Replace("'", "\\'");

                        DbWorker.DbQuerySilentSender(sqliteConn, recievedUnacceptableMessageToDbQuery);
                    }

                    else
                    {
                        DbWorker.DbQuerySilentSender(sqliteConn, recievedMessageToDbQuery);
                    }

                    await ParrotedMessageSender(botClient, message, chatId, cancellationToken);
                }
                return Task.CompletedTask;
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

        private async Task<CancellationTokenSource> OnBotLoadMsg()
        {
            CancellationTokenSource OnBotLoadCts = new();
            User me = await botClient.GetMeAsync();
            LiveLogOutput.Text += $"Start listening bot @{me.Username} named as {me.FirstName}. Timestamp: {DateTime.Now}\n";
            LiveLogOutput.Text += "-------------------------------------------------------------------------------------------------------------\n";
            return OnBotLoadCts;
        }

        void LiveLogger(Message? message)
        {
            Dispatcher.Invoke(() =>
            {
                return LiveLogOutput.Text += $"Received a '{message.Text}' message from @{message.Chat.Username} aka {message.Chat.FirstName} {message.Chat.LastName} in chat {message.Chat.Id} at {DateTime.Now}.\n" +
                            "-------------------------------------------------------------------------------------------------------------\n";
            });
            //Console.WriteLine($"Raw message: {Newtonsoft.Json.JsonConvert.SerializeObject(message)}");
        }

        private static async void FileLogger(Message message, string messageText, long chatId, string logPath) //логгирование полученных сообщений в файл
        {
            using StreamWriter logWriter = new(logPath, true); //инициализация экземпляра Streamwriter

            await logWriter.WriteLineAsync($"Received a '{messageText}' message from @{message.Chat.Username} aka {message.Chat.FirstName} {message.Chat.LastName} in chat {chatId} at {DateTime.Now}."); //эхо                                                                                                                                                                                                       //await logWriter.FlushAsync();
            await logWriter.WriteLineAsync("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            await logWriter.WriteLineAsync($"Raw message: {Newtonsoft.Json.JsonConvert.SerializeObject(message)}");
            await logWriter.WriteLineAsync("--------------------------------------------------------------------------------------------------------------------");

        }

        private static async Task PrepairedMessageSender(ITelegramBotClient botClient, string sendingMessage, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId: chatId,
                                                    text: sendingMessage,
                                                    cancellationToken: cancellationToken);
        }

        private static async Task<Task> ParrotedMessageSender(ITelegramBotClient botClient, Message? message, long? chatId, CancellationToken cancellationToken) //отправка пользователю текста его сообщения
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

        #region other buttons
        private void StopBotBtn_Click(object sender, RoutedEventArgs e)
        {
            MainBotCts.Cancel();
            LiveLogOutput.Clear();
            LiveLogOutput.Text = "Bot has been stopped";
        }

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

        private void CmdOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            //метод кнопки запуска парсера в cmd
            ConsoleBot.BotFixer();
        }


        #region settings buttons
        private void SetTokenBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetLogPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetDbPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OutputPauseBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImportSettingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void SetPwdBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (SetPwdBox.Password != SetRepeatedPwdBox.Password)
            //{
            //    MessageBox.Show("Passwords are not the same", "Error");
            //}
            //if (SetPwdBox.Password is null || SetRepeatedPwdBox is null)
            //{
            //    MessageBox.Show("Please enter the password", "Error");
            //}
            //else
            //{
            //    App.pwd = SetRepeatedPwdBox.Password;
            //    MessageBox.Show("Password setted", "Notice");
            //}    

            if (SetPwdBox.Password == SetRepeatedPwdBox.Password)
            {
                App.pwd = SetRepeatedPwdBox.Password;
                MessageBox.Show("Password setted", "Notice");
            }
            else
            {
                MessageBox.Show("Passwords are not the same", "Error");
                SetPwdBox.Clear();
                SetRepeatedPwdBox.Clear();
            }
        }

        private void StopExitBotBtn_Click(object sender, RoutedEventArgs e)
        {
            MainBotCts.Cancel();
            LiveLogOutput.Clear();
            LiveLogOutput.Text = "Bot has been stopped";
            Environment.Exit(0);
        }
    }

}