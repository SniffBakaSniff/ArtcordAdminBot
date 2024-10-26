using System.ComponentModel;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

public enum MessageDeletionTimeframe
{
    [ChoiceDisplayName("None")]
    None = 0,

    [ChoiceDisplayName("Last 1 Day")]
    Last1Day = 1,

    [ChoiceDisplayName("Last 2 Days")]
    Last2Days = 2,

    [ChoiceDisplayName("Last 3 Days")]
    Last3Days = 3,

    [ChoiceDisplayName("Last 4 Days")]
    Last4Days = 4,

    [ChoiceDisplayName("Last 5 Days")]
    Last5Days = 5,

    [ChoiceDisplayName("Last 6 Days")]
    Last6Days = 6,

    [ChoiceDisplayName("Last 7 Days")]
    Last7Days = 7 
}
