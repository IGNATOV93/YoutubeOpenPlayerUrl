using IniParser;
using IniParser.Model;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace YoutubeOpenPotPlayerUrl.BotTelegram
{
    public abstract class BotTelegram
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tokens.ini");
        private static FileIniDataParser parser = new FileIniDataParser();
        private static IniData data = parser.ReadFile(path);
        static private TelegramBotClient client = new TelegramBotClient(data["Profile0"]["YourBotTelegreamToken"]);

        public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            string urlInput;
            var message = update.Message;
            if (message == null) { return; }

            if (message.Text != null && message.Text.Contains("https://youtu"))
            {
                Process[] processes = Process.GetProcessesByName(data["Profile0"]["NameProcessPlayer"]);
                foreach (Process process in processes)
                {
                    process.Kill();
                }

                try
                {
                    var p = new Process();
                    p.StartInfo.FileName = data["Profile0"][@"PatchOftePlayr"];// just for example, you can use yours.
                    p.StartInfo.Arguments = message.Text;
                    p.Start();
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "" +
                        "Запустил");
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
                return;
            }
        }
        public static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3) => throw new NotImplementedException();
        static public void StartsBot()
        {
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }
    }

}
