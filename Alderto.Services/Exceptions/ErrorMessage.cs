namespace Alderto.Services.Exceptions
{
    public class ErrorMessage
    {
        public int HttpCode { get; set; }
        public int InternalErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorMessage(int httpCode, int internalCode, string message)
        {
            HttpCode = httpCode;
            InternalErrorCode = internalCode;
            Message = message;
        }
    }

    public static class ErrorMessages
    {
        // === Forbid ===

        /// <summary>
        /// Forbid reason. Used when unable to confirm admin status.
        /// </summary>
        public static ErrorMessage UserNotDiscordAdmin { get; } =
            new ErrorMessage(403, 1100, "Could not confirm if user is an admin of the specified guild.");

        /// <summary>
        /// Forbid reason. Used when unable to confirm bank moderator status.
        /// </summary>
        public static ErrorMessage UserNotBankModerator { get; } =
            new ErrorMessage(403, 1200, "Could not confirm if user has access to add or remove items form the specified bank.");

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

        /// <summary>
        /// NotFound reason. Used when bot was unable to find the bank.
        /// </summary>
        public static ErrorMessage BankNotFound { get; } =
            new ErrorMessage(404, 2200, "The specified bank was not found.");

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
        /// BadRequest reason. Used received name was empty.
        /// </summary>
        public static ErrorMessage NameIsNull { get; } =
            new ErrorMessage(400, 3300, "Name cannot be empty.");


        public static ErrorMessage FromCode(int code)
        {
            return code switch
            {
                1100 => UserNotDiscordAdmin,
                1200 => UserNotBankModerator,
                2100 => GuildNotFound,
                2101 => UserNotFound,
                2102 => ChannelNotFound,
                2200 => BankNotFound,
                3000 => MissingChannelAccess,
                3001 => MissingWritePermissions,
                3100 => PayloadOver100,
                3200 => BankNameAlreadyExists,
                3300 => NameIsNull,
                _ => null
            };
        }
    }
}