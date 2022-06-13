using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Parsing.Cli.Result;

public class CliParsingResult : IParsingResult
{
    private string _command;
    private readonly Dictionary<string, List<string>> _keyValues;

    public CliParsingResult()
    {
        _keyValues = new Dictionary<string, List<string>>();
    }

    public void SetCommand(string command)
    {
        this.Command = command;
    }

    public void AddKeyValue(string alias, string value)
    {
        var list = _keyValues.GetValueOrDefault(alias);

        if (list == null)
        {
            list = new List<string>
            {
                value
            };
            _keyValues.Add(alias, list);
        }
        else
        {
            list.Add(value);
        }
    }

    public string Command
    {
        get => _command;
        private set
        {
            if (_command != null)
            {
                throw new NotImplementedException(); // duplicate action
            }

            _command = value;
        }
    }

    public IReadOnlyDictionary<string, List<string>> KeyValues => _keyValues;
    public int Version { get; private set; }

    public void IncreaseVersion()
    {
        this.Version++;
    }
}
