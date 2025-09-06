using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using WelterKit.Extensions.SystemCommandLine;


namespace WelterKit.Extensions.SystemCommandLine;


public static class CommandExtensions {
   public static TCommand WithSubcommand<TCommand, TSubCommand>(this TCommand command,
                                                                TSubCommand subCommand)
         where TCommand : Command
         where TSubCommand : Command {
      command.Subcommands.Add(subCommand);
      return command;
   }


   public static TCommand WithAction<TCommand>(this TCommand command, Action<ParseResult> action) where TCommand : Command {
      command.SetAction(action);
      return command;
   }


   public static TCommand WithAction<TCommand>(this TCommand command, Func<ParseResult, Task> asyncAction) where TCommand : Command {
      command.SetAction(asyncAction);
      return command;
   }


   public static TCommand WithGlobalOption<TCommand, TOption>(this TCommand command,
                                                              TOption globalOption,
                                                              out TOption passBack)
         where TCommand : Command
         where TOption : Option {
      passBack = globalOption;
      return command.DoAndReturn(c => { c.AddGlobalOption(globalOption); });
   }


   public static TCommand WithOption<TCommand, TOption>(this TCommand command,
                                                        TOption Option,
                                                        out TOption passBack)
         where TCommand : Command
         where TOption : Option {
      passBack = Option;
      return command.DoAndReturn(c => { c.AddOption(Option); });
   }

}
