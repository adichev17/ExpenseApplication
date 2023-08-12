namespace MessageBus.Common
{
    public static class MessageBusConstants
    {
        //Exchanges
        public const string ExchangeUsers = "user";
        public const string ExchangeFns = "fns";

        //Queue
        public const string QueueUserRegister = "user.register";
        public const string QueueFnsPhone = "phone.code";

        //RouteKeys
        public const string UserRegisterKey = "user.register-key";
    }
}
