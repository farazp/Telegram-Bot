using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using WhoisMasterBot.Models.Commands;

namespace WhoisMasterBot.Models
{
    public class Bot
    {
        public Bot(IOptions<BotConfiguration> _config)
        {

            config = _config;
            botConfiguration = _config.Value;
        }
        private static TelegramBotClient botClient;
        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        private readonly IOptions<BotConfiguration> config;
        private BotConfiguration botConfiguration = new BotConfiguration();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }


            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            //TODO: Add more commands
            var _token = Startup.StaticConfig.GetSection("BotConfiguration:BotToken").Value;
            var _url = Startup.StaticConfig.GetSection("BotConfiguration:Url").Value;

            botClient = new TelegramBotClient(token: _token);
            botClient.Timeout = new TimeSpan(0, 0, 360);
            string hook = string.Format(_url, "api/DomainWhois");
            await botClient.SetWebhookAsync(hook);

            return botClient;
        }
    }
}
