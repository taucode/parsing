using System;
using System.Linq;
using System.Text;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs
{
    public static class GraphExtensions
    {
        public static IGroupMold GetRootGroupMold(this IScriptElementMold scriptElementMold)
        {
            if (scriptElementMold == null)
            {
                throw new ArgumentNullException(nameof(scriptElementMold));
            }

            var current = scriptElementMold;

            while (true)
            {
                if (current.Owner == null)
                {
                    if (current is IGroupMold rootGroup)
                    {
                        return rootGroup;
                    }
                    else
                    {
                        throw new GraphException("Error getting root group.");
                    }
                }

                current = current.Owner;
            }
        }

        public static ILinkableMold ResolvePath(this IScriptElementMold scriptElementMold, string path)
        {
            if (scriptElementMold == null)
            {
                throw new ArgumentNullException(nameof(scriptElementMold));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.StartsWith("/"))
            {
                #region deal with absolute path

                var root = scriptElementMold.GetRootGroupMold();

                var partsFromRoot = path.Split('/');
                var expectedRootName = partsFromRoot[1];
                if (root.Name != expectedRootName)
                {
                    throw new GraphException($"Path not found: '{path}'.");
                }

                if (partsFromRoot.Length == 3 && partsFromRoot[2] == "")
                {
                    // path is like '/root/' where 'root' is name of the root group
                    return root;
                }

                var sb = new StringBuilder();
                for (var i = 2; i < partsFromRoot.Length; i++)
                {
                    var part = partsFromRoot[i];
                    sb.Append(part);

                    if (i < partsFromRoot.Length - 1)
                    {
                        sb.Append("/");
                    }
                }

                var remainingPath = sb.ToString();
                return root.ResolvePath(remainingPath);

                #endregion
            }

            var parts = path.Split('/');

            IGroupMold currentGroup;

            if (scriptElementMold is IGroupMold groupMold)
            {
                currentGroup = groupMold;
            }
            else if (scriptElementMold is IVertexMold vertexMold)
            {
                currentGroup = vertexMold.Owner;
            }
            else if (scriptElementMold is IRefMold refMold)
            {
                currentGroup = refMold.Owner;
            }
            else if (scriptElementMold is IArcMold arcMold)
            {
                currentGroup = arcMold.Owner;
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
                        throw new GraphException($"Name part not found: '{part}'.");
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
                    else if (child is IRefMold refMold)
                    {
                        if (i == parts.Length - 1)
                        {
                            return refMold;
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
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            throw new NotImplementedException();
        }

        public static T GetKeywordValue<T>(this IScriptElementMold scriptElementMold, string keyword, T valueIfNull)
        {
            var value = scriptElementMold.GetKeywordValue(keyword);
            if (value == null)
            {
                return valueIfNull;
            }

            if (value is T t)
            {
                return t;
            }

            throw new NotImplementedException("error: not expected type");
        }

        public static T GetKeywordValue<T>(this IScriptElementMold scriptElementMold, string keyword)
        {
            var value = scriptElementMold.GetKeywordValue(keyword);
            if (value == null)
            {
                throw new GraphException($"Keyword not found: '{keyword}'.");
            }

            if (value is T t)
            {
                return t;
            }

            throw new NotImplementedException("error: not expected type");
        }

        public static IVertexMold GetEntranceVertexOrThrow(this ILinkableMold linkableMold)
        {
            // todo: cache for groups (via internal properties)?

            if (linkableMold == null)
            {
                throw new ArgumentNullException(nameof(linkableMold));
            }

            if (linkableMold is IVertexMold vertexMold)
            {
                return vertexMold;
            }

            if (linkableMold is IGroupMold groupMold)
            {
                var innerLinkables = groupMold.Linkables;
                foreach (var innerLinkable in innerLinkables)
                {
                    if (innerLinkable.IsEntrance)
                    {
                        return innerLinkable.GetEntranceVertexOrThrow();
                    }
                }

                throw new GraphException("Entrance expected to exist, but there is no one.");
            }

            if (linkableMold is IRefMold refMold)
            {
                var referencedMold = refMold.ResolvePath(refMold.ReferencedPath);
                return referencedMold.GetEntranceVertexOrThrow();
            }

            throw new NotImplementedException();

            //var entranceVertex = linkableMold.GetEntranceVertex();
            //if (entranceVertex == null)
            //{
            //    throw new GraphException("Entrance expected to exist, but there is no one.");
            //}

            //return entranceVertex;
        }

        public static IVertexMold GetExitVertexOrThrow(this ILinkableMold linkableMold)
        {
            // todo: cache for groups (via internal properties)?
            if (linkableMold == null)
            {
                throw new ArgumentNullException(nameof(linkableMold));
            }

            if (linkableMold is IVertexMold vertexMold)
            {
                return vertexMold;
            }

            if (linkableMold is IGroupMold groupMold)
            {
                var innerLinkables = groupMold.Linkables;
                foreach (var innerLinkable in innerLinkables)
                {
                    if (innerLinkable.IsExit)
                    {
                        return innerLinkable.GetExitVertexOrThrow();
                    }
                }

                throw new GraphException("Entrance expected to exist, but there is no one.");
            }

            if (linkableMold is IRefMold refMold)
            {
                var referencedMold = refMold.ResolvePath(refMold.ReferencedPath);
                return referencedMold.GetExitVertexOrThrow();
            }

            throw new NotImplementedException();

            //var exitVertex = linkableMold.GetExitVertex();
            //if (exitVertex == null)
            //{
            //    throw new GraphException("error: wanted exit, but there is no one.");
            //}

            //return exitVertex;
        }
    }
}
