using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding
{
    public interface IScriptElementMold
    {
        IGroupMold Owner { get; }
        Atom Car { get; }
        string Name { get; set; }

        Element LispElement { get; }

        void SetKeywordValue(string keyword, object value);
        object GetKeywordValue(string keyword);
        IReadOnlyCollection<string> Keywords { get; }
        bool RemoveKeyword(string keyword);

        void ProcessKeywords();

        //bool IsFinalized { get; }
        //void ValidateAndFinalize();
    }
}
