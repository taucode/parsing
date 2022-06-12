using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs
{
    // todo clean
    public static class GraphExtensions
    {
        public static ILinkableMold ResolvePath(this ILinkableMold partMold, string path)
        {
            // todo checks
            if (path.StartsWith("/"))
            {
                throw new NotImplementedException();
            }

            var parts = path.Split('/');

            IGroupMold currentGroup;

            if (partMold is IGroupMold groupMold)
            {
                currentGroup = groupMold;
            }
            else if (partMold is IVertexMold vertexMold)
            {
                currentGroup = vertexMold.Owner;
            }
            else
            {
                throw new NotImplementedException();
            }

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                if (part == ".")
                {
                    // 'currentGroup' doesn't change
                }
                else if (part == "..")
                {
                    currentGroup = currentGroup.Owner;
                    if (currentGroup == null)
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (part == "")
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // todo: check part is a valid name
                    var child = currentGroup.AllElements.SingleOrDefault(x => x.Name == part);
                    // todo: check name uniqueness; todo: forbid name change?

                    if (child == null)
                    {
                        throw new NotImplementedException();
                    }

                    if (child is IVertexMold vertexMold)
                    {
                        if (i == parts.Length - 1)
                        {
                            return vertexMold;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        public static bool GetKeywordValueAsBool(this IScriptElementMold scriptElementMold, string keyword)
        {
            var value = scriptElementMold.GetKeywordValue(keyword);
            if (value == null)
            {
                return false;
            }

            if (value is bool b)
            {
                return b;
            }

            throw new NotImplementedException("error: not expected type");
        }

        public static IVertexMold GetEntranceVertexOrThrow(this ILinkableMold part)
        {
            // todo checks
            var entranceVertex = part.GetEntranceVertex();
            if (entranceVertex == null)
            {
                throw new NotImplementedException("error: wanted entrance, but there is no one.");
            }

            return entranceVertex;
        }

        public static IVertexMold GetExitVertexOrThrow(this ILinkableMold part)
        {
            // todo checks
            var exitVertex = part.GetExitVertex();
            if (exitVertex == null)
            {
                throw new NotImplementedException("error: wanted exit, but there is no one.");
            }

            return exitVertex;
        }


        //public static IList<IPartMold> GetPartsOfGroup(
        //    this IGroupMold groupMold,
        //    out int? entrancePartIndex,
        //    out int? exitPartIndex,
        //    Action<int, IScriptElementMold> scriptElementCallback = null,
        //    Action<int, IPartMold> partCallback = null)
        //{
        //    // todo checks
        //    var list = new List<IPartMold>();

        //    entrancePartIndex = null;
        //    exitPartIndex = null;

        //    var index = -1;

        //    for (var i = 0; i < groupMold.Content.Count; i++)
        //    {
        //        var scriptElementMold = groupMold.Content[i];
        //        scriptElementCallback?.Invoke(i, scriptElementMold);

        //        if (scriptElementMold is IPartMold partMold)
        //        {
        //            index++;
        //            list.Add(partMold);

        //            if (partMold.IsEntrance)
        //            {
        //                if (entrancePartIndex != null)
        //                {
        //                    throw new NotImplementedException("error: more than one part with IsEntrance == true");
        //                }

        //                entrancePartIndex = index;
        //            }

        //            if (partMold.IsExit)
        //            {
        //                if (exitPartIndex != null)
        //                {
        //                    throw new NotImplementedException("error: more than one part with IsExit == true");
        //                }

        //                exitPartIndex = index;
        //            }

        //            partCallback?.Invoke(index, partMold);
        //        }
        //    }

        //    foreach (var scriptElementMold in groupMold.Content)
        //    {

        //    }

        //    return list;
        //}
    }
}
