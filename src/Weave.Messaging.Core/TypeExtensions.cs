using System;
using System.Linq;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;

namespace Weave.Messaging.Core
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsQueryHandlerType(this Type type)
        {
            return type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCommandHandlerType(this Type type)
        {
            return type.GetInterfaces()
                .Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                                              i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));
        }

        public static bool IsEventHandlerType(this Type type)
        {
            return type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSagaType(this Type type)
        {
            return true;
        }

        public static Type GetQueryHandlerType(this Type type)
        {
            return type;
        }

        public static Type GetQueryMessageType(this Type type)
        {
            var queries = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                .Select(i => i.GetGenericArguments().First());

            return queries.First();
        }

        public static Type GetCommandMessageType(this Type type)
        {
            var commands = type.GetInterfaces()
                .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                                                i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
                .Select(i => i.GetGenericArguments().First());

            return commands.First();
        }

        public static Type GetEventMessageType(this Type type)
        {
            var events = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                .Select(i => i.GetGenericArguments().First());

            return events.First();
        }

        public static Type GetResponseMessage(this Type type)
        {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryMessage<,>))
                .Select(i => i.GetGenericArguments().Last()).First();
        }
        
        public static Type GetResponseMessage2(this Type type)
        {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandMessage<,>))
                .Select(i => i.GetGenericArguments().Last()).First();
        }
    }
}
