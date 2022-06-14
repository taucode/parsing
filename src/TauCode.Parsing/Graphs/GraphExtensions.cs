using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs
{
    public static class GraphExtensions
    {
        public static ILinkableMold ResolvePath(this ILinkableMold linkableMold, string path)
        {
            if (linkableMold == null)
            {
                throw new ArgumentNullException(nameof(linkableMold));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.StartsWith("/"))
            {
                throw new NotImplementedException();
            }

            var parts = path.Split('/');

            IGroupMold currentGroup;

            if (linkableMold is IGroupMold groupMold)
            {
                currentGroup = groupMold;
            }
            else if (linkableMold is IVertexMold vertexMold)
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
                    return currentGroup;
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
                    else if (child is IGroupMold childGroupMold)
                    {
                        currentGroup = childGroupMold;
                    }
                    else if (child is IGroupRefMold groupRefMold)
                    {
                        if (i == parts.Length - 1)
                        {
                            return groupRefMold;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
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

        public static IVertexMold GetEntranceVertexOrThrow(this ILinkableMold linkableMold)
        {
            // todo checks
            var entranceVertex = linkableMold.GetEntranceVertex();
            if (entranceVertex == null)
            {
                throw new NotImplementedException("error: wanted entrance, but there is no one.");
            }

            return entranceVertex;
        }

        public static IVertexMold GetExitVertexOrThrow(this ILinkableMold linkableMold)
        {
            // todo checks
            var exitVertex = linkableMold.GetExitVertex();
            if (exitVertex == null)
            {
                throw new NotImplementedException("error: wanted exit, but there is no one.");
            }

            return exitVertex;
        }
    }
}
