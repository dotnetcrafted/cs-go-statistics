using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Extensions;

namespace TelegramBot
{
    public class Bot
    {
        private readonly TelegramBotClient _client;
        private readonly ChatId _chatId;

        public Bot(string botToken, long chatId)
        {
            _client = new TelegramBotClient(botToken);
            _chatId = new ChatId(chatId);
        }

        public async Task SendMessage(string text, ParseMode mode)
        {
            if (text.IsEmpty())
            {
                return;
            }

            await _client.SendTextMessageAsync(_chatId, text, mode);
        }

        public async Task SendSticker(string stickerId)
        {
            await _client.SendStickerAsync(_chatId, stickerId);
        }

        public async Task<User> GetInfo()
        {
            return await _client.GetMeAsync();
        }
    }
}