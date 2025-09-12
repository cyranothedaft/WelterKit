using System;
using System.CommandLine;
using System.Threading.Tasks;


namespace WelterKit.Extensions.SystemCommandLine;


public static class CommandExtensions {
   public static TCommand WithSubcommand<TCommand, TSubCommand>(this TCommand command,
                                                                TSubCommand subCommand)
         where TCommand : Command
         where TSubCommand : Command {
      command.Subcommands.Add(subCommand);
      return command;
   }


   public static TCommand WithAction<TCommand>(this TCommand command, Func<ParseResult, int> action) where TCommand : Command {
      command.SetAction(action);
      return command;
   }


   public static TCommand WithAction<TCommand>(this TCommand command, Func<ParseResult, Task> asyncAction) where TCommand : Command {
      command.SetAction(asyncAction);
      return command;
   }


   public static TCmd WithArgument<TCmd, TArg>(this TCmd command, TArg argument, out TArg passBack)
         where TCmd : Command
         where TArg : Argument {
      passBack = argument;
      command.Add(argument);
      return command;
   }


   public static TCmd WithOption<TCmd, TOpt>(this TCmd command, TOpt option, out TOpt passBack)
         where TCmd : Command
         where TOpt : Option {
      passBack = option;
      command.Add(option);
      return command;
   }

}
