using System;

namespace Alderto.Services.Exceptions
{
    public class ErrorMessage
    {
        /// <summary>
        /// HTTP Status code. 400, 403, or 404
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Internal status code. Consult documentation for a list of codes.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Human readable error message.
        /// </summary>
        public string Message { get; set; }

        public ErrorMessage(int status, int internalCode, string message)
        {
            Status = status;
            Code = internalCode;
            Message = message;
        }
    }

    public static class ErrorMessages
    {
        // === Forbid ===

        /// <summary>
        /// Forbid reason. Used when unable to confirm admin status.
        /// </summary>
        public static ErrorMessage UserNotGuildAdmin { get; } =
            new ErrorMessage(403, 1100, "Could not confirm if user is an admin of the specified guild.");

        /// <summary>
        /// Forbid reason. Used when unable to confirm bank moderator status.
        /// </summary>
        public static ErrorMessage UserNotGuildModerator { get; } =
            new ErrorMessage(403, 1200, "Could not confirm if user has moderation access to the specified resource.");

        // === NotFound ===

        /// <summary>
        /// NotFound reason. Used when bot was unable to find the guild.
        /// </summary>
        public static ErrorMessage GuildNotFound { get; } =
            new ErrorMessage(404, 2100, "The specified guild was not found.");

        /// <summary>
        /// NotFound reason. Used when bot was unable to find the user.
        /// </summary>
        public static ErrorMessage UserNotFound { get; } =
            new ErrorMessage(404, 2101, "The specified user was not found.");

        /// <summary>
        /// NotFound reason. Used when bot was unable to find the channel.
        /// </summary>
        public static ErrorMessage ChannelNotFound { get; } =
            new ErrorMessage(404, 2102, "The specified channel was not found.");

        public static ErrorMessage MessageNotFound { get; } =
            new ErrorMessage(404, 2103, "The specified message was not found.");

        /// <summary>
        /// NotFound reason. Used when bot was unable to find the bank.
        /// </summary>
        public static ErrorMessage BankNotFound { get; } =
            new ErrorMessage(404, 2200, "The specified bank was not found.");

        /// <summary>
        /// NotFound reason. Used when bot was unable to find the bank item.
        /// </summary>
        public static ErrorMessage BankItemNotFound { get; } =
            new ErrorMessage(404, 2201, "The specified bank item was not found.");

        // === BadRequest ===

        /// <summary>
        /// BadRequest reason. Used when the bot does not have access to a particular resource.
        /// </summary>
        public static ErrorMessage MissingChannelAccess { get; } =
            new ErrorMessage(400, 3000,
                "The bot does not have access to this channel. Make sure the bot has Read Messages permission.");

        /// <summary>
        /// BadRequest reason. Used when the bot does not have permissions to a perform a particular action.
        /// </summary>
        public static ErrorMessage MissingWritePermissions { get; } =
            new ErrorMessage(400, 3001,
                "The bot does not have Send Messages or Embed Links permissions in this channel. To function properly, the bot requires both of these permissions.");

        /// <summary>
        /// BadRequest reason. Used when received over 100 elements in a request.
        /// </summary>
        public static ErrorMessage PayloadOver100 { get; } =
            new ErrorMessage(400, 3100, "Request payload cannot exceed 100 elements.");

        /// <summary>
        /// BadRequest reason. Used when bank with the given name already exists.
        /// </summary>
        public static ErrorMessage BankNameAlreadyExists { get; } =
            new ErrorMessage(400, 3200, "A bank with the given name already exists.");

        /// <summary>
        /// BadRequest reason. Used when received name was empty.
        /// </summary>
        public static ErrorMessage NameCannotBeNull { get; } =
            new ErrorMessage(400, 3300, "Name cannot be empty.");

        /// <summary>
        /// BadRequest reason. Used when received name was empty.
        /// </summary>
        public static ErrorMessage BotNotMessageOwner { get; } =
            new ErrorMessage(400, 3400, "The bot is not the owner of the specified message.");

        public static ErrorMessage ChannelNotMessageChannel { get; } =
            new ErrorMessage(400, 3401, "The specified channel is not a message channel.");
        


        public static ErrorMessage FromCode(int code)
        {
            return code switch
            {
                1100 => UserNotGuildAdmin,
                1200 => UserNotGuildModerator,
                2100 => GuildNotFound,
                2101 => UserNotFound,
                2102 => ChannelNotFound,
                2103 => MessageNotFound,
                2200 => BankNotFound,
                2201 => BankItemNotFound,
                3000 => MissingChannelAccess,
                3001 => MissingWritePermissions,
                3100 => PayloadOver100,
                3200 => BankNameAlreadyExists,
                3300 => NameCannotBeNull,
                3400 => BotNotMessageOwner,
                3401 => ChannelNotMessageChannel,
                _ => throw new ArgumentException($"Provided internal {nameof(code)} was not found.")
            };
        }
    }
}