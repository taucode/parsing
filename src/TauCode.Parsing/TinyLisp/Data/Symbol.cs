﻿using System.Diagnostics;

namespace TauCode.Parsing.TinyLisp.Data;

[DebuggerDisplay("{" + nameof(Name) + "}")]
public class Symbol : Atom
{
    /// <summary>
    /// todo: not a good thing. who will remove symbols when they are not used anymore? GC won't since this property is static.
    /// </summary>
    private static readonly Dictionary<string, Symbol> Symbols;

    static Symbol()
    {
        Symbols = new Dictionary<string, Symbol>();

        // touch NIL and T so they are created.
        Symbol dummy;
        dummy = Nil.Instance;
        dummy = True.Instance;
    }

    protected Symbol(string realName)
    {
        this.Name = realName;
    }

    public string Name { get; }

    public override bool Equals(Element? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.GetType() == this.GetType())
        {
            var otherSymbol = (Symbol)other;
            return this.Name.Equals(otherSymbol.Name);
        }

        return false;
    }

    protected override int GetHashCodeImpl() => this.Name.GetHashCode();

    public static Symbol Create(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (name.Length == 0)
        {
            throw new ArgumentException($"Symbol name cannot be empty.", nameof(name));
        }

        if (name[0] == ':')
        {
            return CreateKeyword(name);
        }
        else
        {
            return CreateSymbol(name);
        }
    }

    protected static void RegisterSymbol(Symbol symbol)
    {
        Symbols.Add(symbol.Name, symbol);
    }

    private static Symbol CreateSymbol(string name)
    {
        var validName = TinyLispHelper.IsValidSymbolName(name, false);
        if (!validName)
        {
            throw new ArgumentException($"Invalid symbol name: '{name}'.", nameof(name));
        }

        var realName = GetRealName(name);
        var have = Symbols.TryGetValue(realName, out var existing);
        if (have)
        {
            return existing!;
        }

        var @new = new Symbol(realName);
        RegisterSymbol(@new);
        return @new;
    }

    private static Keyword CreateKeyword(string name)
    {
        var validName = TinyLispHelper.IsValidSymbolName(name, true);
        if (!validName)
        {
            throw new ArgumentException($"Invalid keyword name: '{name}'.", nameof(name));
        }

        var realName = GetRealName(name);
        var have = Symbols.TryGetValue(realName, out var existing);
        if (have)
        {
            return (Keyword)existing!;
        }

        var @new = new Keyword(realName);
        RegisterSymbol(@new);
        return @new;
    }

    private static string GetRealName(string name) => name.ToUpperInvariant();

    public override string ToString() => Name;
}